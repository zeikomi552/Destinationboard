using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models.db
{
	/// <summary>
	/// スタッフマスター
	/// StaffMasterテーブルをベースに作成しています
	/// 作成日：2021/03/28 作成者gohya
	/// </summary>
	[Table("StaffMaster")]
	public class StaffMasterBase : ModelBase
	{
		#region パラメータ
		#region 従業員ID[StaffID]プロパティ
		/// <summary>
		/// 従業員ID[StaffID]プロパティ用変数
		/// </summary>
		String _StaffID = string.Empty;
		/// <summary>
		/// 従業員ID[StaffID]プロパティ
		/// </summary>
		[Key]
		[Column("StaffID")]
		public String StaffID
		{
			get
			{
				return _StaffID;
			}
			set
			{
				if (_StaffID == null || !_StaffID.Equals(value))
				{
					_StaffID = value;
					NotifyPropertyChanged("StaffID");
				}
			}
		}
		#endregion

		#region ソート順[SortOrder]プロパティ
		/// <summary>
		/// ソート順[SortOrder]プロパティ用変数
		/// </summary>
		Int32 _SortOrder = 0;
		/// <summary>
		/// ソート順[SortOrder]プロパティ
		/// </summary>
		[Column("SortOrder")]
		public Int32 SortOrder
		{
			get
			{
				return _SortOrder;
			}
			set
			{
				if (!_SortOrder.Equals(value))
				{
					_SortOrder = value;
					NotifyPropertyChanged("SortOrder");
				}
			}
		}
		#endregion

		#region 従業員名[StaffName]プロパティ
		/// <summary>
		/// 従業員名[StaffName]プロパティ用変数
		/// </summary>
		String _StaffName = string.Empty;
		/// <summary>
		/// 従業員名[StaffName]プロパティ
		/// </summary>
		[Column("StaffName")]
		public String StaffName
		{
			get
			{
				return _StaffName;
			}
			set
			{
				if (_StaffName == null || !_StaffName.Equals(value))
				{
					_StaffName = value;
					NotifyPropertyChanged("StaffName");
				}
			}
		}
		#endregion

		#region 表示(True)/非表示(False)[Display]プロパティ
		/// <summary>
		/// 表示(True)/非表示(False)[Display]プロパティ用変数
		/// </summary>
		bool _Display = false;
		/// <summary>
		/// 表示(True)/非表示(False)[Display]プロパティ
		/// </summary>
		[Column("Display")]
		public bool Display
		{
			get
			{
				return _Display;
			}
			set
			{
				if (!_Display.Equals(value))
				{
					_Display = value;
					NotifyPropertyChanged("Display");
				}
			}
		}
		#endregion

		#region 従業員識別FelicaID[FelicaID]プロパティ
		/// <summary>
		/// 従業員識別FelicaID[FelicaID]プロパティ用変数
		/// </summary>
		String _FelicaID = string.Empty;
		/// <summary>
		/// 従業員識別FelicaID[FelicaID]プロパティ
		/// </summary>
		[Column("FelicaID")]
		public String FelicaID
		{
			get
			{
				return _FelicaID;
			}
			set
			{
				if (_FelicaID == null || !_FelicaID.Equals(value))
				{
					_FelicaID = value;
					NotifyPropertyChanged("FelicaID");
				}
			}
		}
		#endregion

		#region 従業員識別QRコード[QRCode]プロパティ
		/// <summary>
		/// 従業員識別QRコード[QRCode]プロパティ用変数
		/// </summary>
		String _QRCode = string.Empty;
		/// <summary>
		/// 従業員識別QRコード[QRCode]プロパティ
		/// </summary>
		[Column("QRCode")]
		public String QRCode
		{
			get
			{
				return _QRCode;
			}
			set
			{
				if (_QRCode == null || !_QRCode.Equals(value))
				{
					_QRCode = value;
					NotifyPropertyChanged("QRCode");
				}
			}
		}
		#endregion

		#region 作成日時[CreateDate]プロパティ
		/// <summary>
		/// 作成日時[CreateDate]プロパティ用変数
		/// </summary>
		DateTime _CreateDate = DateTime.MinValue;
		/// <summary>
		/// 作成日時[CreateDate]プロパティ
		/// </summary>
		[Column("CreateDate")]
		public DateTime CreateDate
		{
			get
			{
				return _CreateDate;
			}
			set
			{
				if (!_CreateDate.Equals(value))
				{
					_CreateDate = value;
					NotifyPropertyChanged("CreateDate");
				}
			}
		}
		#endregion

		#region 作成者[CreateUser]プロパティ
		/// <summary>
		/// 作成者[CreateUser]プロパティ用変数
		/// </summary>
		String _CreateUser = string.Empty;
		/// <summary>
		/// 作成者[CreateUser]プロパティ
		/// </summary>
		[Column("CreateUser")]
		public String CreateUser
		{
			get
			{
				return _CreateUser;
			}
			set
			{
				if (_CreateUser == null || !_CreateUser.Equals(value))
				{
					_CreateUser = value;
					NotifyPropertyChanged("CreateUser");
				}
			}
		}
		#endregion


		#endregion

		#region 関数
		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public StaffMasterBase()
		{

		}
		#endregion

		#region コピーコンストラクタ
		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="item">コピー内容</param>
		public StaffMasterBase(StaffMasterBase item)
		{
			// 要素のコピー
			Copy(item);
		}
		#endregion

		#region コピー
		/// <summary>
		/// コピー
		/// </summary>
		/// <param name="item">コピー内容</param>
		public void Copy(StaffMasterBase item)
		{
			this.StaffID = item.StaffID;

			this.SortOrder = item.SortOrder;

			this.StaffName = item.StaffName;

			this.Display = item.Display;

			this.FelicaID = item.FelicaID;

			this.QRCode = item.QRCode;

			this.CreateDate = item.CreateDate;

			this.CreateUser = item.CreateUser;


		}
		#endregion

		#endregion


	}



}
