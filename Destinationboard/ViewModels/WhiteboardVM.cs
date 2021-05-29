using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Destinationboard.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        #region 背景イメージパス[ImagePath]プロパティ
        /// <summary>
        /// 背景イメージパス[ImagePath]プロパティ用変数
        /// </summary>
        string _ImagePath = string.Empty;
        /// <summary>
        /// 背景イメージパス[ImagePath]プロパティ
        /// </summary>
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                if (!_ImagePath.Equals(value))
                {
                    _ImagePath = value;
                    NotifyPropertyChanged("ImagePath");
                }
            }
        }
        #endregion

        private string _StorkePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Common\Themes\map\canvas1-stroke";
        private bool handle = true;
        InkCanvas _InkCanvas;
        List<StrokePairM> _StrokeUndo = new List<StrokePairM>();
        List<StrokePairM> _StrokeRedo = new List<StrokePairM>();

        private string _MagnetFilePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Common\Themes\map\wndname-magnet";

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


        private string _WndName = string.Empty;
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
                    _InkCanvas = wnd.theInkCanvas;
                    _WndName = wnd.Name;

                    // Configフォルダのパス取得
                    string conf_dir = Path.Combine(Utilities.GetApplicationFolder(), "temporary");
                    string image_file_path = Path.Combine(conf_dir, string.Format("{0}-layout", wnd.Name));


                    this.ImagePath = Path.Combine(conf_dir, string.Format("{0}-layout", wnd.Name));
                    this._StorkePath = Path.Combine(conf_dir, string.Format("{0}-stroke", wnd.Name)); 

                    if (File.Exists(this._StorkePath))
                    {
                        using (System.IO.FileStream fs =
                            new System.IO.FileStream(this._StorkePath, System.IO.FileMode.Open))
                        {
                            this._InkCanvas.Strokes = new System.Windows.Ink.StrokeCollection(fs);
                        }
                    }
                    wnd.theInkCanvas.Strokes.StrokesChanged -= Strokes_StrokesChanged;
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
                    this._StrokeUndo.Add(new StrokePairM(e.Added, e.Removed));
                    this._StrokeRedo.Clear();

                    using (System.IO.FileStream fs =
                        new System.IO.FileStream(this._StorkePath, System.IO.FileMode.Create))
                    {
                        this._InkCanvas.Strokes.Save(fs);
                    }
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

                    // 最後の変更を取り出す
                    var tmp = this._StrokeUndo.LastOrDefault();

                    // nullチェック
                    if (tmp != null)
                    {
                        // Redo用に保存する
                        _StrokeRedo.Add(new StrokePairM(tmp.AddedStroke, tmp.RemovedStroke));

                        // 最後に追加された分は取り除く
                        wnd.theInkCanvas.Strokes.Remove(tmp.AddedStroke);

                        // 最後に取り除かれた分は追加する
                        wnd.theInkCanvas.Strokes.Add(tmp.RemovedStroke);

                        // Undoのリストから削除する
                        this._StrokeUndo.Remove(tmp);
                    }

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

                    // 最後の変更を取り出す
                    var tmp = this._StrokeRedo.LastOrDefault();

                    if (tmp != null)
                    {
                        // Undoで消されたストロークを追加
                        wnd.theInkCanvas.Strokes.Add(tmp.AddedStroke);

                        // Undoで戻されたストロークを削除
                        wnd.theInkCanvas.Strokes.Remove(tmp.RemovedStroke);

                        // Undo用のストロークを保存
                        this._StrokeUndo.Add(new StrokePairM(tmp.AddedStroke, tmp.RemovedStroke));

                        // Redo用のストロークを削除
                        this._StrokeRedo.Remove(tmp);
                    }

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

        #region 保存ボタン処理(.png)
        /// <summary>
        /// 保存ボタン処理(.png)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion

        #region マグネットリスト[Magnets]プロパティ
        /// <summary>
        /// マグネットリスト[Magnets]プロパティ用変数
        /// </summary>
        MagnetCollectionM _Magnets = new MagnetCollectionM();
        /// <summary>
        /// マグネットリスト[Magnets]プロパティ
        /// </summary>
        public MagnetCollectionM Magnets
        {
            get
            {
                return _Magnets;
            }
            set
            {
                if (_Magnets == null || !_Magnets.Equals(value))
                {
                    _Magnets = value;
                    NotifyPropertyChanged("Magnets");
                }
            }
        }
        #endregion

        /// <summary>
        /// マグネットの作成
        /// </summary>
        public void CreateMagnet()
        {
            try
            {
                MagnetV wnd = new MagnetV();

                if (wnd.ShowDialog() == true)
                {
                    var vm = wnd.DataContext as MagnetVM;
                    this.Magnets = vm.Magnets.Clone();

                    // Configフォルダのパス取得
                    string conf_dir = Path.Combine(Utilities.GetApplicationFolder(), "temporary");
                    string magnet_path = Path.Combine(conf_dir, string.Format("{0}-magnet", wnd.Name));


                    XMLUtil.Seialize<MagnetCollectionM>(magnet_path, this.Magnets);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }

    }

}
