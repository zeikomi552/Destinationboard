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

        #region 行先情報の全データ[DestinationItemsAll]プロパティ
        /// <summary>
        /// 行先情報の全データ[DestinationItemsAll]プロパティ用変数
        /// </summary>
        DestinationInfoCollectionM _DestinationItemsAll = new DestinationInfoCollectionM();
        /// <summary>
        /// 行先情報の全データ[DestinationItemsAll]プロパティ
        /// </summary>
        public DestinationInfoCollectionM DestinationItemsAll
        {
            get
            {
                return _DestinationItemsAll;
            }
            set
            {
                if (_DestinationItemsAll == null || !_DestinationItemsAll.Equals(value))
                {
                    _DestinationItemsAll = value;
                    NotifyPropertyChanged("DestinationItemsAll");
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
                // チャネルの作成
                var channel = new Grpc.Core.Channel(CommonValues.GetInstance().ServerName,
                    CommonValues.GetInstance().Port, ChannelCredentials.Insecure);

                // クライアントの作成
                var client = new DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient(channel);

                // リクエストの作成
                GetActionsRequest actions_request = new GetActionsRequest();

                // データの取得
                GetActionsReply actions_reply = client.GetActions(actions_request);
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        public void Regist()
        {
            try
            {
                try
                {
                    // チャネルの取得
                    var channel = new Grpc.Core.Channel(CommonValues.GetInstance().ServerName, CommonValues.GetInstance().Port,
                        ChannelCredentials.Insecure);
                    var client = new DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient(channel);

                    // リクエストの作成
                    RegistActionsRequest request = new RegistActionsRequest();

                    // ユーザー名の作成
                    request.IP = Environment.MachineName;

                    //RefreshSortOrder(); // ソート順の更新

                    //// スタッフ情報の作成
                    //foreach (var tmp in this.StaffItems.Items)
                    //{
                    //    StaffMasterRequest staff_item = new StaffMasterRequest();
                    //    staff_item.StaffID = tmp.StaffID;
                    //    staff_item.SortOrder = tmp.SortOrder;
                    //    staff_item.StaffName = tmp.StaffName;
                    //    staff_item.Display = tmp.Display;
                    //    staff_item.CreateUser = Environment.UserName;
                    //    staff_item.CreateDate = DateTime.Now.ToString("yyyy/MM/dd");
                    //    request.StaffInfoList.Add(staff_item);
                    //}

                    //// 送信
                    //var reply = client.RegistStaff(request);

                    //ShowMessage.ShowNoticeOK("登録しました", "通知");

                    // 閉じる処理
                    this.DialogResult = true;

                }
                catch (Exception e)
                {
                    _logger.Error("Fatal Error", e);
                    ShowMessage.ShowErrorOK(e.Message, "Error");
                }
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }

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
