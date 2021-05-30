using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Destinationboard.Common.Converters
{
	[System.Windows.Data.ValueConversion(typeof(string), typeof(SolidColorBrush))]
	public class HexToSolidColorBrushConverter : System.Windows.Data.IValueConverter
	{
		#region IValueConverter メンバ
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string target = (string)value;

            if (target.Length == 9)
            {
				try
				{
					byte a = System.Convert.ToByte(int.Parse(target.Substring(1, 2), System.Globalization.NumberStyles.HexNumber));
					byte r = System.Convert.ToByte(int.Parse(target.Substring(3, 2), System.Globalization.NumberStyles.HexNumber));
					byte g = System.Convert.ToByte(int.Parse(target.Substring(5, 2), System.Globalization.NumberStyles.HexNumber));
					byte b = System.Convert.ToByte(int.Parse(target.Substring(7, 2), System.Globalization.NumberStyles.HexNumber));
					var c = Color.FromArgb(a, r, g, b);
					return new SolidColorBrush(c);
				}
				catch
                {
					return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)); ;
				}
			}
            else
			{
				return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)); ;
			}
		}

		// TwoWayの場合に使用する
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			SolidColorBrush target = (SolidColorBrush)value;

			if (target != null)
			{
				var c = target.Color;
				return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.A, c.R, c.G, c.B);
			}
			else
			{
				return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (byte)255, (byte)255, (byte)255, (byte)255); ;
			}
		}

		#endregion
	}

}
