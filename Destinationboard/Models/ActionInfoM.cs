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
