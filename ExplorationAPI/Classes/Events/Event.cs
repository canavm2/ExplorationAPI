namespace Events
{
    public class Event
    {
        internal string Name { get; set; } = string.Empty;
       


    }

    public class TestEvent : Event
    {
        public TestEvent(string name)
        {
            Name = name;
            Value = string.Empty;
        }
        string Value { get; set; }
    }
}
