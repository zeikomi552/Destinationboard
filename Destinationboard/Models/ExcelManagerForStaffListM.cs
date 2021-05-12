using ClosedXML.Excel;
using Destinationboard.Common.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        const string Column7 = "サンプルQRコード";
        static string EndColumn = Column7;

        const string SheetName = "従業員リスト";

        /// <summary>
        /// カラム名定義
        /// </summary>
        private static string[] _ColumnName = new string[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 };

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
                    worksheet.Row(row).Height = 70;
                    worksheet.Cell(row, ColumnNames.IndexOf(Column1) + 1).Value = row - 1;
                    worksheet.Cell(row, ColumnNames.IndexOf(Column2) + 1).SetValue(item.StaffName)
                        .Style.NumberFormat.Format = "@";
                    worksheet.Cell(row, ColumnNames.IndexOf(Column3) + 1).SetValue(item.StaffID)
                        .Style.NumberFormat.Format = "@";
                    worksheet.Cell(row, ColumnNames.IndexOf(Column4) + 1).SetValue(item.QRCode)
                        .Style.NumberFormat.Format = "@";
                    worksheet.Cell(row, ColumnNames.IndexOf(Column5) + 1).SetValue(item.FelicaID)
                        .Style.NumberFormat.Format = "@";
                    worksheet.Cell(row, ColumnNames.IndexOf(Column6) + 1).Value = item.Display;

                    DotNetBarcode makeQR = new DotNetBarcode();
                    makeQR.Type = DotNetBarcode.Types.QRCode;

                    makeQR.QRSetECCRate = (DotNetBarcode.QRECCRates)DotNetBarcode.QRECCRates.Medium15Percent;
                    makeQR.QRQuitZone = 2;
                    makeQR.QRSetVersion = (DotNetBarcode.QRVersions)DotNetBarcode.QRVersions.Ver02;

                    //描画先とするImageオブジェクトを作成する
                    Bitmap canvas = new Bitmap(300, 300);
                    //ImageオブジェクトのGraphicsオブジェクトを作成する
                    using (var g = Graphics.FromImage(canvas))
                    {
                        makeQR.WriteBar(item.SampleQRCode, 2, 2, 300, 300, g);

                        MemoryStream memoryStream = new MemoryStream();
                        canvas.Save(memoryStream, ImageFormat.Png);

                        using (var ms = new MemoryStream())
                        {

                            canvas.Save(ms, ImageFormat.Bmp);
                            var image = worksheet.AddPicture(ms);
                            image.MoveTo(worksheet.Cell(row, ColumnNames.IndexOf(Column7) + 1)).Scale(0.3);
                        }
                    }

                    row++;
                }

                // 表全体をまとめて調整する
                worksheet.ColumnsUsed().AdjustToContents();

                // サンプルQRの部分は固定幅で調整する
                worksheet.Column(ColumnNames.IndexOf(Column7) + 1).Width = 20;

                string end_col = (ColumnNames.IndexOf(EndColumn) + 1).ToColumnName();
                worksheet.Range(string.Format("A1:{0}{1}", end_col, row-1)).Style
                    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetRightBorder(XLBorderStyleValues.Thin);

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
