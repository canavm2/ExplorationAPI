using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using FileTools;

namespace People
{
    public class Skill
    {
        public Skill(int unmod, string pstat, string sstat)
        {
            Full = unmod;
            Unmodified = unmod;
            pStat = pstat;
            sStat = sstat;
            Known = false;
            if (Full > 200) Known = true;
        }

        [JsonConstructor]
        public Skill(int full, int unmodified, string pstat, string sstat, bool known, int statAdjustment)
        {
            Full = full;
            Unmodified = unmodified;
            pStat = pstat;
            sStat = sstat;
            Known = known;
            StatAdjustment = statAdjustment;
        }
        public int Full { get; set; }
        public int Unmodified { get; set; }
        public string pStat { get; }
        public string sStat { get; }
        public bool Known { get; set; }
        public int StatAdjustment { get; set; }

        public void Update(Dictionary<string, Stat> primaryStats)
        {
            StatAdjustment = (primaryStats[pStat].Full / 10) + (primaryStats[sStat].Full / 5);
            Full = Unmodified + StatAdjustment;
        }
    }

}
