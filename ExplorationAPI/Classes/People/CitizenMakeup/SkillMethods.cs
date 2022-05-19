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
        public void CalculateSkills()
        {
            foreach (var skill in Skills.Values)
            {
                skill.Update(PrimaryStats);
            }
        }

        #endregion
    }
}
