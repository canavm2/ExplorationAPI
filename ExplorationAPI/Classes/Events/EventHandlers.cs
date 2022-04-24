﻿namespace Events
{    
    public class Status
    {
        //TODO Working HERE
        #region Dictionaries and Properties
        bool InEvent { get; set; } = false;
        string CurrentActionID { get; set; } = string.Empty;
        public void SetEvent(string actionID)
        {
            InEvent = true;
            CurrentActionID = actionID;
        }
        public void EndEvent()
        {
            InEvent = false;
            CurrentActionID = string.Empty;
        }
        #endregion
        
    }
    public class EventResult
    {
        public string ResultDescription = string.Empty;
        public List<EventOption> Options { get; set; } = new();
        public bool Complete { get; set; } = false;
    }

    public class EventOption
    {
        public Guid AdvisorId { get; set; } = Guid.Empty;
        public string AdvisorName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
