using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People
{
    public partial class Citizen
    {
        #region Methods
        public int GetAttribute(Attribute attribute)
        {
            int value = 0;
            if (this.Attributes[attribute]._modifiers.Count > 0)
            {  // Only checks if there are any Modifiers, else returns 0.
                List<string> expired = new();

                // Checks each modifier to see if its expired, if not expired it adds it up; if expired, deletes it.
                foreach (AttributeModifier modifier in this.Attributes[attribute]._modifiers.Values)
                {
                    if (modifier.Expiration > DateTime.UtcNow) expired.Add(modifier.Name);
                    else value += modifier.Value;
                }
                foreach (String name in expired) this.Attributes[attribute].RemoveModifier(name);
            }
            value += this.Attributes[attribute]._unmod;
            return value;
        }
        public bool TestAttribute(Attribute attribute, int modify = 0)
        {
            bool test = false;
            AttributeBlock attributeBlock = Attributes[attribute];
            Random random = new Random();
            // Rolls against the skill with modifier if applicable
            if (random.Next(0, 1000) < (this.GetAttribute(attribute) + modify)) test = true;
            return test;
        }
        #endregion
    }
}
