using Destinationboard.Common;
using Destinationboard.Common.Utilities;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class ActionInfoCollectionM : ModelList<ActionInfoM>
	{
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ActionInfoCollectionM()
        {

        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="item">List</param>
        public ActionInfoCollectionM(List<ActionInfoM> item)
        {
            this.Items = new System.Collections.ObjectModel.ObservableCollection<ActionInfoM>(item);
        }
        #endregion

        #region 行動一覧の取得処理
        /// <summary>
        /// 行動一覧の取得処理
        /// </summary>
        /// <returns>行動一覧</returns>
        public static ActionInfoCollectionM GetActionInfo()
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

            ActionInfoCollectionM action_list = new ActionInfoCollectionM();


            // 行動マスター情報の取り出し
            foreach (var action in actions_reply.ActionList)
            {
                // 該当するIDの行先情報を登録する
                var dist = (from x in actions_reply.DestinationList
                            where x.ActionID.Equals(action.ActionID)
                            select x).ToList<DestinationMasterReply>();

                // 行動情報のコピー
                action_list.Add(new ActionInfoM(action, dist));
            }

            // 行動リストの返却
            return action_list;
        }
        #endregion

        #region 行動情報および行先情報をソートする
        /// <summary>
        /// 行動情報および行先情報をソートする
        /// </summary>
        /// <returns></returns>
        public ActionInfoCollectionM Sort()
        {
            // 行動の情報をソート
            ActionInfoCollectionM items = new ActionInfoCollectionM((from x in this.Items
                                                                     orderby x.SortOrder
                                                                     select x).ToList<ActionInfoM>());

            // 行動の数だけまわす
            for (int index = 0; index < this.Items.Count; index++)
            {
                // 行動を取り出す
                var elem = this.Items.ElementAt(index);

                // 行先情報のソート
                elem.DestinationItems = elem.SortDestination();
            }


            return items;
        }
        #endregion

        #region INotifyPropertyChanged 
        public new event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#endregion

	}
}
