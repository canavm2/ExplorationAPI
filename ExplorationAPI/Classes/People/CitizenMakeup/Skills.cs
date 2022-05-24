using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FileTools;

namespace People
{
    public enum Skill
    {
            Academia,
            Athletics,
            AnimalHandling,
            Blacksmithing,
            Carpentry,
            Cooking,
            Diplomacy,
            Drill,
            Engineering,
            FirstAid,
            History,
            Hunting,
            Law,
            Leadership,
            Leatherworking,
            Martial,
            Medical,
            Metalworking,
            Pathfinding,
            Persuation,
            Politics,
            Prospecting,
            Refining,
            Quartermastery,
            Skullduggery,
            Stealth,
            Survival,
            Tactics,
            Tinker
    }
    public class SkillBlock
    {
        public SkillBlock(int unmod, Stat pstat, Stat sstat)
        {
            Unmod = unmod;
            pStat = pstat;
            sStat = sstat;
            Known = false;
            Random random = new Random();
            Max = random.Next(400, 600);
            if (Full > 200) Known = true;
        }

        [JsonConstructor]
        public SkillBlock(int unmod, Stat pstat, Stat sstat, bool known, int max)
        {
            Unmod = unmod;
            pStat = pstat;
            sStat = sstat;
            Known = known;
            Max = max;
        }

        public int Full {
            get {return _unmod; }
        }

        private int _unmod;
        public int Unmod{
            get { return _unmod; }
            internal set { _unmod = value; }
        }

        //private int _statAdjustment;
        //public int StatAdjustment{
        //    get { return _statAdjustment; }
        //    internal set { _statAdjustment = value;
        //        _full = _unmod + _statAdjustment;
        //    }
        //}

        private int _max;
        public int Max {
            get { return _max; }
            internal set { _max = value; }
        }

        public Stat pStat { get; }
        public Stat sStat { get; }
        public bool Known { get; set; }
    }

}
