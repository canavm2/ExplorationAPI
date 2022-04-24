namespace Events
{
    public interface IEventCache
    {
        public Guid id { get; set; }
        public Dictionary<string, List<Event>> EventCategory { get; set; }
    }

    public class EventCache : IEventCache
    {
        public EventCache()
        {
            id = Guid.NewGuid();
            EventCategory = new Dictionary<string, List<Event>>();
        }
        public Guid id { get; set; }
        public Dictionary<string, List<Event>> EventCategory { get; set; }
    }
}
