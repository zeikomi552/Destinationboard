using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Destinationboard.Common.Utilities
{
    public static class IntExtensions
    {
        /// <summary>
        /// 数値をExcelのカラム文字へ変換します
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string ToColumnName(this int source)
        {
            if (source < 1) return string.Empty;
            return ToColumnName((source - 1) / 26) + (char)('A' + ((source - 1) % 26));
        }
    }

    public class Utilities
    {


        #region 最上位のWindowの取得処理
        /// <summary>
        /// 最上位のWindowの取得処理
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static DependencyObject GetWindow<T>(object sender)
        {
            DependencyObject depobj = (DependencyObject)sender;
            while (true)
            {
                depobj = VisualTreeHelper.GetParent(depobj);

                if (depobj is T)
                {
                    return depobj;
                }
                else if (depobj == null)
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
