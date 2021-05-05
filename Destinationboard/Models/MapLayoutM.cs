using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Destinationboard.Models
{
    public class MapLayoutM : ModelBase
    {
        #region スタッフID[StaffID]プロパティ
        /// <summary>
        /// スタッフID[StaffID]プロパティ用変数
        /// </summary>
        String _StaffID = string.Empty;
        /// <summary>
        /// スタッフID[StaffID]プロパティ
        /// </summary>
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

        #region マップでの位置情報[MapPos]プロパティ
        /// <summary>
        /// マップでの位置情報[MapPos]プロパティ用変数
        /// </summary>
        Point _MapPos = new Point(0, 0);
        /// <summary>
        /// マップでの位置情報[MapPos]プロパティ
        /// </summary>
        public Point MapPos
        {
            get
            {
                return _MapPos;
            }
            set
            {
                if (!_MapPos.Equals(value))
                {
                    _MapPos = value;
                    NotifyPropertyChanged("MapPos");
                }
            }
        }
        #endregion
        
    }
}
