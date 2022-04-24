namespace Events
{
    public class Event
    {
        internal string Name { get; set; } = string.Empty;
       
    }

    public class TestEvent : Event
    {

        // Normal Constructor, Builds the event and then the methods can be called by passing the company to it.
        public TestEvent(string name)
        {
            Name = name;
        }


        // Each "stage" has a method that is called and passed a company and the previous stage's result.
        // It returns an event result which is passed back through the API to the players, so they can choose an option.
        // This is the first Stage so doesn't have a result parameter.
        public EventResult StageOne(PlayerCompany company)
        {
            company.Status.SetEvent("TestEvent");
            Random rnd = new Random();

            EventResult OutgoingResult = new();
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
            company.Status.EventResult = OutgoingResult;
            return OutgoingResult;
        }

        // This stage takes the outgoing result from Stage One, with the players choice and returns either another set of choices or ends the event.
        public EventResult StageTwo(PlayerCompany company, EventResult incomingResult, int choice)
        {
            EventResult OutgoingResult = new();

            return OutgoingResult;
        }
    }
}
