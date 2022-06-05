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
            Dictionary<string, StatModifier> modifiers = this.PrimaryStats[stat]._modifiers;
            int totalModifiedValue = 0;
            if (modifiers.Count > 0)
            {  // Only checks if there are any Modifiers, else returns 0.
                List<string> expired = new();

                // Checks each modifier to see if its expired, if not expired it adds it up; if expired, deletes it.
                foreach (StatModifier modifier in modifiers.Values)
                {
                    if (modifier.Expiration > DateTime.UtcNow) expired.Add(modifier.Name);
                    else totalModifiedValue += modifier.Value;
                }
                foreach (String name in expired) this.PrimaryStats[stat].RemoveModifier(name);
            }
            return this.PrimaryStats[stat]._unmod + this.PrimaryStats[stat]._racialModifier + totalModifiedValue;

        }

        public int GetDerivedStat(DerivedStat stat)
        {
            if (stat == DerivedStat.PHYS)
            {
                return this.GetStat(Stat.STR) + this.GetStat(Stat.AGI) + this.GetStat(Stat.CON);
            } else if (stat == DerivedStat.MNTL)
            {
                return this.GetStat(Stat.WIS) + this.GetStat(Stat.INT) + this.GetStat(Stat.PER);
            } else
            {
                return this.GetStat(Stat.CHA) + this.GetStat(Stat.LDR) + this.GetStat(Stat.WIL);
            } 
        }

        public void SetStatDev(Stat stat, int value)
        {
            PrimaryStats[stat]._unmod = value;
        }
        public bool TestAdvanceStat(Stat stat, int modify = 0)
        {
            bool test = false;
            StatBlock statBlock = PrimaryStats[stat];
            Random random = new Random();
            // Rolls against the skill with modifier if applicable
            if (random.Next(0, 1000) < (GetStat(stat) + modify)) test = true;
            if (random.Next(0, 1000) < (statBlock._max - statBlock._unmod))
                statBlock._unmod += 1; // TODO log results
            return test;
        }

        #endregion
    }
}
