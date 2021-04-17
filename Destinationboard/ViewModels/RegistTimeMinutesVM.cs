using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Destinationboard.ViewModels
{
    public class RegistTimeMinutesVM : ViewModelBase
    {
        #region 選択された数値[ClickNumber]プロパティ
        /// <summary>
        /// 選択された数値[ClickNumber]プロパティ用変数
        /// </summary>
        int _ClickNumber = -1;
        /// <summary>
        /// 選択された数値[ClickNumber]プロパティ
        /// </summary>
        public int ClickNumber
        {
            get
            {
                return _ClickNumber;
            }
            set
            {
                if (!_ClickNumber.Equals(value))
                {
                    _ClickNumber = value;
                    NotifyPropertyChanged("ClickNumber");
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
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 分のセット処理
        /// <summary>
        /// 分のセット処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetMinute(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;  // 押されたボタンの取得

                int button_no = -1; // 初期化
                int.TryParse(button.Content.ToString(), out button_no); // ボタンの文字列を取得
                this.ClickNumber = button_no;   // ボタンの番号をセット
                this.DialogResult = true;       // 画面を閉じる
            }
            catch (Exception ex)
            {
                _logger.Error("致命的なエラー", ex);
                ShowMessage.ShowErrorOK(ex.Message, "Error");
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
                this.DialogResult = false;
            }
            catch (Exception e)
            {
                _logger.Error("致命的なエラー", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion
    }
}
