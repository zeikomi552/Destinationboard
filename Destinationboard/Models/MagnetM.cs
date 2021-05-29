using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class MagnetM : ModelBase
    {
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

		#region 文字色[Foreground]プロパティ
		/// <summary>
		/// 文字色[Foreground]プロパティ用変数
		/// </summary>
		Brush _Foreground = Brushes.Black;
		/// <summary>
		/// 文字色[Foreground]プロパティ
		/// </summary>
		public Brush Foreground
		{
			get
			{
				return _Foreground;
			}
			set
			{
				if (_Foreground == null || !_Foreground.Equals(value))
				{
					_Foreground = value;
					NotifyPropertyChanged("Foreground");
				}
			}
		}
		#endregion

		/// <summary>
		/// 浅いコピー
		/// </summary>
		/// <returns>コピー結果</returns>
		public MagnetM ShallowCopy()
		{
			return (MagnetM)MemberwiseClone();
		}
	}
}
