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
                // 行動情報の取得処理
                this.ActionLists = ActionInfoCollectionM.GetActionInfo();

                // 行動予定の取得処理
                this.ActionPlan = ActionPlanM.GetActionPlan(this.ActionPlan.StaffID);

                // イベントの初期化処理
                InitEvent();
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        /// <summary>
        /// イベントの初期化処理
        /// </summary>
        public void InitEvent()
        {
            foreach (var item in this.ActionLists.Items)
            {
                item.SelectionEvent -= Item_SelectionEvent;
                item.SelectionEvent += Item_SelectionEvent;

                foreach (var dest in item.DestinationItems.Items)
                {
                    dest.SelectionEvent -= Dest_SelectionEvent;
                    dest.SelectionEvent += Dest_SelectionEvent;
                }
            }
        }

        #region 行先の選択処理
        /// <summary>
        /// 行先の選択処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dest_SelectionEvent(object sender, EventArgs e)
        {
            // 行動リスト
            for (int index = 0; index < this.ActionLists.Items.Count; index++)
            {
                var tmp = this.ActionLists.Items.ElementAt(index);

                // 行先リスト
                for (int index2 = 0; index2 < tmp.DestinationItems.Items.Count; index2++)
                {
                    // 行先リストの要素取り出し
                    var dest_tmp = tmp.DestinationItems.ElementAt(index2);

                    // 選択された要素と比較
                    if (dest_tmp.Equals(sender))
                    {
                        // 一致したのでこれが選択された
                        dest_tmp.IsSelected = true;
                    }
                    else
                    {
                        // 不一致なので選択を解除
                        dest_tmp.IsSelected = false;
                    }
                }
            }

            // 画面の更新
            NotifyPropertyChanged("ActionLists");
        }
        #endregion

        #region 行動の選択処理
        /// <summary>
        /// 行動の選択処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_SelectionEvent(object sender, EventArgs e)
        {
            // 行動リスト
            for (int index = 0; index < this.ActionLists.Items.Count; index++)
            {
                // 要素の取り出し
                var tmp = this.ActionLists.Items.ElementAt(index);

                // 選択された要素と比較
                if (tmp.Equals(sender))
                {
                    // 一致したのでこれが選択された
                    tmp.IsSelected = true;
                    // 選択アイテムのセット
                    this.ActionLists.SelectedItem = tmp;
                }
                else
                {
                    // 不一致なので選択を解除
                    tmp.IsSelected = false;
                }
            }

            NotifyPropertyChanged("ActionLists");
        }
        #endregion



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
