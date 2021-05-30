using Destinationboard.Common.Utilities;
using Destinationboard.Models;
using Destinationboard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Destinationboard.ViewModels
{
    public class MagnetVM : ViewModelBase
    {
		#region マグネットリスト[Magnets]プロパティ
		/// <summary>
		/// マグネットリスト[Magnets]プロパティ用変数
		/// </summary>
		MagnetCollectionM _Magnets = new MagnetCollectionM();
		/// <summary>
		/// マグネットリスト[Magnets]プロパティ
		/// </summary>
		public MagnetCollectionM Magnets
		{
			get
			{
				return _Magnets;
			}
			set
			{
				if (_Magnets == null || !_Magnets.Equals(value))
				{
					_Magnets = value;
					NotifyPropertyChanged("Magnets");
				}
			}
		}
        #endregion

        public string WhiteBoardName { get; set; }

        public override void Init()
        {
            try
            {

            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }

        public override void Close()
        {
            
        }

        public void Regist()
        {
            try
            {
                this.DialogResult = true;
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }

        public void Cancel()
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception e)
            {
                _logger.Error("Fatal Error", e);
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
    }
}
