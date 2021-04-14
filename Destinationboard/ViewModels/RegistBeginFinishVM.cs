using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.ViewModels
{
    public class RegistBeginFinishVM : ViewModelBase
    {
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
