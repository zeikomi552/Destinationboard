using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Destinationboard.Common.Helpers
{
    public static class InkCanvasColorHelper
    {
        /// <summary>
        /// ViewModelから制御するための依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.RegisterAttached(
                "Color",
                typeof(Color),
                typeof(InkCanvasColorHelper),
                new PropertyMetadata((d, e) =>
                {
                    var canvas = d as InkCanvas;
                    if (canvas != null)
                    {
                        canvas.DefaultDrawingAttributes.Color = e.NewValue is Color ? (Color)e.NewValue : Colors.Black;
                    }
                }));

        /// <summary>
        /// Xamlから添付プロパティとして設定させるためのメソッド
        /// </summary>
        public static void SetColor(InkCanvas target, Color value)
        {
            target.SetValue(ColorProperty, value);
        }
    }
}
