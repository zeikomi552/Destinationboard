using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Windows.Xps.Packaging;

namespace Destinationboard.Common.Utilities
{
    public static class IntExtensions
    {
        /// <summary>
        /// 数値をExcelのカラム文字へ変換します
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string ToColumnName(this int source)
        {
            if (source < 1) return string.Empty;
            return ToColumnName((source - 1) / 26) + (char)('A' + ((source - 1) % 26));
        }
    }

    public class Utilities
    {
        #region ディレクトリを再帰的に作成する
        /// <summary>
        /// ディレクトリを再帰的に作成する
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string dir_path)
        {
            if (!Directory.Exists(dir_path))
            {
                string parent = Directory.GetParent(dir_path).FullName;
                CreateDirectory(parent);
                Directory.CreateDirectory(dir_path);
            }
        }
        #endregion

        #region ファイルのカレントディレクトリを作成する
        /// <summary>
        /// ファイルのカレントディレクトリを作成する
        /// </summary>
        /// <param name="file_path">ファイルパス</param>
        public static void CreateCurrentDirectory(string file_path)
        {
            string parent = Directory.GetParent(file_path).FullName;
            if (!Directory.Exists(parent))
            {
                CreateDirectory(parent);
            }
        }
        #endregion

        #region アプリケーションフォルダの取得
        /// <summary>
        /// アプリケーションフォルダの取得
        /// </summary>
        /// <returns>アプリケーションフォルダパス</returns>
        public static string GetApplicationFolder()
        {
            var fv = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fv.CompanyName, fv.ProductName);
        }
        #endregion

        public static string GetTemporaryPath()
        {

            return Path.Combine(Utilities.GetApplicationFolder(), "temporary");

        }

        public static string GetTemporaryPath(string file_name)
        {

            return Path.Combine(GetTemporaryPath(), file_name);

        }


        /// <summary>
        /// パワーポイントをXPSファイルに変換する
        /// </summary>
        /// <param name="pptFilename">pptxファイルパス</param>
        /// <param name="xpsFilename">xpsファイルパス</param>
        /// <returns></returns>
        public static XpsDocument ConvertPowerPointToXps(string pptFilename, string xpsFilename)
        {
            var pptApp = new Microsoft.Office.Interop.PowerPoint.Application();

            var presentation = pptApp.Presentations.Open(pptFilename, MsoTriState.msoTrue, MsoTriState.msoFalse,
            MsoTriState.msoFalse);

            try
            {
                presentation.ExportAsFixedFormat(xpsFilename, PpFixedFormatType.ppFixedFormatTypeXPS);
            }
            catch
            {
                throw;
            }
            finally
            {
                presentation.Close();
                pptApp.Quit();
            }

            return new XpsDocument(xpsFilename, FileAccess.Read);
        }

        #region 最上位のWindowの取得処理
        /// <summary>
        /// 最上位のWindowの取得処理
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static DependencyObject GetWindow<T>(object sender)
        {
            DependencyObject depobj = (DependencyObject)sender;
            while (true)
            {
                depobj = VisualTreeHelper.GetParent(depobj);

                if (depobj is T)
                {
                    return depobj;
                }
                else if (depobj == null)
                {
                    return null;
                }
            }
        }
        #endregion

        const string DefaultURI = "https://www.google.com/";

        public static string ConvertURI(string search_text)
        {
            string uri = DefaultURI;

            if (string.IsNullOrEmpty(search_text))
            {
                // 空白なのでGoogleのトップ画面へ
            }
            // "http://" または "https://"が含まれているのでそのまま使用
            else if ((search_text.Length >= 7 && search_text.Substring(0, 7).ToLower().Equals("http://"))
                || (search_text.Length >= 8 && search_text.Substring(0, 8).ToLower().Equals("https://")))
            {
                uri = search_text; // そのまま使用
            }
            else
            {
                // URLではないのでGoogle検索を実行
                uri = string.Format(DefaultURI + "search?q={0}", search_text);
            }
            return uri;
        }
    }
}
