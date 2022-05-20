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
            Unmod = unmod;
            pStat = pstat;
            sStat = sstat;
            Known = false;
            if (Full > 200) Known = true;
        }

        [JsonConstructor]
        public Skill(int full, int unmod, string pstat, string sstat, bool known, int statAdjustment)
        {
            Full = full;
            Unmod = unmod;
            pStat = pstat;
            sStat = sstat;
            Known = known;
            StatAdjustment = statAdjustment;
        }


        private int _full;
        public int Full {
            get {return _full; }
            internal set {_full = value;
                _full = _unmod + _statAdjustment;
            }
        }
        private int _unmod;
        public int Unmod{
            get { return _unmod; }
            set {_unmod = value;
                _full = _unmod + _statAdjustment;
            }
        }
        private int _statAdjustment;
        public int StatAdjustment{
            get { return _statAdjustment; }
            internal set { _statAdjustment = value;
                _full = _unmod + _statAdjustment;
            }
        }
        public string pStat { get; }
        public string sStat { get; }
        public bool Known { get; set; }

        public void Update(Dictionary<string, Stat> primaryStats)
        {
            StatAdjustment = (primaryStats[pStat].Full / 10) + (primaryStats[sStat].Full / 5);
        }
    }

}
