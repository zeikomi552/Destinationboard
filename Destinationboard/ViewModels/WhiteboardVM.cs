using Destinationboard.Common.Utilities;
using Destinationboard.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Destinationboard.ViewModels
{
    public class WhiteboardVM : ViewModelBase
    {
        #region ホワイトボード用マーカーカラー[MarkerColor]プロパティ
        /// <summary>
        /// ホワイトボード用マーカーカラー[MarkerColor]プロパティ用変数
        /// </summary>
        Color _MarkerColor = Colors.Black;
        /// <summary>
        /// ホワイトボード用マーカーカラー[MarkerColor]プロパティ
        /// </summary>
        public Color MarkerColor
        {
            get
            {
                return _MarkerColor;
            }
            set
            {
                if (!_MarkerColor.Equals(value))
                {
                    _MarkerColor = value;
                    NotifyPropertyChanged("MarkerColor");
                }
            }
        }
        #endregion
        #region ペンサイズ[Size]プロパティ
        /// <summary>
        /// ペンサイズ[Size]プロパティ用変数
        /// </summary>
        int _Size = 5;
        /// <summary>
        /// ペンサイズ[Size]プロパティ
        /// </summary>
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                if (!_Size.Equals(value))
                {
                    _Size = value;
                    NotifyPropertyChanged("Size");
                }
            }
        }
        #endregion

        #region 背景イメージのパス[ImagePath]プロパティ
        /// <summary>
        /// 背景イメージのパス[ImagePath]プロパティ
        /// </summary>
        public string ImagePath
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + @"Common\Themes\map\canvas1-layout";
            }
        }
        #endregion
        #region 背景変更処理
        /// <summary>
        /// 背景変更処理
        /// </summary>
        public void BackgroundChange()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "画像ファイル (*.png;*.jpg;*.gif;*.bmp)|*.png;*.jpg;*.gif;*.bmp";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    string file_name = this.ImagePath;
                    File.Copy(dialog.FileName, file_name, true);


                    NotifyPropertyChanged("ImagePath");
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 背景画像の消去
        /// <summary>
        /// 背景画像の消去
        /// </summary>
        public void BackgroundClear()
        {
            try
            {
                File.Delete(this.ImagePath);
                NotifyPropertyChanged("ImagePath");
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        System.Windows.Ink.StrokeCollection _added;
        System.Windows.Ink.StrokeCollection _removed;
        private bool handle = true;

        #region 書き込みモード[EditingMode]プロパティ
        /// <summary>
        /// 書き込みモード[EditingMode]プロパティ用変数
        /// </summary>
        InkCanvasEditingMode _EditingMode = InkCanvasEditingMode.Ink;
        /// <summary>
        /// 書き込みモード[EditingMode]プロパティ
        /// </summary>
        public InkCanvasEditingMode EditingMode
        {
            get
            {
                return _EditingMode;
            }
            set
            {
                if (!_EditingMode.Equals(value))
                {
                    _EditingMode = value;
                    NotifyPropertyChanged("EditingMode");
                }
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Init()
        {
            try
            {
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region InkCanvasの初期化処理
        /// <summary>
        /// InkCanvasの初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void InkCanvasInit(object sender, EventArgs e)
        {
            try
            {
                var wnd = Utilities.GetWindow<UserControl>(sender) as WhiteboardV;

                if (wnd != null)
                {
                    wnd.theInkCanvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }

        }
        #endregion


        #region Close処理
        /// <summary>
        /// Close処理
        /// </summary>
        public override void Close()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
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
            try
            {
                var wnd = Utilities.GetWindow<UserControl>(sender) as WhiteboardV;

                if (wnd != null)
                {
                    wnd.theInkCanvas.Strokes.Clear();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region ストロークが変化した場合の処理
        /// <summary>
        /// ストロークが変化した場合の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {
            try
            {
                if (handle)
                {
                    _added = e.Added;
                    _removed = e.Removed;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Undo処理
        /// <summary>
        /// Undo処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Undo(object sender, RoutedEventArgs e)
        {
            try
            {
                var wnd = Utilities.GetWindow<UserControl>(sender) as WhiteboardV;

                if (wnd != null)
                {
                    handle = false;
                    wnd.theInkCanvas.Strokes.Remove(_added);
                    wnd.theInkCanvas.Strokes.Add(_removed);
                    handle = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Redo処理
        /// <summary>
        /// Redo処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Redo(object sender, RoutedEventArgs e)
        {
            try
            {
                var wnd = Utilities.GetWindow<UserControl>(sender) as WhiteboardV;

                if (wnd != null)
                {
                    handle = false;
                    wnd.theInkCanvas.Strokes.Add(_added);
                    wnd.theInkCanvas.Strokes.Remove(_removed);
                    handle = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        // [保存]ボタンクリック時の処理
        public void Save(object sender, RoutedEventArgs e)
        {
            var wnd = Utilities.GetWindow<UserControl>(sender) as WhiteboardV;

            if (wnd != null)
            {
                Microsoft.Win32.SaveFileDialog dlgSave = new Microsoft.Win32.SaveFileDialog();

                dlgSave.Filter = "PNGファイル(*.png)|*.png";
                dlgSave.AddExtension = true;

                if ((bool)dlgSave.ShowDialog())
                {
                    // レンダリング
                    var bmp = new RenderTargetBitmap(
                        (int)wnd.Drawgrid.ActualWidth,
                        (int)wnd.Drawgrid.ActualHeight,
                        96, 96, // DPI
                        PixelFormats.Pbgra32);
                    bmp.Render(wnd.Drawgrid);

                    // jpegで保存
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var fs = File.Open(dlgSave.FileName, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
            }
        }
    }

}
