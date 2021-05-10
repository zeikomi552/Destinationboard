using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Common.Converters
{
	[System.Windows.Data.ValueConversion(typeof(string), typeof(string))]
	public class FelicaIDToHusejiConverter : System.Windows.Data.IValueConverter
	{
		#region IValueConverter メンバ
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string target = (string)value;

			string ret = string.Empty;

			if (target.Length > 0)
			{
				string[] split = target.Split("-");

				for (int index = 0; index < split.Length; index++)
                {
					if (index == 0)
					{
						ret = "XX";
					}
					else if (index == split.Length - 1)
					{
						ret += "-" + split[index];
					}
					else
					{
						ret += "-XX";
					}
                }
			}

			// ここに処理を記述する
			return ret;
		}

		// TwoWayの場合に使用する
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			//throw new NotImplementedException();
			return value;
		}

		#endregion
	}

}
