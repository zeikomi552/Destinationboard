using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Destinationboard.Common.Helpers
{
    public static class DocumentViewerManipulationHelper
    {
        /// <summary>
        /// ViewModelから制御するための依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ManipulationFeedbackProperty =
            DependencyProperty.RegisterAttached(
                "ManipulationFeedback",
                typeof(bool?),
                typeof(DocumentViewerManipulationHelper),
                new PropertyMetadata((d, e) =>
                {
                    var docv = d as DocumentViewer;
                    if (docv != null)
                    {
                        var tmp = (bool?)docv.GetValue(ManipulationFeedbackProperty);

                        if (tmp.HasValue && tmp.Value == true)
                        {
                            //ここ（コールバック）でWindowのManipulationFeedbackプロパティを設定（画面が閉じられる）
                            docv.ManipulationBoundaryFeedback += Docv_ManipulationBoundaryFeedback;
                        }
                        else
                        {
                            docv.ManipulationBoundaryFeedback -= Docv_ManipulationBoundaryFeedback;
                        }
                    }
                }));

        private static void Docv_ManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Xamlから添付プロパティとして設定させるためのメソッド
        /// </summary>
        public static void SetManipulationFeedback(DocumentViewer target, bool? value)
        {
            target.SetValue(ManipulationFeedbackProperty, value);
        }
    }
}
