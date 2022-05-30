using System.Text.Json.Serialization;

namespace People
{
    public enum Relationship
    {
        Friendliness,
        Teamwork,
        Connection
    }
    public class RelationshipBlock
    {
        #region Constructors
        public RelationshipBlock(Guid otheradvisor)
        {
            Random random = new Random();
            OtherAdvisor = otheradvisor;
            Known = false;
            Value = random.Next(0,200);
        }
        [JsonConstructor]
        public RelationshipBlock(Guid otheradvisor, bool known, int value)
        {
            OtherAdvisor = otheradvisor;
            Known = known;
            Value = value;
        }
        #endregion


        private Guid OtherAdvisor { get; set; }
        public Boolean Known { get; set; }

        private int _value;
        public int Value { get { return _value; } set { _value = value; } }

    }
}