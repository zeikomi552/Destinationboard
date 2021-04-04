using Destinationboard.Common.Utilities;
using Destinationboard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Init()
        {
            try
            {
            }
            catch (Exception e)
            {
                _logger.Error("Fatal error", e);
            }
        }
        #endregion

        #region 設定画面への画面遷移
        /// <summary>
        /// 設定画面への画面遷移
        /// </summary>
        public void MoveSettingApplicationV()
        {
            try
            {
                SettingApplicationV wnd = new SettingApplicationV();
                SettingApplicationVM vm = wnd.DataContext as SettingApplicationVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                }

            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 従業員設定画面への画面遷移
        /// <summary>
        /// 従業員設定画面への画面遷移
        /// </summary>
        public void MoveSettingStaffV()
        {
            try
            {
                SettingStaffV wnd = new SettingStaffV();
                SettingStaffVM vm = wnd.DataContext as SettingStaffVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 行動設定画面への画面遷移
        /// <summary>
        /// 行動設定画面への画面遷移
        /// </summary>
        public void MoveSettingActionV()
        {
            try
            {
                SettingActionV wnd = new SettingActionV();
                SettingActionVM vm = wnd.DataContext as SettingActionVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                }

            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 行動登録画面への画面遷移
        /// <summary>
        /// 行動登録画面への画面遷移
        /// </summary>
        public void MoveRegistActionV()
        {
            try
            {
                RegistActionV wnd = new RegistActionV();
                RegistActionVM vm = wnd.DataContext as RegistActionVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 開始終了登録画面への画面遷移
        /// <summary>
        /// 開始終了登録画面への画面遷移
        /// </summary>
        public void MoveRegistBeginFinishV()
        {
            try
            {
                RegistBeginFinishV wnd = new RegistBeginFinishV();
                RegistBeginFinishVM vm = wnd.DataContext as RegistBeginFinishVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 時間登録画面への画面遷移
        /// <summary>
        /// 時間登録画面への画面遷移
        /// </summary>
        public void MoveRegistTimeV()
        {
            try
            {
                RegistTimeV wnd = new RegistTimeV();
                RegistTimeVM vm = wnd.DataContext as RegistTimeVM;

                // 画面を開く
                if (wnd.ShowDialog() == true)
                {
                }
            }
            catch (Exception e)
            {
                _logger.Error("fatal error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 閉じる処理
        /// <summary>
        /// 閉じる処理
        /// </summary>
        public override void Close()
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
    }
}
