using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Destinationboard.ViewModels
{
    public class MapVM : ViewModelBase
    {
        public MapVM()
        {
        }
        #region マップ情報（バックアップ用)[MapInfo]プロパティ
        /// <summary>
        /// マップ情報（バックアップ用)[MapInfo]プロパティ用変数
        /// </summary>
        MapInfoM _MapInfo = new MapInfoM();
        /// <summary>
        /// マップ情報（バックアップ用)[MapInfo]プロパティ
        /// </summary>
        public MapInfoM MapInfo
        {
            get
            {
                return _MapInfo;
            }
            set
            {
                if (_MapInfo == null || !_MapInfo.Equals(value))
                {
                    _MapInfo = value;
                    NotifyPropertyChanged("MapInfo");
                }
            }
        }
        #endregion


        #region 行動予定一覧[ActionPlans]プロパティ
        /// <summary>
        /// 行動予定一覧[ActionPlans]プロパティ用変数
        /// </summary>
        ActionPlanCollectionM _ActionPlans = new ActionPlanCollectionM();
        /// <summary>
        /// 行動予定一覧[ActionPlans]プロパティ
        /// </summary>
        public ActionPlanCollectionM ActionPlans
        {
            get
            {
                return _ActionPlans;
            }
            set
            {
                if (_ActionPlans == null || !_ActionPlans.Equals(value))
                {
                    _ActionPlans = value;

                    // 行動計画にマップ情報をセット
                    _ActionPlans.SetMapLayout(this.MapInfo.MapPosition);

                    // マップ情報のバックアップ
                    this.MapInfo.MapPosition.Items = new System.Collections.ObjectModel.ObservableCollection<MapLayoutM>(_ActionPlans.ToMapLayoutList());

                    NotifyPropertyChanged("ActionPlans");
                }
            }
        }
        #endregion

        #region マップイメージのパス[ImagePath]プロパティ
        /// <summary>
        /// マップイメージのパス[ImagePath]プロパティ
        /// </summary>
        public string ImagePath
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + @"Common\Themes\map\map-layout";
            }
        }
        #endregion


        #region マップのイメージ[MapImage]プロパティ
        /// <summary>
        /// マップのイメージ[MapImage]プロパティ用変数
        /// </summary>
        BitmapImage _MapImage = new BitmapImage();
        /// <summary>
        /// マップのイメージ[MapImage]プロパティ
        /// </summary>
        public BitmapImage MapImage
        {
            get
            {
                return _MapImage;
            }
            set
            {
                if (_MapImage == null || !_MapImage.Equals(value))
                {
                    _MapImage = value;
                    NotifyPropertyChanged("MapImage");
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
                // マップのロード処理
                LoadMapPosition();
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
                this.SaveMapPostion();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region ドラッグスタート
        /// <summary>
        /// ドラッグスタート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var border = thumb.Template.FindName("Thumb_Border", thumb) as Border;
                if (null != border)
                {
                    border.BorderThickness = new Thickness(1);
                }
            }
        }
        #endregion

        #region ドラッグ完了
        /// <summary>
        /// ドラッグ完了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var border = thumb.Template.FindName("Thumb_Border", thumb) as Border;
                if (null != border)
                {
                    border.BorderThickness = new Thickness(0);
                }
            }
        }
        #endregion

        #region ドラッグ中
        /// <summary>
        /// ドラッグ中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                var vm = thumb.DataContext as ActionPlanM;
                // nullチェック
                if (vm != null)
                {
                    var ap = (from elem in this.ActionPlans.Items
                              where elem.StaffID.Equals(vm.StaffID)
                              select elem).FirstOrDefault();

                    ap.MapPos = new Point(x, y);

                    var bk = (from m in this.MapInfo.MapPosition.Items
                              where m.StaffID.Equals(ap.StaffID)
                              select m).FirstOrDefault();

                    if (bk != null)
                    {
                        bk.MapPos = new Point(x, y);
                    }
                    else
                    {
                        this.MapInfo.MapPosition.Add(new MapLayoutM() { StaffID = ap.StaffID, MapPos = new Point(x, y) });
                    }
                }
            }
        }
        #endregion

        #region マップ情報のロード処理
        /// <summary>
        /// マップ情報のロード処理
        /// </summary>
        private void LoadMapPosition()
        {
            try
            {
                string map_bk_path = @"temporary\_mapinfo_tmporary";

                if (File.Exists(map_bk_path))
                {
                    this.MapInfo = XMLUtil.Deserialize<MapInfoM>(map_bk_path);
                }
                else
                {
                    if (!Directory.Exists(@"temporary"))
                    {
                        Directory.CreateDirectory(@"temporary");
                    }
                    else
                    {
                        SaveMapPostion();
                    }
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
        private void SaveMapPostion()
        {
            try
            {
                XMLUtil.Seialize<MapInfoM>(@"temporary\_mapinfo_tmporary", this.MapInfo);
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region マップ変更処理
        /// <summary>
        /// マップ変更処理
        /// </summary>
        public void MapChange()
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

                    ClearMapPosition(); // 位置情報の初期化

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

        #region 位置情報の初期化処理
        /// <summary>
        /// 位置情報の初期化処理
        /// </summary>
        public void ClearMapPosition()
        {
            this.ActionPlans.ClearMapPosition();

            foreach (var tmp in this.MapInfo.MapPosition.Items)
            {
                tmp.MapPos = new Point(0, 0);
            }

        }
        #endregion
    }
}
