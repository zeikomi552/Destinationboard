using Destinationboard.Common.Utilities;
using System;
using System.Windows;

namespace Destinationboard.Models
{
    public class MagnetM : ModelBase
    {
		#region 項目識別子[ID]プロパティ
		/// <summary>
		/// 項目識別子[ID]プロパティ用変数
		/// </summary>
		string _ID = Guid.NewGuid().ToString();
		/// <summary>
		/// 項目識別子[ID]プロパティ
		/// </summary>
		public string ID
		{
			get
			{
				if (string.IsNullOrEmpty(_ID))
				{
					_ID = Guid.NewGuid().ToString();
				}

				return _ID;
			}
			set
			{
				if (!_ID.Equals(value))
				{
					_ID = value;
					NotifyPropertyChanged("ID");
				}
			}
		}
		#endregion

		#region 文字列[Text]プロパティ
		/// <summary>
		/// 文字列[Text]プロパティ用変数
		/// </summary>
		string _Text = string.Empty;
		/// <summary>
		/// 文字列[Text]プロパティ
		/// </summary>
		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				if (!_Text.Equals(value))
				{
					_Text = value;
					NotifyPropertyChanged("Text");
				}
			}
		}
		#endregion

		#region 文字色(ファイル出力用HEX)[Foreground]プロパティ
		/// <summary>
		/// 文字色(ファイル出力用HEX)[Foreground]プロパティ用変数
		/// </summary>
		string _Foreground = "#FF000000";
		/// <summary>
		/// 文字色(ファイル出力用HEX)[Foreground]プロパティ
		/// </summary>
		public string Foreground
		{
			get
			{
				return _Foreground;
			}
			set
			{
				if (!_Foreground.Equals(value))
				{
					_Foreground = value;
					NotifyPropertyChanged("Foreground");
				}
			}
		}
		#endregion

		#region 背景色(ファイル出力用HEX)[Background]プロパティ
		/// <summary>
		/// 背景色(ファイル出力用HEX)[Background]プロパティ用変数
		/// </summary>
		string _Background = "#FFFFFFFF";
		/// <summary>
		/// 背景色(ファイル出力用HEX)[Background]プロパティ
		/// </summary>
		public string Background
		{
			get
			{
				return _Background;
			}
			set
			{
				if (!_Background.Equals(value))
				{
					_Background = value;
					NotifyPropertyChanged("Background");
				}
			}
		}
		#endregion

		#region マップでの位置情報[MapPos]プロパティ
		/// <summary>
		/// マップでの位置情報[MapPos]プロパティ用変数
		/// </summary>
		Point _MapPos = new Point(0, 0);
		/// <summary>
		/// マップでの位置情報[MapPos]プロパティ
		/// </summary>
		public Point MapPos
		{
			get
			{
				return _MapPos;
			}
			set
			{
				if (!_MapPos.Equals(value))
				{
					_MapPos = value;
					NotifyPropertyChanged("MapPos");
				}
			}
		}
		#endregion

		/// <summary>
		/// シャローコピー
		/// </summary>
		/// <returns>コピー結果</returns>
		public MagnetM ShallowCopy()
		{
			return (MagnetM)MemberwiseClone();
		}
	}
}
