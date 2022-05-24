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
            TimePoints = Convert.ToInt32(TimeSpan.FromDays(2).TotalSeconds);
            LastAction = DateTime.Now;
        }

        [JsonConstructor]
        public TimeBlock(int timepoints, DateTime lastAction)
        {
            TimePoints = timepoints;
            LastAction = lastAction;

        }

        private int _timePoints;
        public int TimePoints {
            get {
                TimeSpan change = DateTime.UtcNow - LastAction;
                int tpgain = Convert.ToInt32(change.TotalSeconds);
                _timePoints += tpgain;
                if (_timePoints > 345600) _timePoints = 345600;
                return _timePoints; }
            internal set { _timePoints = value; } }
        public DateTime CurrentTime { get { return DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(_timePoints)); } }
        public DateTime LastAction;

        #region Methods
        public bool SpendTimePoints(int timePoints)
        {
            if (timePoints < this.TimePoints)
            {
                _timePoints -= timePoints;
                LastAction = DateTime.UtcNow;
                return true;
            }
            else return false;
        }
        #endregion
    }

}
