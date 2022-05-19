using System.Text.Json.Serialization;

namespace People
{
    public class Stat
    {
        #region Constructors
        public Stat(int unmod, int racialModifier)
        {
            Random random = new Random();
            Unmodified = unmod;
            Full = unmod + racialModifier;
            Max = random.Next(250,350);
            RacialModifier = racialModifier;
            Known = false;
        }

        [JsonConstructor]
        public Stat(int unmodified, int full, int max, int racialModifier, Boolean known)
        {
            Unmodified = unmodified;
            Full = full;
            Max = max;
            RacialModifier = racialModifier;
            Known = known;
        }
        #endregion

        public int Unmodified { get; set; }
        public int Full { get; set; }
        public int Max { get; set; }
        public int RacialModifier { get; set; }
        public Boolean Known { get; set; }
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
