namespace APIMethods
{
    public static class ExplorationAPIMethods
    {
        public static string Walk(User user, PlayerCompany company)
        {
            if (!company.TimeBlock.SpendTimePoints(100)) return "You do not have enough Timepoints, you have " + company.TimeBlock.TimePoints.ToString() + " timepoints.";
            if (company.EventStatus.InEvent) return "You are currently in an event, please resolve the current event.\n\n" +
                company.EventStatus.ResultDescription;
            Random random = new Random();
            //TODO make a cache of event and choose one "randomly"
            if (random.Next(2) == 1) company.EventStatus.NextStage = Event.TestEventStageOne;
            else company.EventStatus.NextStage = Event.BrokenCartOne;
            EventOperators.RunStage(company);
            return company.EventStatus.ResultDescription;
        }
        public static string ProgressEvent(PlayerCompany company, int choice)
        {
            company.EventStatus.PlayerChoice = choice;
            if (!company.EventStatus.InEvent) return "You are not in an event, perhaps you should walk down the road.  There might be more berries.";
            if (company.EventStatus.PlayerChoice < 0 || company.EventStatus.PlayerChoice > company.EventStatus.Options.Count-1)
                return "Invalid selection, choose an option from the list.\n\n" + company.EventStatus.ResultDescription;
            EventOperators.RunStage(company);
            return company.EventStatus.ResultDescription;
        }
    }
}
