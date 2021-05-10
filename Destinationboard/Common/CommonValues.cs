using Destinationboard.Common.Utilities;
using Grpc.Core;
using QRCodeScannerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

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

		#region 自PCのIPアドレス
		/// <summary>
		/// 自PCのIPアドレス
		/// </summary>
		public string OwnIP
		{
            get
            {
                // ホスト名を取得する
                string hostname = Dns.GetHostName();

                // ホスト名からIPアドレスを取得する
                IPAddress[] adrList = Dns.GetHostAddresses(hostname);

				if (adrList.Length > 0)

				{
					return adrList.ElementAt(0).ToString();
				}
				else
				{
					return string.Empty;
				}
			}
		}
		#endregion

		#region スキャナー[Scanner]プロパティ
		/// <summary>
		/// スキャナー[Scanner]プロパティ用変数
		/// </summary>
		ScannerManager _Scanner;
		/// <summary>
		/// スキャナー[Scanner]プロパティ
		/// </summary>
		public ScannerManager Scanner
		{
			get
			{
				return _Scanner;
			}
			set
			{
				if (_Scanner == null || !_Scanner.Equals(value))
				{
					_Scanner = value;
					NotifyPropertyChanged("Scanner");
				}
			}
		}
		#endregion

		#region スキャナの初期化処理
		/// <summary>
		/// スキャナの初期化処理
		/// </summary>
		public void ScannerInitialize()
        {
			// スキャナを使用する場合
			if(EnableHandyScanner)
            {
				// スキャナのオブジェクト作成
				this._Scanner = new ScannerManager(CommonValues.GetInstance().HandyScannerComPort);
			}
		}
        #endregion

        #region ハンディスキャナを使用するかどうか(true:使用する false:使用しない)[EnableHandyScanner]プロパティ
        /// <summary>
        /// ハンディスキャナを使用するかどうか(true:使用する false:使用しない)[EnableHandyScanner]プロパティ用変数
        /// </summary>
        bool _EnableHandyScanner = false;
		/// <summary>
		/// ハンディスキャナを使用するかどうか(true:使用する false:使用しない)[EnableHandyScanner]プロパティ
		/// </summary>
		public bool EnableHandyScanner
		{
			get
			{
				return _EnableHandyScanner;
			}
			set
			{
				if (!_EnableHandyScanner.Equals(value))
				{
					_EnableHandyScanner = value;
				}
			}
		}
		#endregion

		#region ハンディスキャナ用COMポート番号[HandyScannerComPort]プロパティ
		/// <summary>
		/// ハンディスキャナ用COMポート番号[HandyScannerComPort]プロパティ用変数
		/// </summary>
		int _HandyScannerComPort = 3;
		/// <summary>
		/// ハンディスキャナ用COMポート番号[HandyScannerComPort]プロパティ
		/// </summary>
		public int HandyScannerComPort
		{
			get
			{
				return _HandyScannerComPort;
			}
			set
			{
				if (!_HandyScannerComPort.Equals(value))
				{
					_HandyScannerComPort = value;
				}
			}
		}
		#endregion

		#region PaSoRi(Felica用)の有効かどうか(true:使用する false:使用しない)を示すフラグ[EnablePaSoRi]プロパティ
		/// <summary>
		/// PaSoRi(Felica用)の有効かどうか(true:使用する false:使用しない)を示すフラグ[EnablePaSoRi]プロパティ用変数
		/// </summary>
		bool _EnablePaSoRi = false;
		/// <summary>
		/// PaSoRi(Felica用)の有効かどうか(true:使用する false:使用しない)を示すフラグ[EnablePaSoRi]プロパティ
		/// </summary>
		public bool EnablePaSoRi
		{
			get
			{
				return _EnablePaSoRi;
			}
			set
			{
				if (!_EnablePaSoRi.Equals(value))
				{
					_EnablePaSoRi = value;
				}
			}
		}
		#endregion

		/// <summary>
		/// Configデータのセット
		/// </summary>
		public void ReadConfig()
		{
			ConfigManager conf = new ConfigManager();
			conf.LoadConfig();
			ServerName = conf.ServerName;
			Port = conf.Port;

			// ハンディスキャナ設定
			HandyScannerComPort = conf.HandyScannerComPort;
			EnableHandyScanner = conf.EnableHandyScanner;

			// PaSoRiの設定
			EnablePaSoRi = conf.EnablePaSoRi;
		}

		public DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient GetClient()
		{
			// チャネルの取得
			var channel = new Grpc.Core.Channel(CommonValues.GetInstance().ServerName, CommonValues.GetInstance().Port,
				ChannelCredentials.Insecure);

			// 通信用クライアントの取得
			var client = new DestinationbardCommunicationAPI.DestinationbardCommunicationAPIClient(channel);

			// クライアントの返却
			return client;
		}
	}
}
