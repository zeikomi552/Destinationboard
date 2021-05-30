using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;
using System.IO;
using Destinationboard.Views;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using Microsoft.Win32;

namespace Destinationboard.ViewModels
{
    public class PowerPointVM : ViewModelBase
    {
        #region パワーポイントのxpsファイル変換後パス[PowerpointPath]プロパティ
        /// <summary>
        /// パワーポイントのxpsファイル変換後パス[PowerpointPath]プロパティ用変数
        /// </summary>
        string _PowerpointPath = string.Empty;
        /// <summary>
        /// パワーポイントのxpsファイル変換後パス[PowerpointPath]プロパティ
        /// </summary>
        public string PowerpointPath
        {
            get
            {
                return _PowerpointPath;
            }
            set
            {
                if (!_PowerpointPath.Equals(value))
                {
                    _PowerpointPath = value;
                    NotifyPropertyChanged("PowerpointPath");
                }
            }
        }
        #endregion

        PowerPointV _Wnd;
        XpsDocument _XpsDocument;

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
                this._Wnd = Utilities.GetWindow<UserControl>(sender) as PowerPointV;
                if (File.Exists(this.TemporaryPath))
                {
                    if (this._XpsDocument != null)
                    {
                        this._XpsDocument.Close();
                    }

                    this._XpsDocument = new XpsDocument(this.TemporaryPath, FileAccess.Read);

                    var fixedFixedDocumentSequence = this._XpsDocument.GetFixedDocumentSequence();
                    this._Wnd.DocumentviewPowerPoint.Document = fixedFixedDocumentSequence;
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

        #region テンポラリファイルパス
        /// <summary>
        /// テンポラリファイルパス
        /// </summary>
        public string TemporaryPath
        { 
            get
            {
                if (this._Wnd != null)
                {
                    return Utilities.GetTemporaryPath(string.Format("{0}-ppt.xps", this._Wnd.Name));
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region パワーポイントを開く処理
        /// <summary>
        /// パワーポイントを開く処理
        /// </summary>
        public void OpenPowerPoint()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "パワーポイント (*.pptx;*.PPTX)|*.pptx;*.PPTX";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {

                    if (this._XpsDocument != null)
                    {
                        this._XpsDocument.Close();
                    }

                    string file_name = dialog.FileName;
                    this._XpsDocument = Utilities.ConvertPowerPointToXps(file_name, this.TemporaryPath);
                    var fixedFixedDocumentSequence = this._XpsDocument.GetFixedDocumentSequence();
                    this._Wnd.DocumentviewPowerPoint.Document = fixedFixedDocumentSequence;
                }
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
    }
}
