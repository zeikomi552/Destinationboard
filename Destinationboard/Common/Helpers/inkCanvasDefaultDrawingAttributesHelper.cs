using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Destinationboard.Common.Helpers
{
    class inkCanvasHighlighterHelper
    {
        /// <summary>
        /// ViewModelから制御するための依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty HighlighterProperty =
            DependencyProperty.RegisterAttached(
                "Highlighter",
                typeof(bool),
                typeof(inkCanvasHighlighterHelper),
                new PropertyMetadata((d, e) =>
                {
                    var canvas = d as InkCanvas;

                    if (canvas != null)
                    {
                        canvas.DefaultDrawingAttributes.IsHighlighter = (bool)e.NewValue;
                    }
                }));

        /// <summary>
        /// Xamlから添付プロパティとして設定させるためのメソッド
        /// </summary>
        public static void SetHighlighter(InkCanvas target, int value)
        {
            target.SetValue(HighlighterProperty, value);
        }
    }
}
