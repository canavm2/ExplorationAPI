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
            foreach (Citizen citizen in this.Advisors.Values)
            {
                list.Add(citizen);
            }
            // Randomizes the list
            list = list.OrderBy(item => random.Next()).ToList();
            list = list.GetRange(0, number);
            return list;
        }
    }
}
