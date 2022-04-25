namespace Events
{    
    public class Status
    {
        #region Dictionaries and Properties
        // Very important to be accurate, if this is true, the company must respond to the current event.
        public bool InEvent { get; set; } = false;
        public string CurrentActionID { get; set; } = string.Empty;

        // This need to be instantiated with an empty object for the EventOperators to work.
        public EventResult EventResult { get; set; } = new EventResult();
        public void SetEvent(string actionID)
        {
            InEvent = true;
            CurrentActionID = actionID;
        }
        public void EndEvent()
        {
            InEvent = false;
            CurrentActionID = string.Empty;
            EventResult = new EventResult();
        }
        #endregion
        
    }
    public class EventResult
    {
        public string ResultDescription = string.Empty;
        public Func<PlayerCompany, EventResult> NextStage = null;
        public List<EventOption> Options { get; set; } = new();
        public bool Complete { get; set; } = false;
        public int PlayerChoice { get; set; } = 0;
    }

    public class EventOption
    {
        public Guid AdvisorId { get; set; } = Guid.Empty;
        public string AdvisorName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
