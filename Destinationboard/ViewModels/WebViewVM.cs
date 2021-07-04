using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Destinationboard.Views;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Destinationboard.ViewModels
{
    public class WebViewVM : ViewModelBase
    {
        #region スライドショーフラグ(true:スライドショー false:停止)[IsSlideShow]プロパティ
        /// <summary>
        /// スライドショーフラグ(true:スライドショー false:停止)[IsSlideShow]プロパティ用変数
        /// </summary>
        bool _IsSlideShow = false;
        /// <summary>
        /// スライドショーフラグ(true:スライドショー false:停止)[IsSlideShow]プロパティ
        /// </summary>
        public bool IsSlideShow
        {
            get
            {
                return _IsSlideShow;
            }
            set
            {
                if (!_IsSlideShow.Equals(value))
                {
                    _IsSlideShow = value;
                    NotifyPropertyChanged("IsSlideShow");
                }
            }
        }
        #endregion


        const string DefaultURI = "https://www.google.com/";
        #region 表示URI[URI]プロパティ
        /// <summary>
        /// 表示URI[URI]プロパティ用変数
        /// </summary>
        string _URI = DefaultURI;
        /// <summary>
        /// 表示URI[URI]プロパティ
        /// </summary>
        public string URI
        {
            get
            {
                return _URI;
            }
            set
            {
                if (!_URI.Equals(value))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        // 空白なのでGoogleのトップ画面へ
                        this._URI = "https://www.google.com/";
                    }
                    else if (value.Contains("http://") || value.Contains("https://"))
                    {
                        this._URI = value; // そのまま使用
                    }
                    else
                    {
                        // URLではないのでGoogle検索を実行
                        this._URI = string.Format("https://www.google.com/search?q={0}", value);
                    }
                    NotifyPropertyChanged("URI");
                }
            }
        }
        #endregion

        #region お気に入りリスト[Bookmarks]プロパティ
        /// <summary>
        /// お気に入りリスト[Bookmarks]プロパティ用変数
        /// </summary>
        ModelList<BookmarkM> _Bookmarks = new ModelList<BookmarkM>();
        /// <summary>
        /// お気に入りリスト[Bookmarks]プロパティ
        /// </summary>
        public ModelList<BookmarkM> Bookmarks
        {
            get
            {
                return _Bookmarks;
            }
            set
            {
                if (_Bookmarks == null || !_Bookmarks.Equals(value))
                {
                    _Bookmarks = value;
                    NotifyPropertyChanged("Bookmarks");
                }
            }
        }
        #endregion


        #region WebView2オブジェクト[WebView2Obj]プロパティ
        /// <summary>
        /// WebView2オブジェクト[WebView2Obj]プロパティ用変数
        /// </summary>
        WebView2 _WebView2Obj = null;
        /// <summary>
        /// WebView2オブジェクト[WebView2Obj]プロパティ
        /// </summary>
        public WebView2 WebView2Obj
        {
            get
            {
                return _WebView2Obj;
            }
            set
            {
                if (_WebView2Obj == null || !_WebView2Obj.Equals(value))
                {
                    _WebView2Obj = value;
                    NotifyPropertyChanged("WebView2Obj");
                }
            }
        }
        #endregion


        #region 画面の初期化処理
        /// <summary>
        /// 画面の初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        public void InitDisplay(object sender, EventArgs ev)
        {
            try
            {
                // お気に入りの読み込み
                LoadBookMark();

                if (this.WebView2Obj == null)
                {
                    var tmp = sender as WebViewV;
                    this.WebView2Obj = tmp.webView2;
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        // タイマのインスタンス
        private DispatcherTimer _SlideShowTimer;

        #region 画面の初期化処理
        /// <summary>
        /// 画面の初期化処理
        /// </summary>
        public override void Init()
        {
            try
            {
                _SlideShowTimer = new DispatcherTimer();
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region クローズ処理
        /// <summary>
        /// クローズ処理
        /// </summary>
        public override void Close()
        {

        }
        #endregion

        #region Web画面遷移
        /// <summary>
        /// Web画面遷移
        /// </summary>
        public void WebMove()
        {
            try
            {
                WebMove(this.URI);
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 画面遷移処理
        /// <summary>
        /// 画面遷移処理
        /// </summary>
        /// <param name="uri">URI</param>
        private void WebMove(string uri)
        {
            try
            {
                this.WebView2Obj.CoreWebView2.Navigate(Destinationboard.Common.Utilities.Utilities.ConvertURI(uri));
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }

        }
        #endregion

        #region お気に入りの選択変更
        /// <summary>
        /// お気に入りの選択変更
        /// </summary>
        public void BookMarkSelectionChanged()
        {
            try
            {
                this.WebView2Obj.CoreWebView2.Navigate(Destinationboard.Common.Utilities.Utilities.ConvertURI(this.Bookmarks.SelectedItem.URI));
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region ブックマーク処理
        /// <summary>
        /// ブックマーク処理
        /// </summary>
        public void BookMark()
        {
            try
            {
                this.Bookmarks.Add(new BookmarkM() { URI = this.URI });

                // ブックマークの保存
                SaveBookMark();

            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 上へ移動
        /// <summary>
        /// 上へ移動
        /// </summary>
        public void MoveUp()
        {
            try
            {
                this.Bookmarks.MoveUp();

                // ブックマークの保存
                SaveBookMark();

                NotifyPropertyChanged("Bookmarks");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 下へ移動処理
        /// <summary>
        /// 下へ移動処理
        /// </summary>
        public void MoveDown()
        {
            try
            {
                this.Bookmarks.MoveDown();

                // ブックマークの保存
                SaveBookMark();

                NotifyPropertyChanged("Bookmarks");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 削除処理
        /// <summary>
        /// 削除処理
        /// </summary>
        public void Delete()
        {
            try
            {
                this.Bookmarks.DeleteSelectedItem();

                // ブックマークの保存
                SaveBookMark();

                NotifyPropertyChanged("Bookmarks");
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion


        private string _BookmarkDir = "Config";
        private string _BookmarkName = "_bookmarks";

        #region マップ情報のロード処理
        /// <summary>
        /// マップ情報のロード処理
        /// </summary>
        private void LoadBookMark()
        {
            try
            {

                // Configフォルダのパス取得
                string conf_dir = Path.Combine(Utilities.GetApplicationFolder(), _BookmarkDir);
                string map_bk_path = Path.Combine(conf_dir, _BookmarkName);


                if (File.Exists(map_bk_path))
                {
                    this.Bookmarks = XMLUtil.Deserialize<ModelList<BookmarkM>>(map_bk_path);
                }
                else
                {
                    // カレントディレクトリの作成
                    Utilities.CreateCurrentDirectory(map_bk_path);

                    // マップ情報の保存
                    SaveBookMark();
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region マップの配置情報の保存
        /// <summary>
        /// マップの配置情報の保存
        /// </summary>
        private void SaveBookMark()
        {
            try
            {
                // Configフォルダのパス取得
                string conf_dir = Path.Combine(Utilities.GetApplicationFolder(), _BookmarkDir);
                string map_bk_path = Path.Combine(conf_dir, _BookmarkName);


                XMLUtil.Seialize<ModelList<BookmarkM>>(map_bk_path, this.Bookmarks);
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        public void StartSlidShow()
        {
            // インターバルを設定
            _SlideShowTimer.Interval = new TimeSpan(0, 0, 5);
            // タイマメソッドを設定
            _SlideShowTimer.Tick += new EventHandler(SlideShowChange);
            // タイマを開始
            _SlideShowTimer.Start();
        }

        public void StopSlidShow()
        {
            // タイマを開始
            _SlideShowTimer.Stop();
        }

        // タイマメソッド
        private void SlideShowChange(object sender, EventArgs ev)
        {
            try
            {
                if (this.Bookmarks == null)
                    return;

                if (this.Bookmarks.SelectedItem == null)
                {
                    this.Bookmarks.SelectedItem = this.Bookmarks.Items.ElementAt(0);
                    WebMove(this.Bookmarks.SelectedItem.URI);
                    return;
                }

                int index = this.Bookmarks.Items.IndexOf(this.Bookmarks.SelectedItem);

                if (index < 0)
                {
                }
                else if (index < this.Bookmarks.Items.Count - 1)
                {
                    this.Bookmarks.SelectedItem = this.Bookmarks.Items.ElementAt(index + 1);
                }
                else
                {
                    this.Bookmarks.SelectedItem = this.Bookmarks.Items.ElementAt(0);
                }
                WebMove(this.Bookmarks.SelectedItem.URI);
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
    }
}
