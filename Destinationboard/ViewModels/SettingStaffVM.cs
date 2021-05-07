using Destinationboard.Common;
using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Grpc.Core;
using PCSC;
using PCSC.Iso7816;
using PCSC.Monitoring;
using QRCodeScannerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Destinationboard.ViewModels
{
    public class SettingStaffVM : ViewModelBase
    {
        #region 従業員情報[StaffItems]プロパティ
        /// <summary>
        /// 従業員情報[StaffItems]プロパティ用変数
        /// </summary>
        StaffInfoCollectionM _StaffItems = new StaffInfoCollectionM();
        /// <summary>
        /// 従業員情報[StaffItems]プロパティ
        /// </summary>
        public StaffInfoCollectionM StaffItems
        {
            get
            {
                return _StaffItems;
            }
            set
            {
                if (_StaffItems == null || !_StaffItems.Equals(value))
                {
                    _StaffItems = value;
                    NotifyPropertyChanged("StaffItems");
                }
            }
        }
        #endregion

        #region ソート順の更新
        /// <summary>
        /// ソート順の更新
        /// </summary>
        public void RefreshSortOrder()
        {
            for (int iCnt = 0; iCnt < this.StaffItems.Items.Count; iCnt++)
            {
                var staff = this.StaffItems.ElementAt(iCnt);
                staff.SortOrder = iCnt;
            }
        }
        #endregion

        #region スキャナの初期化処理
        /// <summary>
        /// スキャナの初期化処理
        /// </summary>
        public void ScannerInitialize()
        {
            // スキャナを使用する場合
            if (CommonValues.GetInstance().EnableHandyScanner)
            {
                // 接続処理
                CommonValues.GetInstance().Scanner.Connect();

                // イベントを一旦クリア
                CommonValues.GetInstance().Scanner.DataReceived -= _SerialPort_DataReceived;

                // イベント登録
                CommonValues.GetInstance().Scanner.DataReceived += _SerialPort_DataReceived;
            }
        }
        #endregion

        #region イベント処理
        /// <summary>
        /// イベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _SerialPort_DataReceived(object sender, EventArgs e)
        {
            var ev = e as ScannerDataRecieveEventArgs;

            // スレッドセーフな呼び出し
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (this.StaffItems.SelectedItem != null)
                {
                    this.StaffItems.SelectedItem.QRCode = ev.Message;
                }
            }));
        }
        #endregion

        ISCardMonitor _Monitor;

        #region 表示イベント
        /// <summary>
        /// 表示イベント
        /// </summary>
        /// <param name="readername">リーダーの名称</param>
        /// <param name="unknown">イベント</param>
        private void DisplayEvent(string readername, CardStatusEventArgs unknown)
        {
            string id = PasoriUtil.GetUID(readername);
            // スレッドセーフな呼び出し
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (this.StaffItems.SelectedItem != null)
                {
                    this.StaffItems.SelectedItem.FelicaID = id;
                }
            }));
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Init()
        {
            try
            {
                // Retrieve the names of all installed readers.
                var readerNames = PasoriUtil.GetReaderNames();

                if (!PasoriUtil.IsEmpty(readerNames))
                {
                    _Monitor = MonitorFactory.Instance.Create(SCardScope.System);
                    _Monitor.CardInserted += (sender, args) => DisplayEvent(readerNames.First(), args);
                    _Monitor.Start(readerNames.First());
                }

                // チャネルの作成
                var channel = new Grpc.Core.Channel(CommonValues.GetInstance().ServerName,
                    CommonValues.GetInstance().Port, ChannelCredentials.Insecure);

                // クライアントの作成
                var client = new DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient(channel);

                // リクエストの作成
                GetStaffsRequest tmp = new GetStaffsRequest();
                
                // データの取得
                var reply = client.GetStaffs(tmp);

                // データの移し替え
                StaffInfoCollectionM staff_list = new StaffInfoCollectionM();


                // スタッフ情報のリスト作成
                foreach (var reply_item in reply.StaffInfoList)
                {
                    staff_list.Add(reply_item);
                }

                staff_list.Items = new System.Collections.ObjectModel.ObservableCollection<StaffInfoM>(staff_list.Items.OrderBy(x => x.SortOrder));

                // 画面に表示
                this.StaffItems = staff_list;

                // スキャナ初期化処理
                ScannerInitialize();
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 主キーのチェック
        /// <summary>
        /// 主キーのチェック
        /// </summary>
        /// <returns>true:重複している false:重複していない</returns>
        public bool CheckPrimaryKey()
        {
            var empty_key_count = (from x in this.StaffItems.Items
                                  where string.IsNullOrWhiteSpace(x.StaffID)
                                  select x).Count();

            if (empty_key_count > 0)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("入力されていない従業員IDがあります。");
                msg.AppendLine("従業員IDは入力必須です。");
                ShowMessage.ShowNoticeOK(msg.ToString(), "通知");
                return false;

            }

            var staffid_count = this.StaffItems.Items
                                    .GroupBy(c => c.StaffID)
                                    .Select(c => new { StaffID = c.Key, Count = c.Count() })
                                    .OrderBy(c => c.StaffID);

            var multi_keys = staffid_count.Where(c => c.Count > 1);
            if (multi_keys.Count() > 0)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("以下の従業員IDが重複しています");
                msg.AppendLine("重複従業員ID:");

                foreach (var tmp in multi_keys)
                {
                    msg.AppendLine(tmp.StaffID);
                }

                ShowMessage.ShowNoticeOK(msg.ToString(), "通知");

                return false;
            }
            return true;
        }
        #endregion

        #region スタッフの登録処理
        /// <summary>
        /// スタッフの登録処理
        /// </summary>
        public void RegistStaffMaster()
        {
            try
            {
                if (!CheckPrimaryKey())
                    return;


                // チャネルの取得
                var channel = new Grpc.Core.Channel(CommonValues.GetInstance().ServerName, CommonValues.GetInstance().Port,
                    ChannelCredentials.Insecure);
                var client = new DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient(channel);

                // リクエストの作成
                RegistStaffRequest request = new RegistStaffRequest();

                // ユーザー名の作成
                request.IP = CommonValues.GetInstance().OwnIP;

                RefreshSortOrder(); // ソート順の更新

                // スタッフ情報の作成
                foreach (var tmp in this.StaffItems.Items)
                {
                    StaffMasterRequest staff_item = new StaffMasterRequest();
                    staff_item.StaffID = tmp.StaffID;
                    staff_item.SortOrder = tmp.SortOrder;
                    staff_item.StaffName = tmp.StaffName;
                    staff_item.Display = tmp.Display;
                    staff_item.QRCode = tmp.QRCode;
                    staff_item.FelicaID = tmp.FelicaID;
                    staff_item.CreateUser = Environment.UserName;
                    staff_item.CreateDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    request.StaffInfoList.Add(staff_item);
                }

                // 送信
                var reply = client.RegistStaff(request);


                ShowMessage.ShowNoticeOK("登録しました", "通知");

                // ハンディスキャナのコネクションの終了処理
                CloseConnection();

                // 閉じる処理
                this.DialogResult = true;

            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region コネクションの終了処理
        /// <summary>
        /// コネクションの終了処理
        /// </summary>
        public void CloseConnection()
        {
            if (CommonValues.GetInstance().EnableHandyScanner)
            {
                // イベント登録されている場合
                if (CommonValues.GetInstance().Scanner.DataReceived != null)
                {
                    CommonValues.GetInstance().Scanner.DataReceived -= _SerialPort_DataReceived;
                }

                // 接続中の場合
                if (CommonValues.GetInstance().Scanner.IsConnect)
                {
                    CommonValues.GetInstance().Scanner.Disconnect();
                }
            }
        }
        #endregion

        #region パソリの終了処理
        /// <summary>
        /// パソリの終了処理
        /// </summary>
        public void ClosePasori()
        {
            try
            {
                _Monitor.Cancel();
                _Monitor.CardInserted -= (sender, args) => { }; // イベントの初期化
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 閉じる処理
        /// <summary>
        /// 閉じる処理
        /// </summary>
        public override void Close()
        {
            try
            {
                if (ShowMessage.ShowQuestionYesNo("変更内容は登録されません。よろしいですか？", "確認") == System.Windows.MessageBoxResult.Yes)
                {
                    // ハンディスキャナのコネクションの終了処理
                    CloseConnection();

                    this.DialogResult = false;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 上へ移動
        /// <summary>
        /// 上へ移動
        /// </summary>
        public void MoveUp()
        {
            try
            {
                this.StaffItems.MoveUp();
                RefreshSortOrder(); // ソート順の更新
                NotifyPropertyChanged("StaffItems");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 下へ移動処理
        /// <summary>
        /// 下へ移動処理
        /// </summary>
        public void MoveDown()
        {
            try
            {
                this.StaffItems.MoveDown();
                RefreshSortOrder(); // ソート順の更新
                NotifyPropertyChanged("StaffItems");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 削除処理
        /// <summary>
        /// 削除処理
        /// </summary>
        public void Delete()
        {
            try
            {
                this.StaffItems.DeleteSelectedItem();
                RefreshSortOrder(); // ソート順の更新
                NotifyPropertyChanged("StaffItems");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion
    }
}
