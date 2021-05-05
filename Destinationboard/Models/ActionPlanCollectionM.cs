﻿using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class ActionPlanCollectionM: ModelList<ActionPlanM>
    {
        #region マップの位置情報データに変換する
        /// <summary>
        /// マップの位置情報データに変換する
        /// </summary>
        /// <returns>マップの位置情報データ</returns>
        public List<MapLayoutM> ToMapLayoutList()
        {
            List<MapLayoutM> list = new List<MapLayoutM>();
            foreach (var tmp in this.Items)
            {
                list.Add(tmp.ToMapLayout());
            }
            return list;
        }
        #endregion

        #region マップ情報のセット
        /// <summary>
        /// マップ情報のセット
        /// </summary>
        /// <param name="maplist">マップリスト</param>
        public void SetMapLayout(List<MapLayoutM> maplist)
        {
            foreach (var tmp in this.Items)
            {
                tmp.SetMapLayout(maplist);
            }
        }
        #endregion
    }
}
