using Destinationboard.Common;
using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Destinationboard.ViewModels
{
    public class RegistBeginFinishVM : ViewModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RegistBeginFinishVM()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
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

        #region 個人の行動予定[ActionPlan]プロパティ
        /// <summary>
        /// 個人の行動予定[ActionPlan]プロパティ用変数
        /// </summary>
        ActionPlanM _ActionPlan = new ActionPlanM();
        /// <summary>
        /// 個人の行動予定[ActionPlan]プロパティ
        /// </summary>
        public ActionPlanM ActionPlan
        {
            get
            {
                return _ActionPlan;
            }
            set
            {
                if (_ActionPlan == null || !_ActionPlan.Equals(value))
                {
                    _ActionPlan = value;
                    NotifyPropertyChanged("ActionPlan");
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
                this.ActionPlan = ActionPlanM.GetActionPlan(this.ActionPlan.StaffID);
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
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
                if (this.ActionPlan.Status <= 0)
                {
                    this._ActionPlan.Status = 0;
                }

                this.DialogResult = true;
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion
    }
}
