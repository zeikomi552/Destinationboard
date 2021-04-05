using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Common
{
    public class CommonValues : ModelBase
	{
		#region インスタンス
		/// <summary>
		/// インスタンス
		/// </summary>
		static CommonValues _Instance = new CommonValues();
		#endregion

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		private CommonValues()
        {

        }
		#endregion

		#region インスタンスの取得処理
		/// <summary>
		/// インスタンスの取得処理
		/// </summary>
		/// <returns></returns>
		public static CommonValues GetInstance()
        {
            return _Instance;

        }
		#endregion

		#region サーバー名[ServerName]プロパティ
		/// <summary>
		/// サーバー名[ServerName]プロパティ用変数
		/// </summary>
		string _ServerName = "127.0.0.1";
		/// <summary>
		/// サーバー名[ServerName]プロパティ
		/// </summary>
		public string ServerName
		{
			get
			{
				return _ServerName;
			}
			set
			{
				if (!_ServerName.Equals(value))
				{
					_ServerName = value;
					NotifyPropertyChanged("ServerName");
				}
			}
		}
		#endregion
		#region ポート番号[Port]プロパティ
		/// <summary>
		/// ポート番号[Port]プロパティ用変数
		/// </summary>
		int _Port = 552;
		/// <summary>
		/// ポート番号[Port]プロパティ
		/// </summary>
		public int Port
		{
			get
			{
				return _Port;
			}
			set
			{
				if (!_Port.Equals(value))
				{
					_Port = value;
					NotifyPropertyChanged("Port");
				}
			}
		}
		#endregion


	}
}
