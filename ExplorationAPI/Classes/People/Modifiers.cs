using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People
{
    public class Modifier
    {
        #region Constructors
        public Modifier(string name, string source, string type, string modifiedvalue, int value, string description, bool temporary=false, int duration=0)
        {
            List<string> possibletypes = new List<string>() {"skill", "stat", "attribute"};
            if (!possibletypes.Contains(type)) throw new Exception($"Modifier type not found: {type}.");
            Name = name;
            Description = description;
            Source = source;
            Type = type;
            ModifiedValue = modifiedvalue;
            Value = value;
            Temporary = temporary;
            Duration = duration;
            //TODO change exception
            if (duration < 0) throw new Exception($"Negative Duration: {duration}");
        }
        #endregion

        #region Properties
        public readonly string Name;
        public readonly string Description;
        public readonly string Source;
        public readonly string Type;
        public readonly string ModifiedValue;
        public readonly int Value;
        public readonly bool Temporary;
        public readonly int Duration;
        #endregion

        #region Methods
        public string Summary()
        {
            string returnSummary = $"Citizen Stat Modifier: {Name}\n" +
                $"{ModifiedValue}: {Value}\n" +
                $"Description: {Description}\n";
            if (Temporary)
                returnSummary = returnSummary + 
                    $"Duration: {Duration}\n";
            return returnSummary;
        }
        #endregion
    }
}
