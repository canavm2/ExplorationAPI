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

        //Updates Citizen's Derived stats based on their PrimaryStats
        public void RefreshDerived()
        {
            DerivedStats["PHYS"].Full = (PrimaryStats["STR"].Full + PrimaryStats["AGI"].Full + PrimaryStats["CON"].Full) / 3;
            DerivedStats["MNTL"].Full = (PrimaryStats["INT"].Full + PrimaryStats["WIS"].Full + PrimaryStats["PER"].Full) / 3;
            DerivedStats["SOCL"].Full = (PrimaryStats["CHA"].Full + PrimaryStats["LDR"].Full + PrimaryStats["WIL"].Full) / 3;
        }


        //Adds a temporary modifier to Modifiers(unless already exists) and then applies the modifier
        //Should not be used with trait modifiers, which are stored in the trait
        public void AddModifier(Modifier modifier)
        {
            if (Modifiers.ContainsKey(modifier.Name))
            {
                Modifiers[modifier.Name] = modifier;
                ApplyModifier(Modifiers[modifier.Name]);
            }
            //TODO Determine what happens if the modifier is a duplicate name
        }

        //Removes a temprorary modifier from Modifiers and unapplies it
        //Should not be used with trait modifiers, which are stored in the trait
        public void RemoveModifier(string name)
        {
            if (Modifiers.ContainsKey(name))
            {
                ApplyModifier(Modifiers[name],"remove");
                Modifiers.Remove(name);
            }
            else throw new Exception($"Modifiers key not found: {name}");
        }

        //Reads a modifier and determines what it should be applied to
        //Can be used to unapply modifiers with "remove" as a parameter
        public void ApplyModifier(Modifier modifier, string action = "add")
        {
            int Value = modifier.Value;
            if (action == "remove") Value = -modifier.Value;
            if (modifier.Type == "skill")
            {
                if (Skills.ContainsKey(modifier.ModifiedValue))
                    Skills[modifier.ModifiedValue].Full += Value;
                else throw new Exception($"Skill Modifier ModifiedValue not found: {modifier.ModifiedValue}");
            }
            else if (modifier.Type == "stat")
            {
                if (PrimaryStats.ContainsKey(modifier.ModifiedValue))
                {
                    PrimaryStats[modifier.ModifiedValue].Full += Value;
                    RefreshDerived();
                }
                else if (DerivedStats.ContainsKey(modifier.ModifiedValue))
                    throw new Exception($"Derived stats shouldnt have modifiers: {modifier.ModifiedValue}"); //If derived stats had modifiers, then refreshing modifiers would be tough (not impossible)
                //DerivedStats[modifier.ModifiedValue].Full += Value;
                else throw new Exception($"Stat Modifier ModifiedValue not found: {modifier.ModifiedValue}");
            }
            else if (modifier.Type == "attribute")
            {
                if (Attributes.ContainsKey(modifier.ModifiedValue))
                    Attributes[modifier.ModifiedValue].Full += Value;
                else throw new Exception($"Attribute Modifier ModifiedValue not found: {modifier.ModifiedValue}");
            }
            else throw new Exception($"Modifier Type not found: {modifier.Type}");
        }

        //Adds a trait to the citizen's trait list (unless already exists) and then applies the modifiers
        //Company skills need to be recalculated after
        public void AddTrait(Trait trait)
        {
            if (!Traits.ContainsKey(trait.Name))
            {
                Traits[trait.Name] = trait;
                foreach (Modifier modifier in Traits[trait.Name].Modifiers)
                {
                    ApplyModifier(modifier);
                }
            }
        }

        //Removes a trait from teh citizen's trait list and then unapplies the modifiers
        //Company skills need to be recalculated after
        public void RemoveTrait(string name)
        {
            if (Traits.ContainsKey(name))
            {
                foreach (Modifier modifier in Traits[name].Modifiers)
                {
                    ApplyModifier(modifier, "remove");
                }
                Traits.Remove(name);
            }
        }
        #endregion
    }
}
