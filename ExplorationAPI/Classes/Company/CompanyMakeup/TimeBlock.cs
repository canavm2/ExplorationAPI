using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using FileTools;

namespace Company
{
    public class TimeBlock
    {
        public TimeBlock()
        {
            TimePoints = Convert.ToInt64(TimeSpan.FromDays(2).TotalSeconds);
        }

        [JsonConstructor]
        public TimeBlock(long timepoints)
        {
            TimePoints = timepoints;
        }

        private long _timePoints;
        public long TimePoints { get { return _timePoints; } internal set { _timePoints = value; } }
        private long _time;
        public long Time { get { return _time; } internal set { _time = value; } }

        #region Methods
        public void GainTimePoints(long timePoints)
        {
            long MaxTimePoints = 345600;  // Seconds in 4 days
            if ((_timePoints + timePoints) > MaxTimePoints) _timePoints = MaxTimePoints;
            else _timePoints += timePoints;
        }
        public bool SpendTimePoints(long timePoints)
        {
            if (timePoints < this.TimePoints)
            {
                _timePoints -= timePoints;
                return true;
            }
            else return false;
        }
        #endregion
    }
}
