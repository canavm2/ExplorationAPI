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
            value += PrimaryStats[Skills[skill].pStat].Full;
            value += PrimaryStats[Skills[skill].sStat].Full;
            value += Skills[skill].Full;
            return value;
        }

        public void SetSkillDev(Skill skill, int value)
        {
            Skills[skill].Unmod = value;
        }
        public bool TestAdvanceSkill(Skill skill, int modify = 0)
        {
            bool test = false;
            SkillBlock skillBlock = Skills[skill];
            Random random = new Random();
            // Rolls against the skill with modifier if applicable
            if (random.Next(0, 1000) < (skillBlock.Full + modify)) test = true;
            if (random.Next(0, 1000) < (skillBlock.Max - skillBlock.Unmod))
                skillBlock.Unmod += 1; // TODO log result
            return test;
        }
        #endregion
    }
}
