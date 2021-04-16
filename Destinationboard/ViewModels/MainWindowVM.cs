using Destinationboard.Common;
using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Destinationboard.Views;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Destinationboard.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowVM()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
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
            this.CurrentTime = DateTime.Now;    // 現在時刻の更新
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

                    // 行動情報が見つかった場合
                    if (staff_action != null)
                    {
                        // その行動情報を登録する
                        action_list.Add(new ActionPlanM(staff_action));
                    }
                    else
                    {
                        // 空の行動情報を用意する（従業員IDと従業員名のみ）
                        var emply_action_plan = new ActionPlanM();
                        emply_action_plan.StaffID = staff.StaffID;
                        emply_action_plan.StaffName = staff.StaffName;
                        action_list.Add(emply_action_plan);
                    }
                }

                // 画面に表示
                this.ActionPlans = action_list;

            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
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
                    Init();
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

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    Init();
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
                    Init();
                }

            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
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
