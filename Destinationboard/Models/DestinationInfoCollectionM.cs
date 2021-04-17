using Destinationboard.Common.Utilities;
using Destinationboard.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class DestinationInfoCollectionM : ModelList<DestinationInfoM>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DestinationInfoCollectionM()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">List</param>
        public DestinationInfoCollectionM(List<DestinationInfoM> items)
        {
            this.Items = new System.Collections.ObjectModel.ObservableCollection<DestinationInfoM>(items);
        }
    }
}
