using Destinationboard.Common.Utilities;
using Destinationboard.Models.db;
using Destinationboard.ViewModels;
using Destinationboard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class ActionPlanM : ActionPlanTableBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ActionPlanM()
		{

		}
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="reply">リプライ</param>
        public ActionPlanM(ActionPlanTableReply reply)
        {
            this.Copy(reply);
        }
        #endregion

        #region Copy処理(ActionPlanTableReplyを変換する)
        /// <summary>
        /// Copy処理(ActionPlanTableReplyを変換する)
        /// </summary>
        /// <param name="reply">ActionPlanTableReply</param>
        public void Copy(ActionPlanTableReply reply)
        {
            this.ActionID = reply.ActionID;                 // 行動ID
            this.ActionName = reply.ActionName;             // 行動名
            this.DestinationID = reply.DestinationID;       // 行先ID
            this.DestinationName = reply.DestinationName;   // 行先名

            this.FromTime = null;
            // 開始時刻のnullチェック
            if (!reply.FromTime.Equals(string.Empty))
            {
                // 開始時刻をセット
                this.FromTime = DateTime.ParseExact(reply.FromTime, "yyyy/MM/dd HH:mm:ss", null);
            }
            this.ToTime = null;
            // 終了時刻のnullチェック
            if (!reply.ToTime.Equals(string.Empty))
            {
                // 終了時刻をセット
                this.ToTime = DateTime.ParseExact(reply.ToTime, "yyyy/MM/dd HH:mm:ss", null);
            }

            // メモのセット
            this.Memo = reply.Memo;
        }
        #endregion

        /// <summary>
        /// 出退勤を登録する
        /// </summary>
        public void MoveRegistBeginFinishV()
        {
            var wnd = new RegistBeginFinishV();
            var vm = wnd.DataContext as RegistBeginFinishVM;
            vm.ActionPlan.StaffID = this.StaffID;
            vm.ActionPlan.StaffName = this.StaffName;

            // 画面遷移
            if (wnd.ShowDialog() == true)
            {

            }
        }

    }
}
