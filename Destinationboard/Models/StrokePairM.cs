using Destinationboard.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace Destinationboard.Models
{
    public class StrokePairM : ModelBase
    {
        public StrokePairM(StrokeCollection added, StrokeCollection removed)
        {
            this.AddedStroke = added;
            this.RemovedStroke = removed;
        }

        public StrokeCollection AddedStroke { get; set; }
        public StrokeCollection RemovedStroke { get; set; }
    }
}
