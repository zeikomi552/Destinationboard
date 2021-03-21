using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class ActualActionM : ModelBase
    {
		#region 氏名[Name]プロパティ
		/// <summary>
		/// 氏名[Name]プロパティ用変数
		/// </summary>
		string _Name = string.Empty;
		/// <summary>
		/// 氏名[Name]プロパティ
		/// </summary>
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if (!_Name.Equals(value))
				{
					_Name = value;
					NotifyPropertyChanged("Name");
				}
			}
		}
		#endregion

		#region 行動[Action]プロパティ
		/// <summary>
		/// 行動[Action]プロパティ用変数
		/// </summary>
		string _Action = string.Empty;
		/// <summary>
		/// 行動[Action]プロパティ
		/// </summary>
		public string Action
		{
			get
			{
				return _Action;
			}
			set
			{
				if (!_Action.Equals(value))
				{
					_Action = value;
					NotifyPropertyChanged("Action");
					NotifyPropertyChanged("ShowActionDestination");
				}
			}
		}
		#endregion

		#region 行先[Destination]プロパティ
		/// <summary>
		/// 行先[Destination]プロパティ用変数
		/// </summary>
		string _Destination = string.Empty;
		/// <summary>
		/// 行先[Destination]プロパティ
		/// </summary>
		public string Destination
		{
			get
			{
				return _Destination;
			}
			set
			{
				if (!_Destination.Equals(value))
				{
					_Destination = value;
					NotifyPropertyChanged("Destination");
					NotifyPropertyChanged("ShowActionDestination");
				}
			}
		}
		#endregion

		#region 行動と行先の見え方
		/// <summary>
		/// 行動と行先の見え方
		/// </summary>
		public string ShowActionDestination
		{
			get
			{
				return this.Action + " " + this.Destination;
			}
		}
        #endregion

        #region 開始日時[FromTime]プロパティ
        /// <summary>
        /// 開始日時[FromTime]プロパティ用変数
        /// </summary>
        DateTime _FromTime = DateTime.MinValue;
		/// <summary>
		/// 開始日時[FromTime]プロパティ
		/// </summary>
		public DateTime FromTime
		{
			get
			{
				return _FromTime;
			}
			set
			{
				if (!_FromTime.Equals(value))
				{
					_FromTime = value;
					NotifyPropertyChanged("FromTime");
				}
			}
		}
		#endregion
		#region 終了日時[ToTime]プロパティ
		/// <summary>
		/// 終了日時[ToTime]プロパティ用変数
		/// </summary>
		DateTime _ToTime = DateTime.MinValue;
		/// <summary>
		/// 終了日時[ToTime]プロパティ
		/// </summary>
		public DateTime ToTime
		{
			get
			{
				return _ToTime;
			}
			set
			{
				if (!_ToTime.Equals(value))
				{
					_ToTime = value;
					NotifyPropertyChanged("ToTime");
				}
			}
		}
		#endregion
		#region メモ[Memo]プロパティ
		/// <summary>
		/// メモ[Memo]プロパティ用変数
		/// </summary>
		string _Memo = string.Empty;
		/// <summary>
		/// メモ[Memo]プロパティ
		/// </summary>
		public string Memo
		{
			get
			{
				return _Memo;
			}
			set
			{
				if (!_Memo.Equals(value))
				{
					_Memo = value;
					NotifyPropertyChanged("Memo");
				}
			}
		}
		#endregion

	}
}
