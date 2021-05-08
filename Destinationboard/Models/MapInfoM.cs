using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class MapInfoM : ModelBase
    {
		#region 配置のコントロールサイズ[PositionControlSize]プロパティ
		/// <summary>
		/// 配置のコントロールサイズ[PositionControlSize]プロパティ用変数
		/// </summary>
		double _PositionControlSize = 50.0;
		/// <summary>
		/// 配置のコントロールサイズ[PositionControlSize]プロパティ
		/// </summary>
		public double PositionControlSize
		{
			get
			{
				return _PositionControlSize;
			}
			set
			{
				if (!_PositionControlSize.Equals(value))
				{
					_PositionControlSize = value;
					NotifyPropertyChanged("PositionControlSize");
				}
			}
		}
		#endregion
		#region マップの位置情報[MapPosition]プロパティ
		/// <summary>
		/// マップの位置情報[MapPosition]プロパティ用変数
		/// </summary>
		ModelList<MapLayoutM> _MapPosition = new ModelList<MapLayoutM>();
		/// <summary>
		/// マップの位置情報[MapPosition]プロパティ
		/// </summary>
		public ModelList<MapLayoutM> MapPosition
		{
			get
			{
				return _MapPosition;
			}
			set
			{
				if (_MapPosition == null || !_MapPosition.Equals(value))
				{
					_MapPosition = value;
					NotifyPropertyChanged("MapPosition");
				}
			}
		}
		#endregion


	}
}
