﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Common.Utilities
{
    public abstract class ViewModelBase : INotifyPropertyChanged
	{
		#region ダイアログ結果[DialogResult]プロパティ
		/// <summary>
		/// ダイアログ結果[DialogResult]プロパティ用変数
		/// </summary>
		bool? _DialogResult = null;
		/// <summary>
		/// ダイアログ結果[DialogResult]プロパティ
		/// </summary>
		public bool? DialogResult
		{
			get
			{
				return _DialogResult;
			}
			set
			{
				if (_DialogResult == null || !_DialogResult.Equals(value))
				{
					_DialogResult = value;
					NotifyPropertyChanged("DialogResult");
				}
			}
		}
		#endregion

		/// <summary>
		/// ロガー
		/// </summary>
		protected static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// 初期化処理
		/// </summary>
		public abstract void Init();

		public abstract void Close();

		#region INotifyPropertyChanged 
		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#endregion
	}
}
