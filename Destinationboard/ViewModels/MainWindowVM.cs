using Destinationboard.Common;
using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Destinationboard.Views;
using Grpc.Core;
using PCSC;
using PCSC.Iso7816;
using PCSC.Monitoring;
using QRCodeScannerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Destinationboard.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        DispatcherTimer _timer = new DispatcherTimer();
        DispatcherTimer _timer2 = new DispatcherTimer();
        PasoriUtil _Pasori = new PasoriUtil();

        #region PaSoriで読み取ったイベントの受信
        /// <summary>
        /// PaSoriで読み取ったイベントの受信
        /// </summary>
        /// <param name="readername">リーダーの名称</param>
        /// <param name="unknown">イベント</param>
        private void PasoriReaded(string readername, CardStatusEventArgs unknown)
        {
            try
            {
                System.Media.SoundPlayer player = null;
                string SoundFile = @"Common\Sound\FelicaReaded.wav";

                // 再生ファイルの確認
                if (File.Exists(SoundFile))
                {
                    player = new System.Media.SoundPlayer(SoundFile);
                    player.Play();
                }

                string id = PasoriUtil.GetUID(readername);
                // スレッドセーフな呼び出し
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    var action_plan = (from x in this.ActionPlans.Items
                                       where x.FelicaID.Equals(id)
                                       select x).FirstOrDefault();

                    if (action_plan != null)
                    {
                        MoveRegistBeginFinish(action_plan);
                    }
                }));

                // 再生の停止
                if (player != null)
                {
                    player.Stop();
                    player.Dispose();
                    player = null;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowVM()
        {
            // コンフィグデータの読み込み
            CommonValues.GetInstance().ReadConfig();

        }
        #endregion

        #region 現在時刻[CurrentTime]プロパティ
        /// <summary>
        /// 現在時刻[CurrentTime]プロパティ用変数
        /// </summary>
        DateTime _CurrentTime = DateTime.Now;
        /// <summary>
        /// 現在時刻[CurrentTime]プロパティ
        /// </summary>
        public DateTime CurrentTime
        {
            get
            {
                return _CurrentTime;
            }
            set
            {
                if (!_CurrentTime.Equals(value))
                {
                    _CurrentTime = value;
                    NotifyPropertyChanged("CurrentTime");
                }
            }
        }
        #endregion

        #region 現在時刻を更新するタイマー処理
        /// <summary>
        /// 現在時刻を更新するタイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            // 日付の変化チェック
            if (!this.CurrentTime.Date.Equals(DateTime.Today))
            {
                // 日付が変わったので全ての日付データを更新
                GetPlans();
            }
            
            this.CurrentTime = DateTime.Now;    // 現在時刻の更新
        }
        #endregion

        #region 計画のリフレッシュタイマー
        /// <summary>
        /// 計画のリフレッシュタイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Refresh_Tick(object sender, EventArgs e)
        {
            GetPlans();
        }
        #endregion

        #region 行動予定一覧[ActionPlans]プロパティ
        /// <summary>
        /// 行動予定一覧[ActionPlans]プロパティ用変数
        /// </summary>
        ActionPlanCollectionM _ActionPlans = new ActionPlanCollectionM();
        /// <summary>
        /// 行動予定一覧[ActionPlans]プロパティ
        /// </summary>
        public ActionPlanCollectionM ActionPlans
        {
            get
            {
                return _ActionPlans;
            }
            set
            {
                if (_ActionPlans == null || !_ActionPlans.Equals(value))
                {
                    _ActionPlans = value;
                    NotifyPropertyChanged("ActionPlans");
                }
            }
        }
        #endregion

        #region イベントの初期化処理
        /// <summary>
        /// イベントの初期化処理
        /// </summary>
        public void InitEvent()
        {
            foreach (var staff in this.ActionPlans.Items)
            {
                staff.ClickRegistBeginFinish -= Staff_ClickRegistBeginFinish;
                staff.ClickRegistBeginFinish += Staff_ClickRegistBeginFinish;

                staff.ClickRegistAction -= Staff_ClickRegistAction;
                staff.ClickRegistAction += Staff_ClickRegistAction;

                staff.ClickMemo -= Staff_ClickMemo;
                staff.ClickMemo += Staff_ClickMemo;

                staff.ClickStartTime -= Staff_ClickStartTime;
                staff.ClickStartTime += Staff_ClickStartTime;

                staff.ClickEndTime -= Staff_ClickEndTime;
                staff.ClickEndTime += Staff_ClickEndTime;
            }
        }

        #region 終了時刻のボタン押下処理
        /// <summary>
        /// 終了時刻のボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Staff_ClickEndTime(object sender, EventArgs ev)
        {
            try
            {
                // 終了時刻での呼び出し
                MoveRegistTimeV(sender, ev);
                GetPlans();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 開始時刻の登録処理
        /// <summary>
        /// 開始時刻の登録処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Staff_ClickStartTime(object sender, EventArgs e)
        {
            try
            {
                // 開始時刻での呼び出し
                MoveRegistTimeV(sender, e);
                GetPlans();

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 備考情報の登録処理
        /// <summary>
        /// 備考情報の登録処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Staff_ClickMemo(object sender, EventArgs e)
        {
            try
            {
                RegistMemoV wnd = new RegistMemoV();
                RegistMemoVM vm = wnd.DataContext as RegistMemoVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    var action_plan = sender as ActionPlanM;
                    action_plan.Memo = vm.InputText;

                    // 行動予定の登録
                    ActionPlanM.RegistActionPlanTable(action_plan);

                    GetPlans();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 行先情報の登録処理
        /// <summary>
        /// 行先情報の登録処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void Staff_ClickRegistAction(object sender, EventArgs ev)
        {
            try
            {
                RegistActionV wnd = new RegistActionV();
                RegistActionVM vm = wnd.DataContext as RegistActionVM;
                wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;  // 中央に表示する
                wnd.WindowStyle = System.Windows.WindowStyle.None;

                var action_plan = sender as ActionPlanM;
                vm.ActionPlan.Copy(action_plan);

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    // 行動予定の登録
                    ActionPlanM.RegistActionPlanTable(vm.ActionPlan);
                    GetPlans();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 出勤・退勤入力画面への遷移
        /// <summary>
        /// 出勤・退勤入力画面への遷移
        /// </summary>
        /// <param name="action_plan">行動予定</param>
        public void MoveRegistBeginFinish(ActionPlanM action_plan)
        {
            try
            {
                var wnd = new RegistBeginFinishV();
                wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;  // 中央に表示する
                var vm = wnd.DataContext as RegistBeginFinishVM;

                vm.ActionPlan.Copy(action_plan);

                // 画面遷移
                if (wnd.ShowDialog() == true)
                {
                    // 行動予定の登録
                    ActionPlanM.RegistActionPlanTable(vm.ActionPlan);
                    GetPlans();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 出勤・退勤入力画面への遷移
        /// <summary>
        /// 出勤・退勤入力画面への遷移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Staff_ClickRegistBeginFinish(object sender, EventArgs e)
        {
            var action_plan = sender as ActionPlanM;
            MoveRegistBeginFinish(action_plan);
        }
        #endregion
        #endregion

        #region ハンディスキャナの初期化処理
        /// <summary>
        /// ハンディスキャナの初期化処理
        /// </summary>
        public void HandyScannerInit()
        {
            // スキャナを使用するかどうかの判定
            if( CommonValues.GetInstance().EnableHandyScanner)
            {
                int port = CommonValues.GetInstance().HandyScannerComPort;  // シリアルポート指定
                CommonValues.GetInstance().Scanner = new QRCodeScannerLib.ScannerManager(port);
                CommonValues.GetInstance().Scanner.Connect();   // 接続

                CommonValues.GetInstance().Scanner.DataReceived -= _SerialPort_DataReceived;
                CommonValues.GetInstance().Scanner.DataReceived += _SerialPort_DataReceived;
            }
        }
        #endregion

        #region スキャナの読み取り受信処理
        /// <summary>
        /// スキャナの読み取り受信処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _SerialPort_DataReceived(object sender, EventArgs e)
        {
            var ev = e as ScannerDataRecieveEventArgs;
            string Message = ev.Message;

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                var tmp = (from x in this.ActionPlans.Items
                           where x.QRCode.Equals(Message)
                           select x).FirstOrDefault();

                if (tmp != null)
                {
                    MoveRegistBeginFinish(tmp);
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
                _timer.Tick += timer_Tick;
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Start();

                _timer2.Tick += Refresh_Tick;
                _timer2.Interval = new TimeSpan(0, 0, 30);
                _timer2.Start();

                GetPlans();

                // ハンディスキャナの初期化
                HandyScannerInit();

                // Pasoriの接続状態を確認
                if (_Pasori.ReaderNames.Count > 0)
                {
                    _Pasori.Monitor.CardInserted += (sender, args) => PasoriReaded(_Pasori.ReaderNames.First(), args);
                    _Pasori.MonitorStart();
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 行動予定の取得処理
        /// <summary>
        /// 行動予定の取得処理
        /// </summary>
        public void GetPlans()
        {
            try
            {
                // チャネルの作成
                var channel = new Grpc.Core.Channel(CommonValues.GetInstance().ServerName,
                    CommonValues.GetInstance().Port, ChannelCredentials.Insecure);

                // クライアントの作成
                var client = new DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient(channel);

                // リクエストの作成
                GetActionPlansRequest tmp = new GetActionPlansRequest();
                GetStaffsRequest staff_tmp = new GetStaffsRequest();

                // データの取得
                var reply = client.GetActionPlans(tmp);
                var staff_reply = client.GetStaffs(staff_tmp);

                // データの移し替え
                ActionPlanCollectionM action_list = new ActionPlanCollectionM();

                var staffinf_list = (from x in staff_reply.StaffInfoList
                                     where x.Display.Equals(true)
                                     orderby x.SortOrder
                                     select x);

                // スタッフマスターの情報をベースに行動一覧を作成
                foreach (var staff in staffinf_list)
                {
                    // スタッフマスターの情報の該当する
                    // 行動情報を取得する
                    var staff_action = (from x in reply.ActionPlans
                                      where x.StaffID.Equals(staff.StaffID)
                                      select x).FirstOrDefault();

                    // 非表示の場合
                    if (!staff.Display)
                        continue;

                    // 行動情報が見つかった場合
                    if (staff_action != null)
                    {
                        staff_action.StaffName = staff.StaffName;   // 名前は従業員マスターのものを使用する
                        var action_plan = new ActionPlanM(staff_action);
                        action_plan.QRCode = staff.QRCode;      // 従業員識別用QRコード
                        action_plan.FelicaID = staff.FelicaID;  // 従業員識別用FelicaID

                        // その行動情報を登録する
                        action_list.Add(action_plan);
                    }
                    else
                    {
                        // 空の行動情報を用意する（従業員IDと従業員名のみ）
                        var emply_action_plan = new ActionPlanM();
                        emply_action_plan.StaffID = staff.StaffID;
                        emply_action_plan.StaffName = staff.StaffName;
                        emply_action_plan.QRCode = staff.QRCode;
                        emply_action_plan.FelicaID = staff.FelicaID;

                        action_list.Add(emply_action_plan);
                    }
                }

                // 画面に表示
                this.ActionPlans = action_list;

                // イベント登録処理
                InitEvent();

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 設定画面への画面遷移
        /// <summary>
        /// 設定画面への画面遷移
        /// </summary>
        public void MoveSettingApplicationV()
        {
            try
            {
                SettingApplicationV wnd = new SettingApplicationV();
                SettingApplicationVM vm = wnd.DataContext as SettingApplicationVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    GetPlans();
                }

            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 従業員設定画面への画面遷移
        /// <summary>
        /// 従業員設定画面への画面遷移
        /// </summary>
        public void MoveSettingStaffV()
        {
            try
            {
                SettingStaffV wnd = new SettingStaffV();
                SettingStaffVM vm = wnd.DataContext as SettingStaffVM;

                if (CommonValues.GetInstance().EnableHandyScanner)
                {
                    CommonValues.GetInstance().Scanner.DataReceived -= _SerialPort_DataReceived;
                    CommonValues.GetInstance().Scanner.Disconnect();
                }

                // Pasoriのモニタリングストップ
                _Pasori.MonitorStop();

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    GetPlans();
                }

                // Pasoriのモニタリングスタート
                _Pasori.MonitorStart();

                if (CommonValues.GetInstance().EnableHandyScanner)
                {
                    CommonValues.GetInstance().Scanner.Connect();
                    CommonValues.GetInstance().Scanner.DataReceived += _SerialPort_DataReceived;
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 行動設定画面への画面遷移
        /// <summary>
        /// 行動設定画面への画面遷移
        /// </summary>
        public void MoveSettingActionV()
        {
            try
            {
                SettingActionV wnd = new SettingActionV();
                SettingActionVM vm = wnd.DataContext as SettingActionVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    GetPlans();
                }

            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 時間登録画面への画面遷移
        /// <summary>
        /// 時間登録画面への画面遷移
        /// </summary>
        public void MoveRegistTimeV(object sender, EventArgs ev)
        {
            try
            {
                RegistTimeV wnd = new RegistTimeV();
                wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;  // 中央に表示する
                wnd.WindowStyle = System.Windows.WindowStyle.None;

                RegistTimeVM vm = wnd.DataContext as RegistTimeVM;
                var action_plan = sender as ActionPlanM;
                vm.ActionPlan.Copy(action_plan);

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    // 行動予定の登録
                    ActionPlanM.RegistActionPlanTable(vm.ActionPlan);
                    GetPlans();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
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
                _timer.Stop();  // タイマー破棄
                _timer2.Stop(); // タイマー破棄

                // スキャナを使用するかどうかの判定
                if (CommonValues.GetInstance().EnableHandyScanner)
                {
                    CommonValues.GetInstance().Scanner.Disconnect();
                    CommonValues.GetInstance().Scanner.Dispose();
                }

                Environment.Exit(0);
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion
    }
}
