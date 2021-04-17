using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Destinationboard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.ViewModels
{
    public class RegistTimeVM : ViewModelBase
    {

        public bool FromTimeF { get; set; } = false;

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
                // 行動予定の取得処理
                this.ActionPlan = ActionPlanM.GetActionPlan(this.ActionPlan.StaffID);
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion


        public void FromTime_MoveRegistTimeMinutesV()
        {
            try
            {
                this.ActionPlan.FromTime = MoveRegistTimeMinutesV(this.ActionPlan.FromTime);
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        public void ToTime_MoveRegistTimeMinutesV()
        {
            try
            {
                this.ActionPlan.ToTime = MoveRegistTimeMinutesV(this.ActionPlan.ToTime);
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }

        public void FromTime_MoveRegistTimeHourV()
        {
            try
            {
                this.ActionPlan.FromTime = MoveRegistTimeHourV(this.ActionPlan.FromTime);
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        public void ToTime_MoveRegistTimeHourV()
        {
            try
            {
                this.ActionPlan.ToTime = MoveRegistTimeHourV(this.ActionPlan.ToTime);
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }

        private DateTime? MoveRegistTimeHourV(DateTime? org_time)
        {
            var wnd = new RegistTimeHourV();
            var vm = wnd.DataContext as RegistTimeHourVM;

            if (wnd.ShowDialog() == true)
            {
                DateTime tmp_date = DateTime.Today;
                // 日付を持っている？
                if (org_time.HasValue)
                {
                    // 日付データに変更（時間データを削除）
                    tmp_date = org_time.Value;
                }

                // 押された時間が-の場合
                if (vm.ClickNumber <= 0)
                {
                    // 日付データを返却する
                    return tmp_date.Date;
                }
                else
                {
                    // 時間データを足し合わせて返却する
                    return tmp_date.Date.AddHours(vm.ClickNumber);
                }
            }
            else
            {
                return org_time;
            }
        }

        private DateTime? MoveRegistTimeMinutesV(DateTime? org_time)
        {
            var wnd = new RegistTimeMinutesV();
            var vm = wnd.DataContext as RegistTimeMinutesVM;

            if (wnd.ShowDialog() == true)
            {
                DateTime tmp_date = DateTime.Today;
                // 日付を持っている？
                if (org_time.HasValue)
                {
                    // 日付データに変更（時間データを削除）
                    tmp_date = org_time.Value.Date;
                    // 時間を足す
                    tmp_date = tmp_date.AddHours(org_time.Value.Hour);
                }

                // 押された時間が-の場合
                if (vm.ClickNumber <= 0)
                {
                    // 分は0の状態で返却する
                    return tmp_date;
                }
                else
                {
                    // 分データを足し合わせて返却する
                    return tmp_date.AddMinutes(vm.ClickNumber);
                }
            }
            else
            {
                return org_time;
            }
        }

        /// <summary>
        /// 時間情報をクリアする
        /// </summary>
        public void Clear()
        {
            try
            {
                this.ActionPlan.FromTime = null;    // null設定
                this.ActionPlan.ToTime = null;      // null設定
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }

        #region 登録処理
        /// <summary>
        /// 登録処理
        /// </summary>
        public void Regist()
        {
            try
            {
                this.DialogResult = true;
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
                this.DialogResult = false;
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
