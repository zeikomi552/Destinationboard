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


        public void SetActionPlans(ActionPlanCollectionM action_plans)
        {
            this.ActionPlans = action_plans;
        }

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

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);
            }
        }
    }
}
