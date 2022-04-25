namespace Events
{
    public interface IEventCache
    {
        public Guid id { get; set; }
    }

    public class EventCache : IEventCache
    {
        public EventCache()
        {
            id = Guid.NewGuid();
        }
        public Guid id { get; set; }
    }
}
