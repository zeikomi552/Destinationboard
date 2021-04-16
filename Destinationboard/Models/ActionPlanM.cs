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

        #region 出退勤を登録する
        /// <summary>
        /// 出退勤を登録する
        /// </summary>
        public void MoveRegistBeginFinishV()
        {
            var wnd = new RegistBeginFinishV();
            var vm = wnd.DataContext as RegistBeginFinishVM;
            vm.ActionPlan.Copy(this);

            // 画面遷移
            if (wnd.ShowDialog() == true)
            {
                // 行動予定の登録
                RegistActionPlanTable(vm.ActionPlan);
            }
        }
        #endregion

        #region 行動登録画面への画面遷移
        /// <summary>
        /// 行動登録画面への画面遷移
        /// </summary>
        public void MoveRegistActionV()
        {
            try
            {
                RegistActionV wnd = new RegistActionV();
                RegistActionVM vm = wnd.DataContext as RegistActionVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    //Init();
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        /// <summary>
        /// 開始時刻の登録画面へ画面遷移
        /// </summary>
        public void MoveFromRegistTime()
        {
            MoveRegistTimeV();
        }

        /// <summary>
        /// 終了時刻の登録画面への画面遷移
        /// </summary>
        public void MoveToRegistTime()
        {
            MoveRegistTimeV();
        }

        #region 時間登録画面への画面遷移
        /// <summary>
        /// 時間登録画面への画面遷移
        /// </summary>
        public void MoveRegistTimeV()
        {
            try
            {
                RegistTimeV wnd = new RegistTimeV();
                RegistTimeVM vm = wnd.DataContext as RegistTimeVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                    //Init();
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

    }
}
