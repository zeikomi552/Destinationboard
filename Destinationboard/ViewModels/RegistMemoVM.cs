using Destinationboard.Common.Utilities;
using Destinationboard.Views;
using Microsoft.Ink;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Destinationboard.ViewModels
{
    public class RegistMemoVM : ViewModelBase
    {
        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Init()
        {
            try
            {
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 入力文字列[InputText]プロパティ
        /// <summary>
        /// 入力文字列[InputText]プロパティ用変数
        /// </summary>
        string _InputText = string.Empty;
        /// <summary>
        /// 入力文字列[InputText]プロパティ
        /// </summary>
        public string InputText
        {
            get
            {
                return _InputText;
            }
            set
            {
                if (!_InputText.Equals(value))
                {
                    _InputText = value;
                    NotifyPropertyChanged("InputText");
                }
            }
        }
        #endregion

        #region 最上位のWindowの取得処理
        /// <summary>
        /// 最上位のWindowの取得処理
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public RegistMemoV GetWindow(object sender)
        {
            DependencyObject depobj = (Button)sender;
            while (true)
            {
                depobj = VisualTreeHelper.GetParent(depobj);

                if (depobj is Window)
                {
                    break;
                }
            }

            return  depobj as RegistMemoV;
        }
        #endregion

        #region 手書き文字認識
        /// <summary>
        /// 手書き文字認識
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Recognization(object sender, RoutedEventArgs e)
        {
            var wnd = GetWindow(sender);

            using (MemoryStream ms = new MemoryStream())
            {
                wnd.theInkCanvas.Strokes.Save(ms);
                var myInkCollector = new InkCollector();
                var ink = new Ink();
                ink.Load(ms.ToArray());

                using (RecognizerContext context = new RecognizerContext())
                {
                    if (ink.Strokes.Count > 0)
                    {
                        context.Strokes = ink.Strokes;
                        RecognitionStatus status;

                        var result = context.Recognize(out status);

                        if (status == RecognitionStatus.NoError)
                            this.InputText = result.TopString;
                        else
                            MessageBox.Show("Recognition failed");
                    }
                    else
                    {
                        MessageBox.Show("No stroke detected");
                    }
                }
            }
        }
        #endregion

        #region クリア処理
        /// <summary>
        /// クリア処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Clear(object sender, RoutedEventArgs e)
        {
            var wnd = GetWindow(sender);
            wnd.theInkCanvas.Strokes.Clear();
        }
        #endregion

        #region 登録処理
        /// <summary>
        /// 登録処理
        /// </summary>
        public void Regist(object sender, RoutedEventArgs e)
        {
            try
            {
                Recognization(sender, e);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                _logger.Error("致命的なエラー", ex);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 閉じる処理
        /// <summary>
        /// 閉じる処理
        /// </summary>
        public override void Close()
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion
    }
}
