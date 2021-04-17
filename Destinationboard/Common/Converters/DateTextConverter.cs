using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Common.Converters
{
	[System.Windows.Data.ValueConversion(typeof(DateTime?), typeof(String))]
	public class DateTextConverter : System.Windows.Data.IValueConverter
	{
		#region IValueConverter メンバ
		/// <summary>
		/// 日付型を以下の条件で文字列に変換する
		/// 当日ならば時間を表示する
		/// 当日以外なら日付を表示する
		/// </summary>
		/// <param name="value">バインドする値</param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			DateTime? target = (DateTime?)value;


			if (target.HasValue)
			{
				if (target.Value.Date.Equals(DateTime.Today))
				{
					return target.Value.ToString("H:mm");
				}
				else
				{
					return target.Value.ToString("M/dd(ddd)");
				}
			}
			else
			{
				return string.Empty;
			}
		}

		// TwoWayの場合に使用する
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}


}
