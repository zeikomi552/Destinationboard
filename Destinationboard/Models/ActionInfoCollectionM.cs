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
