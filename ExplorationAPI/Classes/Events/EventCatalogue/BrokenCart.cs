namespace Events
{
    public static partial class Event
    {
        public static void BrokenCartOne(PlayerCompany company)
        {
            Random rnd = new Random();
            var status = company.EventStatus;
            status.ResultDescription = "You come along a man with a broken cart.  What do you do?";
            // First Options
            status.Options = new();
            EventOption option = new EventOption();
            option.Text = "Ignore him, keep moving.";
            option.Type = "default";
            status.Options.Add(option);


            List<Citizen> advisors = FindAdvisorsSkill(company, 2, "Carpentry", 20);
            if (advisors.Count > 0) foreach (Citizen advisor in advisors)
                {
                    option = new EventOption();
                    option.AdvisorId = advisor.id;
                    option.AdvisorName = advisor.Name;
                    option.Text = advisor.Name + " thinks they can fix it.  I let them.";
                    option.Type = "fix";
                    status.Options.Add(option);
                }

            advisors = FindAdvisorsSkill(company, 1, "Tinker", 30);
            if (advisors.Count > 0) foreach (Citizen advisor in advisors)
                {
                    option = new EventOption();
                    option.AdvisorId = advisor.id;
                    option.AdvisorName = advisor.Name;
                    option.Text = advisor.Name + " thinks they can not only fix it, but make it better.  I let them.";
                    option.Type = "tinker";
                    status.Options.Add(option);
                }
            status.NextStage = BrokenCartTwo;
        }

        public static void BrokenCartTwo(PlayerCompany company)
        {
            Random random = new Random();
            EventStatus status = company.EventStatus;
            if (status.PlayerChoice == 0)
            {
                status.ResultDescription = "You pass by the cart without a second thought.";

            }
            else if (status.Options[status.PlayerChoice].Type == "fix")
            {
                //TODO make this more interesting
                status.ResultDescription = status.Options[status.PlayerChoice].AdvisorName + " fixes the wagon, congratulations.";
            }
            else if (status.Options[status.PlayerChoice].Type == "tinker")
            {
                //TODO make this more interesting
                status.ResultDescription = "Oh God!  The blood, its everywhere.  Needless to say, " + status.Options[status.PlayerChoice].AdvisorName + " didn't fix the wagon.";
            }
            status.Options = new();
            status.NextStage = null;
        }
    }
}
