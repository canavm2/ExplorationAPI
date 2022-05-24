using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People
{
    public partial class Citizen
    {
        #region Methods
        public int GetStat(Stat stat)
        {
            return PrimaryStats[stat].Full;
        }

        public int GetDerivedStat(DerivedStat stat)
        {
            return DerivedStats[stat].Full;
        }

        public void SetStatDev(Stat stat, int value)
        {
            PrimaryStats[stat].Unmod = value;
        }
        public bool TestAdvanceStat(Stat stat, int modify = 0)
        {
            bool test = false;
            StatBlock statBlock = PrimaryStats[stat];
            Random random = new Random();
            // Rolls against the skill with modifier if applicable
            if (random.Next(0, 1000) < (GetStat(stat) + modify)) test = true;
            if (random.Next(0, 1000) < (statBlock.Max - statBlock.Unmod))
                statBlock.Unmod += 1; // TODO log results
            return test;
        }

        #endregion
    }
}
