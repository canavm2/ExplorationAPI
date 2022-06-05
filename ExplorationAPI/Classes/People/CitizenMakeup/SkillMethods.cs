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
        public int GetSkill(Skill skill)
        {
            int value = 0;
            if (this.Skills[skill]._modifiers.Count > 0)
            {  // Only checks if there are any Modifiers, else returns 0.
                List<string> expired = new();

                // Checks each modifier to see if its expired, if not expired it adds it up; if expired, deletes it.
                foreach (SkillModifier modifier in this.Skills[skill]._modifiers.Values)
                {
                    if (modifier.Expiration > DateTime.UtcNow) expired.Add(modifier.Name);
                    else value += modifier.Value;
                }
                foreach (String name in expired) this.Skills[skill].RemoveModifier(name);
            }
            value += this.GetStat(Skills[skill].pStat);
            value += this.GetStat(Skills[skill].sStat);
            value += Skills[skill]._unmod;
            return value;
        }
        public bool TestAdvanceSkill(Skill skill, int modify = 0)
        {
            bool test = false;
            SkillBlock skillBlock = Skills[skill];
            Random random = new Random();
            // Rolls against the skill with modifier if applicable
            if (random.Next(0, 1000) < (this.GetSkill(skill) + modify)) test = true;
            if (random.Next(0, 1000) < (skillBlock._max - skillBlock._unmod))
                skillBlock._unmod += 1; // TODO log result
            return test;
        }
        #endregion
    }
}
