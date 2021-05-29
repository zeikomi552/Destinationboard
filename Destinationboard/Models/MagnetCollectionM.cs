using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class MagnetCollectionM : ModelBase
    {
        #region マグネットリスト[Magnets]プロパティ
        /// <summary>
        /// マグネットリスト[Magnets]プロパティ用変数
        /// </summary>
        ModelList<MagnetM> _Magnets = new ModelList<MagnetM>();
        /// <summary>
        /// マグネットリスト[Magnets]プロパティ
        /// </summary>
        public ModelList<MagnetM> Magnets
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



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MagnetCollectionM()
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">List</param>
        public MagnetCollectionM(List<MagnetM> items)
        {
            this.Magnets.Items = new System.Collections.ObjectModel.ObservableCollection<MagnetM>(items);
        }

        public MagnetCollectionM Clone()
        {
            MagnetCollectionM ret = new ();
            foreach (var tmp in this.Magnets.Items)
            {
                ret.Magnets.Add(tmp.ShallowCopy());
            }

            return ret;

        }
    }
}
