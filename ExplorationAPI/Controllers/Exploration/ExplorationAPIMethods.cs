namespace APIMethods
{
    public static class ExplorationAPIMethods
    {
        public static string Walk(PlayerCompany company)
        {
            if (company.EventStatus.InEvent) return "You are currently in an event, please resolve the current event.\n\n" +
                company.EventStatus.ResultDescription;
            company.EventStatus.NextStage = Event.TestEventStageOne;
            return EventOperators.RunStage(company);
        }


    }
}
