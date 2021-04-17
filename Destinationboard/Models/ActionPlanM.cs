using Destinationboard.Common;
using Destinationboard.Common.Utilities;
using Destinationboard.Models.db;
using Destinationboard.ViewModels;
using Destinationboard.Views;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class ActionPlanM : ActionPlanTableBase
    {
        // 従業員名のクリックイベント
        public event EventHandler ClickRegistBeginFinish;

        /// <summary>
        /// 行動ボタンのクリックイベント
        /// </summary>
        public event EventHandler ClickRegistAction;

        /// <summary>
        /// 開始時刻のクリックイベント
        /// </summary>
        public event EventHandler ClickStartTime;

        /// <summary>
        /// 終了時刻のクリックイベント
        /// </summary>
        public event EventHandler ClickEndTime;

        /// <summary>
        /// メモのクリックイベント
        /// </summary>
        public event EventHandler ClickMemo;

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

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="reply">リプライ</param>
        public ActionPlanM(GetActionPlanReply reply)
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
            var tmp = reply;
            this.ActionID = tmp.ActionID;                 // 行動ID
            this.ActionName = tmp.ActionName;             // 行動名
            this.DestinationID = tmp.DestinationID;       // 行先ID
            this.DestinationName = tmp.DestinationName;   // 行先名
            this.StaffID = tmp.StaffID;                   // 従業員情報
            this.StaffName = tmp.StaffName;               // 従業員名
            this.Status = tmp.Status;                     // ステータス

            this.FromTime = null;
            // 開始時刻のnullチェック
            if (!tmp.FromTime.Equals(string.Empty))
            {
                // 開始時刻をセット
                this.FromTime = DateTime.ParseExact(tmp.FromTime, "yyyy/MM/dd HH:mm:ss", null);
            }
            this.ToTime = null;
            // 終了時刻のnullチェック
            if (!tmp.ToTime.Equals(string.Empty))
            {
                // 終了時刻をセット
                this.ToTime = DateTime.ParseExact(tmp.ToTime, "yyyy/MM/dd HH:mm:ss", null);
            }

            // メモのセット
            this.Memo = tmp.Memo;
        }
        #endregion

        #region Copy処理(ActionPlanTableReplyを変換する)
        /// <summary>
        /// Copy処理(ActionPlanTableReplyを変換する)
        /// </summary>
        /// <param name="reply">ActionPlanTableReply</param>
        public void Copy(GetActionPlanReply reply)
        {
            var tmp = reply.ActionPlan;
            this.ActionID = tmp.ActionID;                 // 行動ID
            this.ActionName = tmp.ActionName;             // 行動名
            this.DestinationID = tmp.DestinationID;       // 行先ID
            this.DestinationName = tmp.DestinationName;   // 行先名
            this.StaffID = tmp.StaffID;                   // 従業員情報
            this.StaffName = tmp.StaffName;               // 従業員名
            this.Status = tmp.Status;                     // ステータス

            this.FromTime = null;
            // 開始時刻のnullチェック
            if (!tmp.FromTime.Equals(string.Empty))
            {
                // 開始時刻をセット
                this.FromTime = DateTime.ParseExact(tmp.FromTime, "yyyy/MM/dd HH:mm:ss", null);
            }
            this.ToTime = null;
            // 終了時刻のnullチェック
            if (!tmp.ToTime.Equals(string.Empty))
            {
                // 終了時刻をセット
                this.ToTime = DateTime.ParseExact(tmp.ToTime, "yyyy/MM/dd HH:mm:ss", null);
            }

            // メモのセット
            this.Memo = tmp.Memo;
        }
        #endregion

        #region スタッフの登録処理
        /// <summary>
        /// スタッフの登録処理
        /// </summary>
        public static void RegistActionPlanTable(ActionPlanM action_plan)
        {
            try
            {
                // チャネルの取得
                var channel = new Grpc.Core.Channel(CommonValues.GetInstance().ServerName, CommonValues.GetInstance().Port,
                    ChannelCredentials.Insecure);
                var client = new DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient(channel);

                // リクエストの作成
                RegistActionPlanRequest request = new RegistActionPlanRequest();

                request.IP = Environment.UserName;
                request.ActionPlan = new ActionPlanTableRequest();

                request.ActionPlan.StaffID = action_plan.StaffID;
                request.ActionPlan.StaffName = action_plan.StaffName;
                request.ActionPlan.Status = action_plan.Status;
                request.ActionPlan.ActionID = action_plan.ActionID;
                request.ActionPlan.ActionName = action_plan.ActionName;
                request.ActionPlan.DestinationID = action_plan.DestinationID;
                request.ActionPlan.DestinationName = action_plan.DestinationName;
                request.ActionPlan.FromTime = action_plan.FromTime.HasValue ? action_plan.FromTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;
                request.ActionPlan.ToTime = action_plan.ToTime.HasValue ? action_plan.ToTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;
                request.ActionPlan.Memo = action_plan.Memo;


                // 送信
                var reply = client.RegistActionPlan(request);
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 1人分の行動予定の取得処理
        /// <summary>
        /// 1人分の行動予定の取得処理
        /// </summary>
        /// <param name="staff_id">従業員ID</param>
        /// <returns>1人分の行動予定</returns>
        public static ActionPlanM GetActionPlan(string staff_id)
        {
            try
            {
                // チャネルの取得
                var channel = new Grpc.Core.Channel(CommonValues.GetInstance().ServerName, CommonValues.GetInstance().Port,
                    ChannelCredentials.Insecure);
                var client = new DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient(channel);

                // リクエストの作成
                GetActionPlanRequest request = new GetActionPlanRequest();

                request.IP = Environment.UserName;  // 登録ユーザー名
                request.StaffID = staff_id;         // 従業員ID

                // 送信
                var reply = client.GetActionPlan(request);

                return new ActionPlanM(reply);
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
                return null;
            }
        }
        #endregion

        #region 従業員名ボタンが押された処理
        /// <summary>
        /// 従業員名ボタンが押された処理
        /// </summary>
        public void RegistBeginFinish()
        {
            if (this.ClickRegistBeginFinish != null)
            {
                this.ClickRegistBeginFinish(this, EventArgs.Empty);
            }
        }
        #endregion

        #region 行先ボタンが押された処理
        /// <summary>
        /// 行先ボタンが押された処理
        /// </summary>
        public void RegistAction()
        {
            if (this.ClickRegistAction != null)
            {
                this.ClickRegistAction(this, EventArgs.Empty);
            }
        }
        #endregion

        #region 開始時刻が押された処理
        /// <summary>
        /// 開始時刻が押された処理
        /// </summary>
        public void RegistStartTime()
        {
            if (this.ClickStartTime != null)
            {
                this.ClickStartTime(this, EventArgs.Empty);
            }
        }
        #endregion

        #region 終了時刻が押された処理
        /// <summary>
        /// 終了時刻が押された処理
        /// </summary>
        public void RegistEndTime()
        {
            if (this.ClickEndTime != null)
            {
                this.ClickEndTime(this, EventArgs.Empty);
            }
        }
        #endregion

        #region メモが押された処理
        /// <summary>
        /// メモが押された処理
        /// </summary>
        public void RegistMemo()
        {
            if (this.ClickMemo != null)
            {
                this.ClickMemo(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
