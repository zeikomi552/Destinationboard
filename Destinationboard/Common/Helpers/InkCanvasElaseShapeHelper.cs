using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace Destinationboard.Common.Helpers
{
    class InkCanvasElaseShapeHelper
    {
        /// <summary>
        /// ViewModelから制御するための依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ElaseShapeProperty =
            DependencyProperty.RegisterAttached(
                "ElaseShape",
                typeof(int),
                typeof(InkCanvasElaseShapeHelper),
                new PropertyMetadata((d, e) =>
                {
                    var canvas = d as InkCanvas;

                    if (canvas != null)
                    {
                        canvas.EraserShape = new EllipseStylusShape((int)e.NewValue, (int)e.NewValue);
                    }
                }));

        /// <summary>
        /// Xamlから添付プロパティとして設定させるためのメソッド
        /// </summary>
        public static void SetElaseShape(InkCanvas target, int value)
        {
            target.SetValue(ElaseShapeProperty, value);
        }
    }
}
