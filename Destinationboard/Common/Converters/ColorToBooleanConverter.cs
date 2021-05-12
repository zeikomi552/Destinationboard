using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Destinationboard.Common.Converters
{
	[System.Windows.Data.ValueConversion(typeof(Color), typeof(Boolean))]
	public class ColorToBooleanConverter : System.Windows.Data.IValueConverter
	{
		#region IValueConverter メンバ
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Color target = (Color)value;
			string check = (string)(parameter);

			if (check.Equals("Black") && target.Equals(Colors.Black))
			{
				return true;
			}
			else if (check.Equals("Blue") && target.Equals(Colors.Blue))
			{
				return true;
			}
			else if (check.Equals("Red") && target.Equals(Colors.Red))
			{
				return true;
			}
			else if (check.Equals("Yellow") && target.Equals(Colors.Yellow))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		// TwoWayの場合に使用する
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool target = (bool)value;
			string check = (string)(parameter);

			if (target)
			{
				if (check.Equals("Black"))
				{
					return Colors.Black;
				}
				else if (check.Equals("Blue"))
				{
					return Colors.Blue;
				}
				else if (check.Equals("Red"))
				{
					return Colors.Red;
				}
				else if (check.Equals("Yellow"))
				{
					return Colors.Yellow;
				}
			}

			return Colors.White;

		}

		#endregion
	}

}
