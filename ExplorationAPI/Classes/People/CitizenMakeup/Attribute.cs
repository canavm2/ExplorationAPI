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
            _unmod = 100;
            _max = 300;
        }

        [JsonConstructor]
        public AttributeBlock(int _Unmod, int _Max)
        {
            _unmod = _Unmod;
            _max = _Max;
        }
        #endregion
        public int _unmod { get; set; }
        public int _max { get; set; }
    }
}
