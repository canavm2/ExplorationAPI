using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FileTools;

namespace People
{
    public enum Skill
    {
            Academia,
            Athletics,
            AnimalHandling,
            Blacksmithing,
            Carpentry,
            Cooking,
            Diplomacy,
            Drill,
            Engineering,
            FirstAid,
            History,
            Hunting,
            Law,
            Leadership,
            Leatherworking,
            Martial,
            Medical,
            Metalworking,
            Pathfinding,
            Persuation,
            Politics,
            Prospecting,
            Refining,
            Quartermastery,
            Skullduggery,
            Stealth,
            Survival,
            Tactics,
            Tinker
    }
    public class SkillBlock
    {
        #region Constructors
        public SkillBlock(int unmod, Stat pstat, Stat sstat)
        {
            _unmod = unmod;
            pStat = pstat;
            sStat = sstat;
            Known = false;
            Random random = new Random();
            _max = random.Next(400, 600);
            _modifiers = new();
        }

        [JsonConstructor]
        public SkillBlock(int _Unmod, Stat pstat, Stat sstat, bool known, int _Max, Dictionary<string, SkillModifier> _Modifiers)
        {
            pStat = pstat;
            sStat = sstat;
            Known = known;
            _unmod = _Unmod;
            _max = _Max;
            _modifiers = _Modifiers;
        }
        #endregion

        #region Dictionaries and Properties
        public Stat pStat { get; set; }
        public Stat sStat { get; set; }
        public bool Known { get; set; }

        public int _unmod { get; set; }

        public int _max { get; set; }
        public Dictionary<string, SkillModifier> _modifiers { get; set;  }
        #endregion

        public void ApplyModifier(SkillModifier modifier, PlayerCompany company)
        {
            // Sets temporary modifiers expiration based on the companies current time and the duration of the modifier.
            if (modifier.Temporary) modifier.Expiration = company.TimeBlock.CurrentTime.Add(TimeSpan.FromSeconds(modifier.Duration));

            // Checks to see if the modifier is in the Modifiers list already.
            if (_modifiers.ContainsKey(modifier.Name))
            {
                SkillModifier currentModifier = _modifiers[modifier.Name];
                // If the current modifier is bigger, returns without adding the new modifier.  Otherwise replaces the current with the new
                if ((modifier.Value <= currentModifier.Value)) return;
                else _modifiers[modifier.Name] = modifier;
            }
            // If its not in the Modifiers list, it adds the modifier to the list.
            else
            {
                _modifiers.Add(modifier.Name, modifier);
            }
        }
        public void RemoveModifier(string name)
        {
            if (_modifiers.ContainsKey(name)) _modifiers.Remove(name);
            else throw new Exception("No modifier to remove.");
        }
    }

}
