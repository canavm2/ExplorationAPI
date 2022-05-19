using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using FileTools;

namespace Company
{
    public class CompanySkill
    {
        public CompanySkill(int unmod)
        {
            Full = unmod;
            Unmodified = unmod;
        }

        [JsonConstructor]
        public CompanySkill(int full, int unmodified)
        {
            Full = full;
            Unmodified = unmodified;
        }
        public int Full { get; set; }
        public int Unmodified { get; set; }
    }

}
