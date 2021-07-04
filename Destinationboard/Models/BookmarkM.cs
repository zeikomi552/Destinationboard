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

	}
}
