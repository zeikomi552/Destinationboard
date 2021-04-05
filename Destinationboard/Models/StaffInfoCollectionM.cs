﻿using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class StaffInfoCollectionM : ModelList<StaffInfoM>
    {
        public void Add(StaffMasterReply reply)
        {
            StaffInfoM info = new StaffInfoM(reply);

            this.Items.Add(info);

        }
    }
}
