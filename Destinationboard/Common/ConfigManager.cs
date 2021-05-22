using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Common
{
	public class ConfigManager
	{
		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ConfigManager()
		{
		}
        #endregion

        #region コンフィグ
        /// <summary>
        /// コンフィグフォルダ
        /// </summary>
        string _ConfigFolder = "Config";
		/// <summary>
		/// コンフィグファイル名
		/// </summary>
		string _ConfigFileName = "Destination.conf";

		#region コンフィグファイルパス
		/// <summary>
		/// コンフィグファイルパス
		/// </summary>
		string ConfigPath
		{
			get
			{
				// Configフォルダのパス取得
				string conf_dir = Path.Combine(Utilities.Utilities.GetApplicationFolder(), _ConfigFolder);

				// 存在確認
				if (!Directory.Exists(conf_dir))
				{
					// 存在しない場合は作成
					Utilities.Utilities.CreateDirectory(conf_dir);
				}

				// パスの結合
				return Path.Combine(Utilities.Utilities.GetApplicationFolder(), _ConfigFolder, _ConfigFileName);
			}
		}
		#endregion
		#endregion

		#region ロガー
		/// <summary>
		/// ロガー
		/// </summary>
		protected static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		#endregion

		#region サーバー名（orIPアドレス)[ServerName]プロパティ
		/// <summary>
		/// サーバー名（orIPアドレス)[ServerName]プロパティ用変数
		/// </summary>
		string _ServerName = "localhost";
		/// <summary>
		/// サーバー名（orIPアドレス)[ServerName]プロパティ
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
				}
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

		#region コンフィグファイルの保存処理
		/// <summary>
		/// コンフィグファイルの保存処理
		/// </summary>
		public void SaveConfig()
		{
			try
			{
				// configファイルの出力
				XMLUtil.Seialize<ConfigManager>(ConfigPath, this);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				_logger.Error(e.Message);
			}
		}
		#endregion

		#region コンフィグファイルの存在確認と存在しない場合は初期値で作成
		/// <summary>
		/// コンフィグファイルの存在確認と存在しない場合は初期値で作成
		/// </summary>
		private void ConfigCheckCreate()
		{

			// Configファイルの存在確認
			if (!File.Exists(ConfigPath))
			{
				// 初期値でコンフィグ情報を作成
				ConfigManager ini_conf = new ConfigManager();

				// configファイルの作成
				XMLUtil.Seialize<ConfigManager>(ConfigPath, ini_conf);
			}
		}
		#endregion

		#region 値のコピー
		/// <summary>
		/// 値のコピー
		/// </summary>
		/// <param name="conf">Configデータ</param>
		public void Copy(ConfigManager conf)
		{
			this.ServerName = conf.ServerName;  // サーバー名
			this.Port = conf.Port;          // ポート番号
			this.EnableHandyScanner = conf.EnableHandyScanner;		// スキャナ使用可/不可
			this.HandyScannerComPort = conf.HandyScannerComPort;    // スキャナポート番号
			this.EnablePaSoRi = conf.EnablePaSoRi;	// PaSoRiの使用可/不可
			
		}
		#endregion

		#region コンフィグファイルのロード処理
		/// <summary>
		/// コンフィグファイルのロード処理
		/// </summary>
		public void LoadConfig()
		{
			try
			{
				// コンフィグフォルダの存在確認と読み込み
				ConfigCheckCreate();

				// configファイルの読み込み
				var conf = XMLUtil.Deserialize<ConfigManager>(ConfigPath);

				// 値のコピー
				this.Copy(conf);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				_logger.Error(e.Message);
			}
		}
		#endregion
	}
}
