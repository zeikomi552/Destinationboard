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
using System.Windows.Media;

namespace Destinationboard.ViewModels
{
    public class MediaVM : ViewModelBase
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

        #region メディアのファイルパス[MediaPath]プロパティ
        /// <summary>
        /// メディアのファイルパス[MediaPath]プロパティ用変数
        /// </summary>
        string _MediaPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Common\Themes\media\media1.mp4";
        /// <summary>
        /// メディアのファイルパス[MediaPath]プロパティ
        /// </summary>
        public string MediaPath
        {
            get
            {
                return _MediaPath;
            }
            set
            {
                if (!_MediaPath.Equals(value))
                {
                    _MediaPath = value;
                    NotifyPropertyChanged("MediaPath");
                }
            }
        }
        #endregion

        #region メディアステータス[MediaState]プロパティ
        /// <summary>
        /// メディアステータス[MediaState]プロパティ用変数
        /// </summary>
        MediaState _MediaState = MediaState.Stop;
        /// <summary>
        /// メディアステータス[MediaState]プロパティ
        /// </summary>
        public MediaState MediaState
        {
            get
            {
                return _MediaState;
            }
            set
            {
                if (!_MediaState.Equals(value))
                {
                    _MediaState = value;
                    NotifyPropertyChanged("MediaState");
                }
            }
        }
        #endregion

        #region マップ変更処理
        /// <summary>
        /// マップ変更処理
        /// </summary>
        public void MediaChange()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "動画ファイル (*.mp4)|*.mp4";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.MediaPath = dialog.FileName;
                    // 動画ファイルをロードします。
                    this._Media.LoadedBehavior = MediaState.Stop;
                    this._Media.Source = new Uri(this.MediaPath, UriKind.Relative);

                    NotifyPropertyChanged("MediaPath");
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        MediaElement _Media;
        public void InitMedia(object sender, EventArgs e)
        {
            try
            {
                var tmp = Utilities.GetWindow<UserControl>(sender) as MediaV;
                if (tmp != null)
                {
                    this._Media = tmp.MediaVideo;

                    if (File.Exists(this.MediaPath))
                    {
                        // 動画ファイルをロードします。
                        this._Media.Source = new Uri(this.MediaPath, UriKind.Relative);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

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
    }
}
