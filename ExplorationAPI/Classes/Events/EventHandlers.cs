namespace Events
{    
    public class EventStatus
    {
        #region Dictionaries and Properties
        // Very important to be accurate, if this is true, the company must respond to the current event.
        public bool InEvent { get; set; } = false;
        public string CurrentActionID { get; set; } = string.Empty;
        public string ResultDescription = string.Empty;
        public Action<PlayerCompany> NextStage = null;
        public List<EventOption> Options { get; set; } = new();
        public bool Complete { get; set; } = false;
        public int PlayerChoice { get; set; } = 0;

        // This need to be instantiated with an empty object for the EventOperators to work.
        public void SetEvent(string actionID)
        {
            InEvent = true;
            CurrentActionID = actionID;
        }
        public void EndEvent()
        {
            InEvent = false;
            CurrentActionID = string.Empty;
            NextStage = null;
            Options = new();
            Complete = false;
        }
        public void FormatResult()
        {
            int count = 0;
            foreach (var option in Options)
            {
                ResultDescription += "\n" + count.ToString() + ". " + option.Text;
                count++;
            }
        }
    }
        #endregion
        
    
    //public class EventResult
    //{
    //    public string ResultDescription = string.Empty;
    //    public Func<PlayerCompany, EventResult> NextStage = null;
    //    public List<EventOption> Options { get; set; } = new();
    //    public bool Complete { get; set; } = false;
    //    public int PlayerChoice { get; set; } = 0;
    //}

    public class EventOption
    {
        public Guid AdvisorId { get; set; } = Guid.Empty;
        public string AdvisorName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
