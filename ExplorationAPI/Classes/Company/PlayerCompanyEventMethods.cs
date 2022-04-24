using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using People;
using FileTools;
using Users;
//using Newtonsoft.Json;
using Relation;

namespace Company
{
    public partial class PlayerCompany
    {
        public List<Citizen> GetRandomAdvisors(int number)
        {
            Random random = new Random();   
            List<Citizen> list = new List<Citizen>();
            Dictionary<string, Citizen> Advisors = this.Advisors;
            while (number > 0)
            {
                int advisor = random.Next(Advisors.Count);
                list.Add(Advisors.ElementAt(advisor).Value);
                Advisors.Remove(Advisors.ElementAt(advisor).Key);
                number--;
            }
            return list;
        }
    }
}
