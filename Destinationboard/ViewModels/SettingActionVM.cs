using Destinationboard.Common;
using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.ViewModels
{
    public class SettingActionVM : ViewModelBase
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
                // 行動一覧の取得
                var tmp = ActionInfoCollectionM.GetActionInfo();

                // ソート順でソート
                this.ActionLists = tmp.Sort();
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region ソート順の更新
        /// <summary>
        /// ソート順の更新
        /// </summary>
        private void RefreshSortOrder()
        {
            // 行動リスト分ソート順の更新を行う
            for(int iCnt = 0; iCnt < this.ActionLists.Items.Count; iCnt ++ )
            {
                // 行動の取り出し
                var temp_action = this.ActionLists.Items.ElementAt(iCnt);

                // ソート順の更新
                temp_action.SortOrder = iCnt;

                // 行動に合致する行先を取得
                var action_destinatios = from x in temp_action.DestinationItems.Items
                                      where x.ActionID.Equals(temp_action.ActionID)
                                      select x;

                // 行先分ソート順の更新を行う
                for (int iCnt2 = 0; iCnt2 < action_destinatios.Count(); iCnt2++)
                {
                    // 行先の取り出し
                    var destination_tmp = action_destinatios.ElementAt(iCnt2);

                    // ソート順の更新
                    destination_tmp.SortOrder = iCnt2;
                }
            }
        }
        #endregion

        #region 行先情報用の連結キーを更新
        /// <summary>
        /// 行先情報用の連結キーを更新
        /// </summary>
        public void RefleshDestinationActionID()
        {
            try
            {
                for (int iCnt = 0; iCnt < this.ActionLists.Items.Count; iCnt++)
                {
                    var elem = this.ActionLists.ElementAt(iCnt);

                    // 行動ID
                    elem.RefleshDestinationActionID();
                }
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
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
                DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient client =
                    CommonValues.GetInstance().GetClient();

                // ソート順の更新
                RefreshSortOrder();

                // 行先情報のActionIDを更新
                RefleshDestinationActionID();

                string own_ip = CommonValues.GetInstance().OwnIP;   // 自PCのIPの取得
                RegistActionsRequest action_request = new RegistActionsRequest();
                action_request.IP = own_ip;

                DestinationInfoCollectionM dist_all = new DestinationInfoCollectionM();


                // 行動情報分要求に詰め込む
                foreach (var tmp in this.ActionLists.Items)
                {
                    ActionMasterRequest action_master = new ActionMasterRequest();
                    action_master.ActionID = tmp.ActionID;
                    action_master.ActionName = tmp.ActionName;
                    action_master.CreateDate = tmp.CreateDate.ToString("yyyy/MM/dd");
                    action_master.CreateUser = tmp.CreateUser;
                    action_master.UpdateDate = tmp.UpdateDate.ToString("yyyy/MM/dd");
                    action_master.UpdateUser = tmp.UpdateUser;
                    action_master.SortOrder = tmp.SortOrder;

                    action_request.ActionMasterList.Add(action_master);

                    foreach (var dist_tmp in tmp.DestinationItems.Items)
                    {
                        dist_all.Add(dist_tmp);
                    }
                }

                // 行先情報分要求に詰め込む
                foreach (var tmp in dist_all.Items)
                {
                    DestinationMasterRequest destination_master = new DestinationMasterRequest();
                    destination_master.ActionID = tmp.ActionID;
                    destination_master.CreateDate = tmp.CreateDate.ToString("yyyy/MM/dd");
                    destination_master.CreateUser = tmp.CreateUser;
                    destination_master.UpdateDate = tmp.UpdateDate.ToString("yyyy/MM/dd");
                    destination_master.UpdateUser = tmp.UpdateUser;
                    destination_master.SortOrder = tmp.SortOrder;
                    destination_master.DestinationID = tmp.DestinationID;
                    destination_master.DestinationName = tmp.DestinationName;

                    action_request.DestinationMasterList.Add(destination_master);
                }

                // 送信
                var reply = client.RegistActions(action_request);

                ShowMessage.ShowNoticeOK("登録しました", "通知");

                // 閉じる処理
                this.DialogResult = true;

            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }


        }
        #endregion

        #region 上へ移動
        /// <summary>
        /// 上へ移動
        /// </summary>
        public void MoveUp()
        {
            try
            {
                this.ActionLists.MoveUp();
                RefreshSortOrder(); // ソート順の更新
                NotifyPropertyChanged("ActionLists");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 下へ移動処理
        /// <summary>
        /// 下へ移動処理
        /// </summary>
        public void MoveDown()
        {
            try
            {
                this.ActionLists.MoveDown();
                RefreshSortOrder(); // ソート順の更新
                NotifyPropertyChanged("ActionLists");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 削除処理
        /// <summary>
        /// 削除処理
        /// </summary>
        public void Delete()
        {
            try
            {
                this.ActionLists.DeleteSelectedItem();
                RefreshSortOrder(); // ソート順の更新
                NotifyPropertyChanged("ActionLists");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 上へ移動(行先)
        /// <summary>
        /// 上へ移動(行先)
        /// </summary>
        public void MoveUpDestination()
        {
            try
            {
                // nullチェック
                if (this.ActionLists.SelectedItem != null)
                {
                    // 上へ移動
                    this.ActionLists.SelectedItem.DestinationItems.MoveUp();
                    RefreshSortOrder(); // ソート順の更新
                    NotifyPropertyChanged("ActionLists");
                }
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 下へ移動処理(行先)
        /// <summary>
        /// 下へ移動処理(行先)
        /// </summary>
        public void MoveDownDestination()
        {
            try
            {
                // nullチェック
                if (this.ActionLists.SelectedItem != null)
                {
                    // 下へ移動
                    this.ActionLists.SelectedItem.DestinationItems.MoveDown();
                    RefreshSortOrder(); // ソート順の更新
                    NotifyPropertyChanged("ActionLists");
                }
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 削除処理(行先)
        /// <summary>
        /// 削除処理(行先)
        /// </summary>
        public void DeleteDestination()
        {
            try
            {
                // nullチェック
                if (this.ActionLists.SelectedItem != null)
                {
                    this.ActionLists.SelectedItem.DestinationItems.DeleteSelectedItem();
                    RefreshSortOrder(); // ソート順の更新
                    NotifyPropertyChanged("ActionLists");
                }
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
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
                if (ShowMessage.ShowQuestionYesNo("変更内容は登録されません。よろしいですか？", "確認") == System.Windows.MessageBoxResult.Yes)
                {
                    this.DialogResult = false;
                }
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
