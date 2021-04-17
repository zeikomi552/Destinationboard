using Destinationboard.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class DestinationInfoM : DestinationMasterBase
    {
        public event EventHandler SelectionEvent = null;

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

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DestinationInfoM()
        {
            Guid guidValue = Guid.NewGuid();
            this.DestinationID = guidValue.ToString();

            this.CreateDate = DateTime.Now;
            this.CreateUser = Environment.UserName;
            this.UpdateDate = DateTime.Now;
            this.UpdateUser = Environment.UserName;
        }

        /// <summary>
        /// リプライをコピーするコンストラクタ
        /// </summary>
        public DestinationInfoM(DestinationMasterReply reply)
        {
            this.ActionID = reply.ActionID;
            this.DestinationID = reply.DestinationID;
            this.DestinationName = reply.DestinationName;
            this.SortOrder = reply.SortOrder;
            this.CreateDate = DateTime.ParseExact(reply.CreateDate, "yyyy/MM/dd", null);
            this.CreateUser = reply.CreateUser;
            this.UpdateDate = DateTime.ParseExact(reply.UpdateDate, "yyyy/MM/dd", null);
            this.UpdateUser = reply.UpdateUser;
        }

        /// <summary>
        /// リプライをコピーする処理
        /// </summary>
        /// <param name="reply">リプライ</param>
        /// <returns>行先情報</returns>
        public DestinationInfoM Copy(DestinationMasterReply reply)
        {
            return new DestinationInfoM(reply);
        }
    }
}
