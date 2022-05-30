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
    public class AdvisorBlock
    {
        #region Constructors
        public AdvisorBlock()
        {
            Master = false;
            Advisor = false;
            Vanguard = false;
            Successor = false;
        }

        [JsonConstructor]
        public AdvisorBlock(bool master, bool advisor, bool vanguard, bool successor)
        {
            Master = master;
            Advisor = advisor;
            Vanguard = vanguard;
            Successor = successor;
        }
        #endregion

        #region Dictionaries and Properties
        public bool Master { get; set; }
        
        public bool Successor { get; set; }
        public bool Advisor { get; set; }
        public bool Vanguard { get; set; }
        #endregion

        #region Methods
        #endregion
    }

}
