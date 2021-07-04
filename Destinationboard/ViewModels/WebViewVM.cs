using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Destinationboard.Views;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.ViewModels
{
    public class WebViewVM : ViewModelBase
    {
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

        #region 画面の初期化処理
        /// <summary>
        /// 画面の初期化処理
        /// </summary>
        public override void Init()
        {


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
                this.WebView2Obj.CoreWebView2.Navigate(Destinationboard.Common.Utilities.Utilities.ConvertURI(this.URI));
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        public void BookMark()
        {
            try
            {
                this.Bookmarks.Add(new BookmarkM() { URI = this.URI });
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }

        }
    }
}
