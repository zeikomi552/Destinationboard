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
        DispatcherTimer _timer = new DispatcherTimer();
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RegistBeginFinishVM()
        {
            _timer.Tick += timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
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

        #region ステータスのバックアップ
        /// <summary>
        /// ステータスのバックアップ
        /// </summary>
        private int StatusBak = 0;
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Init()
        {
            try
            {
                // 行動計画の取得
                this.ActionPlan = ActionPlanM.GetActionPlan(this.ActionPlan.StaffID);

                // ステータスを保持
                this.StatusBak = this.ActionPlan.Status;
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
                // ステータスの確認（負の値の場合）
                if (this.ActionPlan.Status < 0)
                {
                    // ステータスを戻す
                    this._ActionPlan.Status = this.StatusBak;
                }
                else
                {
                    _timer.Stop();  // タイマーのストップ
                    // 画面を閉じる
                    this.DialogResult = true;
                }
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
