using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.ViewModels
{
    public class RegistActionVM : ViewModelBase
    {
        #region 行動一覧[ActionLists]プロパティ
        /// <summary>
        /// 行動一覧[ActionLists]プロパティ用変数
        /// </summary>
        ActionInfoCollectionM _ActionLists = new ActionInfoCollectionM();
        /// <summary>
        /// 行動一覧[ActionLists]プロパティ
        /// </summary>
        public ActionInfoCollectionM ActionLists
        {
            get
            {
                return _ActionLists;
            }
            set
            {
                if (_ActionLists == null || !_ActionLists.Equals(value))
                {
                    _ActionLists = value;
                    NotifyPropertyChanged("ActionLists");
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
