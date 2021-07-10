using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class BookmarkM : ModelBase
    {
		#region 名称[Name]プロパティ
		/// <summary>
		/// 名称[Name]プロパティ用変数
		/// </summary>
		string _Name = string.Empty;
		/// <summary>
		/// 名称[Name]プロパティ
		/// </summary>
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if (!_Name.Equals(value))
				{
					_Name = value;
					NotifyPropertyChanged("Name");
				}
			}
		}
		#endregion

		#region URI[URI]プロパティ
		/// <summary>
		/// URI[URI]プロパティ用変数
		/// </summary>
		string _URI = string.Empty;
		/// <summary>
		/// URI[URI]プロパティ
		/// </summary>
		public string URI
		{
			get
			{
				return _URI;
			}
			set
			{
				if (!_URI.Equals(value))
				{
					_URI = value;
					NotifyPropertyChanged("URI");
				}
			}
		}
		#endregion

		#region お気に入りのセット
		/// <summary>
		/// お気に入りのセット
		/// </summary>
		/// <param name="uri">URI</param>
		public void SetBookMark(string uri)
		{
			this.Name = uri;
			this.URI = uri;
		}
		#endregion
	}
}
