using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Destinationboard.Common.Helpers
{
    public static class inkCanvasSizeHelper
    {
        /// <summary>
        /// ViewModelから制御するための依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.RegisterAttached(
                "Size",
                typeof(int),
                typeof(inkCanvasSizeHelper),
                new PropertyMetadata((d, e) =>
                {
                    var canvas = d as InkCanvas;

                    if (canvas != null)
                    {
                        canvas.DefaultDrawingAttributes.Width = (int)e.NewValue;
                        canvas.DefaultDrawingAttributes.Height = (int)e.NewValue;
                    }
                }));

        /// <summary>
        /// Xamlから添付プロパティとして設定させるためのメソッド
        /// </summary>
        public static void SetSize(InkCanvas target, int value)
        {
            target.SetValue(SizeProperty, value);
        }
    }
}
