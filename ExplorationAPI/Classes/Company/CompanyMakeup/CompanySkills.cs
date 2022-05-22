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
    public class CompanySkillBlock
    {
        public CompanySkillBlock(int unmod)
        {
            Full = unmod;
            Unmodified = unmod;
        }

        [JsonConstructor]
        public CompanySkillBlock(int full, int unmodified)
        {
            Full = full;
            Unmodified = unmodified;
        }
        public int Full { get; set; }
        public int Unmodified { get; set; }
    }

}
