namespace Events
{
    public static partial class Event
    {

        // Randomly returns up to  <number> of advisors with <skill> greater than <value>.
        // Will select them randomly if there are more than <value>.
        public static List<Citizen> FindAdvisorsSkill(PlayerCompany company, int number, Skill skill, int value)
        {
            List<Citizen> returnAdvisors = new List<Citizen>();
            foreach (Citizen advisor in company.Advisors)
            {
                if (advisor.Skills.ContainsKey(skill))
                {
                    if (advisor.GetSkill(skill) > value)
                    {
                        returnAdvisors.Add(advisor);
                    }
                }
            }
            if (returnAdvisors.Count > number)
            {
                Random random = new Random();
                returnAdvisors = returnAdvisors.OrderBy(item => random.Next()).ToList();
                returnAdvisors = returnAdvisors.GetRange(0, number);

            }
            return returnAdvisors;
        }

    }
}
