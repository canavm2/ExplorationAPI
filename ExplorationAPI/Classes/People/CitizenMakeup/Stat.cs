using System.Text.Json.Serialization;

namespace People
{
    public class Stat
    {
        #region Constructors
        public Stat(int unmod)
        {
            Unmodified = unmod;
            Full = unmod;
            Max = 300;
        }

        [JsonConstructor]
        public Stat(int unmodified, int full, int max)
        {
            Unmodified = unmodified;
            Full = full;
            Max = max;  
        }
        #endregion

        public int Unmodified { get; set; }
        public int Full { get; set; }
        public int Max { get; set; }
    }
}
