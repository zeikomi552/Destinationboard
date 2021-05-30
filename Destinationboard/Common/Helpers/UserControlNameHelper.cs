using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Destinationboard.Common.Helpers
{
    public static class UserControlNameHelper
    {
        /// <summary>
        /// ViewModelから制御するための依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached(
                "Name",
                typeof(string),
                typeof(UserControlNameHelper),
                new PropertyMetadata((d, e) =>
                {
                    var uc = d as UserControl;
                    if (uc != null)
                    {
                        //ここ（コールバック）でWindowのDialogResultプロパティを設定（画面が閉じられる）
                        uc.Name = e.NewValue as string;
                    }
                }));

        /// <summary>
        /// Xamlから添付プロパティとして設定させるためのメソッド
        /// </summary>
        public static void SetName(UserControl target, string value)
        {
            target.SetValue(NameProperty, value);
        }
    }
}
