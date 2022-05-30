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
        public SkillBlock(int unmod, Stat pstat, Stat sstat)
        {
            Unmod = unmod;
            pStat = pstat;
            sStat = sstat;
            Known = false;
            Random random = new Random();
            Max = random.Next(400, 600);
            _modifiers = new();
            if (Full > 200) Known = true;
        }

        [JsonConstructor]
        public SkillBlock(int unmod, Stat pstat, Stat sstat, bool known, int max, Dictionary<string, SkillModifier> modifiers)
        {
            Unmod = unmod;
            pStat = pstat;
            sStat = sstat;
            Known = known;
            Max = max;
            _modifiers = modifiers;
        }

        public int Full {
            get {return _unmod + _totalModifiedValue; }
        }

        private int _unmod;
        public int Unmod{
            get { return _unmod; }
            internal set { _unmod = value; }
        }

        private int _totalModifiedValue
        {
            get
            {
                if (_modifiers.Count > 0)
                {  // Only checks if there are any Modifiers, else returns 0.
                    int value = 0;
                    List<string> expired = new();

                    // Checks each modifier to see if its expired, if not expired it adds it up; if expired, deletes it.
                    foreach (SkillModifier modifier in _modifiers.Values)
                    {
                        if (modifier.Expiration > DateTime.UtcNow) expired.Add(modifier.Name);
                        else value += modifier.Value;
                    }
                    foreach (String name in expired) RemoveModifier(name);
                    return value;
                }
                else return 0;
            }
        }

        private int _max;
        public int Max {
            get { return _max; }
            internal set { _max = value; }
        }

        public Stat pStat { get; }
        public Stat sStat { get; }
        public bool Known { get; set; }

        private Dictionary<string, SkillModifier> _modifiers { get; set; }

        public void ApplyModifier(SkillModifier modifier, PlayerCompany company)
        {
            // Sets temporary modifiers expiration based on the companies current time and the duration of the modifier.
            if (modifier.Temporary) modifier.Expiration = DateTime.UtcNow.Add(TimeSpan.FromSeconds(modifier.Duration));

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
