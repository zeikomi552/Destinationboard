using ClosedXML.Excel;
using Destinationboard.Common.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class ExcelManagerForStaffListM
    {
        const string Column1 = "No";
        const string Column2 = "従業員名";
        const string Column3 = "従業員ID";
        const string Column4 = "QRコード";
        const string Column5 = "FelicaID";
        const string Column6 = "表示";
        const string SheetName = "従業員リスト";

        /// <summary>
        /// カラム名定義
        /// </summary>
        private static string[] _ColumnName = new string[] { Column1, Column2, Column3, Column4, Column5, Column6 };

        #region カラム名定義
        /// <summary>
        /// カラム名定義
        /// </summary>
        public static List<string> ColumnNames
        {
            get
            {
                return _ColumnName.ToList<string>();
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="save_data">保存データ</param>
        public static void Save(StaffInfoCollectionM save_data)
        {
            // ダイアログのインスタンスを生成
            var dialog = new SaveFileDialog();

            // ファイルの種類を設定
            dialog.Filter = "Excelファイル (*.xlsx)|*.xlsx";

            // ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {
                Save(dialog.FileName, save_data);
                ShowMessage.ShowNoticeOK("ファイルを保存しました。", "通知");
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="filepath">保存先ファイルパス</param>
        /// <param name="save_data">保存データ</param>
        public static void Save(string filepath, StaffInfoCollectionM save_data)
        {
            // ファイルを指定して開く
            using (var xls = new XLWorkbook())
            {
                var worksheet = xls.Worksheets.Add(SheetName);

                for (int index = 0; index < ColumnNames.Count; index++)
                {
                    worksheet.Cell(1, index + 1).Value = ColumnNames.ElementAt(index);
                }

                int row = 2;
                foreach (var item in save_data.Items)
                {
                    worksheet.Cell(row, ColumnNames.IndexOf(Column1) + 1).Value = row - 1;
                    worksheet.Cell(row, ColumnNames.IndexOf(Column2) + 1).Value = item.StaffName;
                    worksheet.Cell(row, ColumnNames.IndexOf(Column3) + 1).Value = item.StaffID;
                    worksheet.Cell(row, ColumnNames.IndexOf(Column4) + 1).Value = item.QRCode;
                    worksheet.Cell(row, ColumnNames.IndexOf(Column5) + 1).Value = item.FelicaID;
                    worksheet.Cell(row, ColumnNames.IndexOf(Column6) + 1).Value = item.Display;
                    row++;
                }
                // ワークブックを保存する
                xls.SaveAs(filepath);

            }
        }
        #endregion

        #region ロード
        /// <summary>
        /// ロード
        /// </summary>
        /// <returns>読み取りデータ</returns>
        public static StaffInfoCollectionM Load()
        {
            // ダイアログのインスタンスを生成
            var dialog = new OpenFileDialog();

            // ファイルの種類を設定
            dialog.Filter = "Excelファイル (*.xlsx)|*.xlsx";

            // ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {
                return Load(dialog.FileName);
            }
            else
            {
                ShowMessage.ShowNoticeOK("ファイルが選択されませんでした", "通知");
                return null;
            }
        }
        #endregion

        #region 読み込み処理
        /// <summary>
        /// 読み込み処理
        /// </summary>
        /// <param name="filepath">読み込みファイルパス</param>
        /// <returns>読み込みデータ</returns>
        private static StaffInfoCollectionM Load(string filepath)
        {
            // ファイルを指定して開く
            using (var xls = new XLWorkbook(filepath))
            {
                var worksheet = xls.Worksheet(1);
                StaffInfoCollectionM tmp = new StaffInfoCollectionM();
                int row = 2;
                while (!string.IsNullOrWhiteSpace(worksheet.Cell(row, ColumnNames.IndexOf(Column1) + 1).Value.ToString()))
                {
                    StaffInfoM staffinfo = new StaffInfoM();

                    staffinfo.SortOrder = row - 1;
                    staffinfo.StaffName = worksheet.Cell(row, ColumnNames.IndexOf(Column2) + 1).Value.ToString();
                    staffinfo.StaffID = worksheet.Cell(row, ColumnNames.IndexOf(Column3) + 1).Value.ToString();
                    staffinfo.QRCode = worksheet.Cell(row, ColumnNames.IndexOf(Column4) + 1).Value.ToString();
                    staffinfo.FelicaID = worksheet.Cell(row, ColumnNames.IndexOf(Column5) + 1).Value.ToString();
                    staffinfo.Display = worksheet.Cell(row, ColumnNames.IndexOf(Column6) + 1).Value.ToString().ToLower().Equals("true");
                    tmp.Items.Add(staffinfo);
                    row++;
                }

                return tmp;
            }
        }
        #endregion
    }
}
