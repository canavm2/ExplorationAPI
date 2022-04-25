namespace Events
{
    public static class Event
    {
        
        // Each "stage" has a method that is called and passed a company and the previous stage's result.
        // It returns an event result which is passed back through the API to the players, so they can choose an option.
        // This is the first Stage so doesn't have a result parameter.
        public static EventStatus TestEventStageOne(PlayerCompany company)
        {
            Random rnd = new Random();

            EventStatus OutgoingResult = new();
            OutgoingResult.ResultDescription = "You find berries along the road, they look edible, and by edible I mean, they fit in your mouth.  What do you do?";
            // First Options
            EventOption option = new EventOption();
            option.Text = "Don't let anyone eat the berries.";
            OutgoingResult.Options.Add(option);

            // Remaining Options, generates random advisors who want to eat the berries.
            int RandomAdvisors = rnd.Next(2, 4);
            List<Citizen> citizens = company.GetRandomAdvisors(RandomAdvisors);
            foreach (Citizen citizen in citizens)
            {
                option = new EventOption();
                option.AdvisorId = citizen.id;
                option.AdvisorName = citizen.Name;
                option.Text = citizen.Name + " wants to eat the berries.  I'll let them.";
                OutgoingResult.Options.Add(option);
            }
            OutgoingResult.NextStage = TestEventStageTwo;
            return OutgoingResult;
        }

        // This stage takes the outgoing result from Stage One, with the players choice and returns either another set of choices or ends the event.
        public static EventStatus TestEventStageTwo(PlayerCompany company)
        {
            EventStatus IncomingResult = company.EventStatus;
            EventStatus OutgoingResult = new();
            if (IncomingResult.Options.Count == 0)
            {
                OutgoingResult.ResultDescription = "No one eats the berries.  Who knows what could have been.";
            }
            else
            {
                OutgoingResult.ResultDescription = IncomingResult.Options[IncomingResult.PlayerChoice].AdvisorName + " eats the berries.  Commence explosive diarrhea.";
            }
            OutgoingResult.NextStage = null;
            // The Event Result is passed to the company in "RunEvent"
            return OutgoingResult;
        }
    }

    public static class EventOperators
    {
        // This takes a company and applies the next part of their stage.
        // It does all the required alterations to the company's event result so that doesnt need to be done in the events.
        public static EventStatus RunStage(PlayerCompany company)
        {
            Func<PlayerCompany, EventStatus> function = company.EventStatus.NextStage;
            EventStatus result = company.EventStatus;
            company.EventStatus.SetEvent(function.ToString());
            result = function(company);
            company.EventStatus = result;
            if (result.Options.Count == 0)
            {
                company.EventStatus.EndEvent();
            }
            return result;
        }
    }
}
