using System.Text.Json.Serialization;

namespace People
{
    public enum Attribute
    {
        Health,
        Happiness,
        Motivation,
        Psyche
    }
    public class AttributeBlock
    {
        #region Constructors
        public AttributeBlock()
        {
            Unmod = 100;
            Max = 300;
        }

        [JsonConstructor]
        public AttributeBlock(int unmod, int max)
        {
            Unmod = unmod;
            Max = max;
        }
        #endregion
        public int Full { get { return _unmod; } }
        private int _unmod;
        public int Unmod { get { return _unmod; } internal set { _unmod = value; } }
        private int _max;
        public int Max { get { return _max; } internal set { _max = value; } }
    }
}
