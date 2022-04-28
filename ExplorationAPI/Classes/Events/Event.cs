namespace Events
{
    public static class Event
    {
        
        // Each "stage" has a method that is called and passed a company and the previous stage's result.
        // It returns an event result which is passed back through the API to the players, so they can choose an option.
        // This is the first Stage so doesn't have a result parameter.
        public static void TestEventStageOne(PlayerCompany company)
        {
            Random rnd = new Random();
            var status = company.EventStatus;
            status.ResultDescription = "You find berries along the road, they look edible, and by edible I mean, they fit in your mouth.  What do you do?";
            // First Options
            status.Options = new();
            EventOption option = new EventOption();
            option.Text = "Don't let anyone eat the berries.";
            status.Options.Add(option);

            // Remaining Options, generates random advisors who want to eat the berries.
            int RandomAdvisors = rnd.Next(2, 4);
            List<Citizen> citizens = company.GetRandomAdvisors(RandomAdvisors);
            foreach (Citizen citizen in citizens)
            {
                option = new EventOption();
                option.AdvisorId = citizen.id;
                option.AdvisorName = citizen.Name;
                option.Text = citizen.Name + " wants to eat the berries.  I'll let them.";
                status.Options.Add(option);
            }
            status.NextStage = TestEventStageTwo;
        }

        // This stage takes the outgoing result from Stage One, with the players choice and returns either another set of choices or ends the event.
        public static void TestEventStageTwo(PlayerCompany company)
        {
            EventStatus status = company.EventStatus;
            if (status.Options.Count == 0)
            {
                status.ResultDescription = "No one eats the berries.  Who knows what could have been.";

            }
            else
            {
                status.ResultDescription = status.Options[status.PlayerChoice].AdvisorName + " eats the berries.  Commence explosive diarrhea.";
            }
            status.Options = new();
            status.NextStage = null;
        }
    }

    public static class EventOperators
    {
        // This takes a company and applies the next part of their stage.
        // It does all the required alterations to the company's event result so that doesnt need to be done in the events.
        public static void RunStage(PlayerCompany company)
        {
            Action<PlayerCompany> function = company.EventStatus.NextStage;
            company.EventStatus.SetEvent(function.ToString());
            function(company);
            if (company.EventStatus.Options.Count == 0)
            {
                company.EventStatus.EndEvent();
            }
            else
            {
                company.EventStatus.FormatResult();
            }
        }
    }
}
