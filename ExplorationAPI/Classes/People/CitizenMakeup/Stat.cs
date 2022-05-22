using System.Text.Json.Serialization;

namespace People
{
    public enum Stat
    {
        STR,
        AGI,
        CON,
        INT,
        WIS,
        PER,
        CHA,
        LDR,
        WIL
    }
    public enum DerivedStat
    {
        PHYS,
        MNTL,
        SOCL
    }
    public class StatBlock
    {
        #region Constructors
        public StatBlock(int unmod, int racialModifier)
        {
            Random random = new Random();
            Unmod = unmod;
            Max = random.Next(250,350);
            RacialModifier = racialModifier;
            Known = false;
        }

        [JsonConstructor]
        public StatBlock(int unmod, int full, int max, int racialModifier, Boolean known)
        {
            Unmod = unmod;
            Full = full;
            Max = max;
            RacialModifier = racialModifier;
            Known = known;
        }
        #endregion

        public int Max { get; set; }
        public bool Known { get; set; }


        private int _unmod;
        public int Unmod {
            get { return _unmod; }
            set {_unmod = value;
                _full = _unmod + _racialModifier;
            }
        }
        private int _full;
        public int Full {
            get { return _full; }
            private set { _full = value;
                _full = _unmod + _racialModifier;
            }
        }
        private int _racialModifier;
        public int RacialModifier {
            get { return _racialModifier; }
            set {_racialModifier = value;
                _full = _unmod + _racialModifier;
            }
        }
    }

    public class DerivedStatBlock
    {
        #region Constructors
        [JsonConstructor]
        public DerivedStatBlock(int full)
        {
            Full = full;
        }
        #endregion
        public int Full { get; set; }
    }
}
