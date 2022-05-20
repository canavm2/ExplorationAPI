using System.Text.Json.Serialization;

namespace People
{
    public class Stat
    {
        #region Constructors
        public Stat(int unmod, int racialModifier)
        {
            Random random = new Random();
            Unmod = unmod;
            Max = random.Next(250,350);
            RacialModifier = racialModifier;
            Known = false;
        }

        [JsonConstructor]
        public Stat(int unmod, int full, int max, int racialModifier, Boolean known)
        {
            Unmod = unmod;
            Full = full;
            Max = max;
            RacialModifier = racialModifier;
            Known = known;
        }
        #endregion

        public int Max { get; set; }
        public Boolean Known { get; set; }


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
            internal set { _full = value;
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

    public class DerivedStat
    {
        #region Constructors
        [JsonConstructor]
        public DerivedStat(int full)
        {
            Full = full;
        }
        #endregion
        public int Full { get; set; }
    }
}
