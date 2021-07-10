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
using System.Windows.Controls;
using System.Windows.Threading;

namespace Destinationboard.ViewModels
{
    public class WebViewVM : ViewModelBase
    {
        public WebViewVM()
        {
            _SlideShowTimer = new DispatcherTimer();

            // インターバルを設定
            _SlideShowTimer.Interval = new TimeSpan(0, 0, 1);

            _SlideShowTimer.Tick += SlideShowChange;
        }

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

        private string _WebViewDir = "EBWebView";

        #region 初期化処理(WebView2の配布)
        /// <summary>
        /// 初期化処理(WebView2の配布)
        /// </summary>
        private async void InitializeAsync()
        {
            var browserExecutableFolder = Path.Combine(Utilities.GetApplicationFolder(), _WebViewDir);

            // カレントディレクトリの作成
            Utilities.CreateDirectory(browserExecutableFolder);

            // 環境の作成
            var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, browserExecutableFolder);

            // 固定バージョンのブラウザを配布
            await this.WebView2Obj.EnsureCoreWebView2Async(webView2Environment);
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

                    InitializeAsync();
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

        string _LastURI = string.Empty;
        #region 画面遷移処理
        /// <summary>
        /// 画面遷移処理
        /// </summary>
        /// <param name="uri">URI</param>
        private void WebMove(string uri)
        {
            try
            {
                if (!this._LastURI.Equals(uri))
                {
                    _LastURI = uri;
                    this.WebView2Obj.CoreWebView2.Navigate(Destinationboard.Common.Utilities.Utilities.ConvertURI(uri));
                }
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
                if (this.Bookmarks != null && this.Bookmarks.SelectedItem != null)
                {
                    this.WebView2Obj.CoreWebView2.Navigate(Destinationboard.Common.Utilities.Utilities.ConvertURI(this.Bookmarks.SelectedItem.URI));
                }
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
                this.Bookmarks.Add(new BookmarkM() { URI = this.URI, Name = this.URI });

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


        // DataGridの手動コミット
        private bool isManualEditCommit;

        #region ブックマークのセルが変更された場合の処理
        /// <summary>
        /// ブックマークのセルが変更された場合の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        public void BookMarkCellEnding(object sender, DataGridCellEditEndingEventArgs ev)
        {
            try
            {
                if (!isManualEditCommit)
                {
                    isManualEditCommit = true;
                    DataGrid grid = (DataGrid)sender;
                    grid.CommitEdit(DataGridEditingUnit.Row, true);
                    SaveBookMark();
                    isManualEditCommit = false;
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region お気に入り情報の保存
        /// <summary>
        /// お気に入り情報の保存
        /// </summary>
        public void SaveBookMark()
        {
            try
            {
                // Configフォルダのパス取得
                string conf_dir = Path.Combine(Utilities.GetApplicationFolder(), _BookmarkDir);
                string map_bk_path = Path.Combine(conf_dir, _BookmarkName);


                var tmp = this.Bookmarks.SelectedItem;
                this.Bookmarks.SelectedItem = null;

                XMLUtil.Seialize<ModelList<BookmarkM>>(map_bk_path, this.Bookmarks);

                this.Bookmarks.SelectedItem = tmp;
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region タイマー切り替え[TimerSec]プロパティ
        /// <summary>
        /// タイマー切り替え[TimerSec]プロパティ用変数
        /// </summary>
        int _TimerSec = 30;
        /// <summary>
        /// タイマー切り替え[TimerSec]プロパティ
        /// </summary>
        public int TimerSec
        {
            get
            {
                return _TimerSec;
            }
            set
            {
                if (value <= 0)
                {
                    value = 1;
                }

                if (!_TimerSec.Equals(value))
                {
                    _TimerSec = value;
                    NotifyPropertyChanged("TimerSec");
                }
            }
        }
        #endregion

        private int _TimerMax = 30;

        #region スライドショーのスタート
        /// <summary>
        /// スライドショーのスタート
        /// </summary>
        public void StartSlidShow()
        {
            try
            {
                _TimerMax = this.TimerSec;

                if (this.Bookmarks != null && this.Bookmarks.Items.Count > 0)
                {
                    this.Bookmarks.SelectedItem = this.Bookmarks.Items.ElementAt(0);
                }

                // タイマを開始
                _SlideShowTimer.Start();
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region スライドショーのストップ
        /// <summary>
        /// スライドショーのストップ
        /// </summary>
        public void StopSlidShow()
        {
            try
            {
                // タイマを開始
                _SlideShowTimer.Stop();

                // 最大値に戻す
                this.TimerSec = this._TimerMax;

            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region タイマー処理
        /// <summary>
        /// タイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void SlideShowChange(object sender, EventArgs ev)
        {
            try
            {
                // 0秒になっていない場合
                if (this.TimerSec > 1)
                {
                    this.TimerSec--;    // カウントダウン
                    return;
                }
                else
                {
                    // 最大値に戻す
                    this.TimerSec = this._TimerMax;
                }

                // ブックマークリストがあるかどうかをチェック
                if (this.Bookmarks == null)
                    return;

                // 選択されているものがあるかどうかをチェックする
                if (this.Bookmarks.SelectedItem == null)
                {
                    this.Bookmarks.SelectedItem = this.Bookmarks.Items.ElementAt(0);
                    WebMove(this.Bookmarks.SelectedItem.URI);
                    return;
                }

                // 選択位置の取り出し
                int index = this.Bookmarks.Items.IndexOf(this.Bookmarks.SelectedItem);

                if (index < 0)
                {
                    ;   // スライド中に突然消された場合とか？
                }
                else if (index < this.Bookmarks.Items.Count - 1)
                {
                    // 次のスライドへ移動
                    this.Bookmarks.SelectedItem = this.Bookmarks.Items.ElementAt(index + 1);
                }
                else
                {
                    // 最後まで行ったので先頭に戻る
                    this.Bookmarks.SelectedItem = this.Bookmarks.Items.ElementAt(0);
                }

                // 画面遷移
                WebMove(this.Bookmarks.SelectedItem.URI);
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion
    }
}
