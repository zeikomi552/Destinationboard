using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Destinationboard.Common.Converters
{
	[System.Windows.Data.ValueConversion(typeof(MediaState), typeof(bool))]
	public class MediaStateToBooleanConverter : System.Windows.Data.IValueConverter
	{
		#region IValueConverter メンバ
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			MediaState target = (MediaState)value;
			string name = parameter.ToString();

			if (target.ToString().Equals(name))
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
			bool value_tmp = (bool)value;
			string param = parameter.ToString();

			MediaState en;

			if (MediaState.TryParse(param, out en))
			{
				return en;
			}
			else
			{
				return MediaState.Stop;
			}
		}

		#endregion
	}

}
