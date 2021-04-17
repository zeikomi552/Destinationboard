using Destinationboard.Common.Utilities;
using Destinationboard.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class ActionInfoM : ActionMasterBase
	{
        public event EventHandler SelectionEvent = null;

        #region 選択されているかどうかを示すフラグ[IsSelected]プロパティ
        /// <summary>
        /// 選択されているかどうかを示すフラグ[IsSelected]プロパティ用変数
        /// </summary>
        bool _IsSelected = false;
        /// <summary>
        /// 選択されているかどうかを示すフラグ[IsSelected]プロパティ
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (!_IsSelected.Equals(value))
                {
                    _IsSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ActionInfoM()
		{
			Guid guidValue = Guid.NewGuid();
			this.ActionID = guidValue.ToString();
			this.CreateDate = DateTime.Today;
			this.CreateUser = Environment.UserName;
			this.UpdateDate = DateTime.Today;
			this.UpdateUser = Environment.UserName;
		}

        /// <summary>
        /// コンストラクタ(StaffMasterReplyを変換する)
        /// </summary>
        /// <param name="reply">StaffMasterReply</param>
        public ActionInfoM(ActionMasterReply reply, List<DestinationMasterReply> list)
        {
            this.Copy(reply, list);
        }
        #endregion

        #region 行先のチェックを外す
        /// <summary>
        /// 行先のチェックを外す
        /// </summary>
        public void ClearDestinationSelection()
        {
            for (int index = 0; index < this.DestinationItems.Items.Count; index++)
            {
                var item = this.DestinationItems.ElementAt(index);
                item.IsSelected = false;
            }
        }
        #endregion

        #region 選択処理
        /// <summary>
        /// 選択処理
        /// </summary>
        public void Selection()
        {
            // イベント登録されているかどうかの確認
            if (SelectionEvent != null)
            {
                // 選択されたイベントを発生させる
                SelectionEvent(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Copy処理(ActionMasterReplyを変換する)
        /// <summary>
        /// Copy処理(ActionMasterReplyを変換する)
        /// </summary>
        /// <param name="reply">ActionMasterReply</param>
        public void Copy(ActionMasterReply reply, List<DestinationMasterReply> list)
        {
            this.ActionID = reply.ActionID;
            this.ActionName = reply.ActionName;
            this.SortOrder = reply.SortOrder;
            this.CreateDate = DateTime.ParseExact(reply.CreateDate, "yyyy/MM/dd", null);
            this.CreateUser = reply.CreateUser;
            this.UpdateDate = DateTime.ParseExact(reply.UpdateDate, "yyyy/MM/dd", null);
            this.UpdateUser = reply.UpdateUser;

            DestinationInfoCollectionM dist = new DestinationInfoCollectionM();

            // 行先情報のコピー
            foreach (var dist_tmp in list)
            {
                dist.Items.Add(new DestinationInfoM(dist_tmp));

            }
            this.DestinationItems = dist;
        }
        #endregion

        #region 行先情報のソート
        /// <summary>
        /// 行先情報のソート
        /// </summary>
        /// <returns>ソート後の行先情報</returns>
        public DestinationInfoCollectionM SortDestination()
        {
            return new DestinationInfoCollectionM((from x in this.DestinationItems.Items
                                                   orderby x.SortOrder
                                                   select x).ToList<DestinationInfoM>());
        }
        #endregion

        #region 行先リスト[DestinationItems]プロパティ
        /// <summary>
        /// 行先リスト[DestinationItems]プロパティ用変数
        /// </summary>
        DestinationInfoCollectionM _DestinationItems = new DestinationInfoCollectionM();
        /// <summary>
        /// 行先リスト[DestinationItems]プロパティ
        /// </summary>
        public DestinationInfoCollectionM DestinationItems
        {
            get
            {
                return _DestinationItems;
            }
            set
            {
                if (_DestinationItems == null || !_DestinationItems.Equals(value))
                {
                    _DestinationItems = value;
                    NotifyPropertyChanged("DestinationItems");
                }
            }
        }
        #endregion

        #region 行先情報用の連結キーの親の行動情報のアクションIDと一致させる
        /// <summary>
        /// 行先情報用の連結キーの親の行動情報のアクションIDと一致させる
        /// </summary>
        public void RefleshDestinationActionID()
        {
            for (int iCnt = 0; iCnt < this.DestinationItems.Items.Count; iCnt++)
            {
                var dist = this.DestinationItems.ElementAt(iCnt);
                if (dist != null)
                {
                    dist.ActionID = this.ActionID;  // ActionIDのセット
                }
            }
        }
        #endregion
    }
}
