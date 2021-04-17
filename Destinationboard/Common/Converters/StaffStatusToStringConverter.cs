using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Common.Converters
{
	[System.Windows.Data.ValueConversion(typeof(int), typeof(string))]
	public class StaffStatusToStringConverter : System.Windows.Data.IValueConverter
	{
		#region IValueConverter メンバ
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int target = (int)value;

			switch (target)
			{
				case 0:
				default:
					{
						return "帰宅";
					}
				case 1:
                    {
						return "出社";
                    }
				case 2:
					{
						return "テレワーク";
					}

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
