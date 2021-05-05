using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using System;
using System.Collections.Generic;
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
                return @"Common\Themes\map\map-layout.png";
            }
        }
        #endregion

        #region 行動予定保存用変数
        /// <summary>
        /// 行動予定保存用
        /// </summary>
        ActionPlanCollectionM _Backup = new ActionPlanCollectionM();
        #endregion

        #region ActionPlanのセット処理
        /// <summary>
        /// ActionPlanのセット処理
        /// </summary>
        /// <param name="action_plans">行動予定リスト</param>
        public void SetActionPlans(ActionPlanCollectionM action_plans)
        {
            // バックアップとの比較
            foreach (var ap in action_plans.Items)
            {
                var bk = (from x in this._Backup.Items
                 where x.StaffID.Equals(ap.StaffID)
                 select x).FirstOrDefault();

                if (bk != null)
                {
                    // 位置情報を反映
                    ap.MapPos = new Point(bk.MapPos.X, bk.MapPos.Y);
                }
            }

            this.ActionPlans = action_plans;

            ActionPlanCollectionM tmp = new ActionPlanCollectionM();
            foreach (var ap in this.ActionPlans.Items)
            {
                ActionPlanM cp = new ActionPlanM();
                cp.Copy(ap);    // 値をコピー
                tmp.Add(cp);    // 要素の追加
            }

            // バックアップ
            _Backup = tmp;
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
                var bmpImg = new BitmapImage();
                bmpImg.BeginInit();
                bmpImg.CacheOption = BitmapCacheOption.OnLoad;
                bmpImg.CreateOptions = BitmapCreateOptions.None;
                bmpImg.UriSource = new Uri(System.AppDomain.CurrentDomain.BaseDirectory + ImagePath);
                bmpImg.EndInit();
                bmpImg.Freeze();
                return bmpImg;
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
                    var bk = (from elem in this._Backup.Items
                              where elem.StaffID.Equals(vm.StaffID)
                              select elem).FirstOrDefault();

                    // 位置情報のセット
                    bk.MapPos = new Point(x, y);

                    var ap = (from elem in this.ActionPlans.Items
                              where elem.StaffID.Equals(vm.StaffID)
                              select elem).FirstOrDefault();

                    ap.MapPos = new Point(x, y);
                    ap.X = x;
                }
            }
        }
        #endregion
    }
}
