using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Destinationboard.Common.Converters
{
	[System.Windows.Data.ValueConversion(typeof(int), typeof(bool))]
	public class IntToBooleanConverter : System.Windows.Data.IValueConverter
	{
		public int Default { get; set; } = -1;


		#region IValueConverter メンバ
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string param = parameter.ToString();
			int input_param = this.Default;
			int target = (int)value;

			// 入力文字列を数値に変換
			if (int.TryParse(param, out input_param) == true)
			{
				// パラメータと一致する場合はtrueを返却する
				if (target == input_param)
				{
					return true;
				}
			}


			// ここに処理を記述する
			return false;
		}

		// TwoWayの場合に使用する
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool value_tmp = (bool)value;

			string param = parameter.ToString();
			int input_param = this.Default;

			// 入力文字列を数値に変換
			if (int.TryParse(param, out input_param) == true)
			{
				if (value_tmp)
				{
					return input_param;
				}
			}

			return input_param;
		}

		#endregion
	}

}
