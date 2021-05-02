using Destinationboard.Common;
using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Grpc.Core;
using QRCodeScannerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
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

        #region 受信データ[Message]プロパティ
        /// <summary>
        /// 受信データ[Message]プロパティ用変数
        /// </summary>
        string _Message = string.Empty;
        /// <summary>
        /// 受信データ[Message]プロパティ
        /// </summary>
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                if (!_Message.Equals(value))
                {
                    _Message = value;
                    NotifyPropertyChanged("Message");
                }
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
            this.Message = ev.Message;

            if (this.StaffItems.SelectedItem != null)
            {
                this.StaffItems.SelectedItem.QRCode = this.Message;
            }
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

        #region スタッフの登録処理
        /// <summary>
        /// スタッフの登録処理
        /// </summary>
        public void RegistStaffMaster()
        {
            try
            {
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
