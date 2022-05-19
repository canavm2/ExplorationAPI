using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using People;
using FileTools;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FileTools
{
    //An object that get instatiated to hold all the available traits
    //It reads them from the .csv files
    public class TraitList
    {
        public TraitList()
        {
            Traits = new();
            ListTool listTool = new();
            string[] lines = System.IO.File.ReadAllLines(path);
            for (int i = 1; i < lines.Length; i++)
            {
                List<Modifier> modifiers = new();
                string[] line = lines[i].Split(",");
                for (int j = 2; j < line.Length; j++)
                {
                    string[] split = line[j].Split(".");
                    string modifiedvalue = split[0];
                    int value = int.Parse(split[1]);
                    string type = "";
                    if (listTool.PrimaryStats.Contains(modifiedvalue)) type = "stat";
                    if (listTool.SkillsList.ContainsKey(modifiedvalue)) type = "skill";
                    if (listTool.Attributes.Contains(modifiedvalue)) type = "attribute";
                    string description = $"{modifiedvalue} {value}";

                    modifiers.Add(new Modifier(line[j], "trait", type, modifiedvalue, value, description));
                }
                Trait trait = new Trait(line[0],int.Parse(line[1]),modifiers);
                Traits[line[0]] = trait;
            }
        }

        [JsonConstructor]
        public TraitList(Dictionary<string, Trait> traits)
        {
            Traits = traits;
        }

        public Dictionary<string, Trait> Traits { get; set; }
        string path = @"C:\Users\canav\Documents\ExplorationProject\exploration_classes\csv_files\traits.csv";
    }


    //An Object to hold all the modifiers, currently not needed, since the traits hold the modifiers
    //new modifiers will probably get built from simple strings that can be parsed into modifiers.
    public class ModifierList
    {
        public ModifierList()
        {
            Modifiers = new();
            string[] lines = System.IO.File.ReadAllLines(path);
            for (int i= 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(",");
                Modifier modifier = new(
                    line[0],
                    line[1],
                    line[2],
                    line[3],
                    Int32.Parse(line[4]),
                    line[7],
                    Convert.ToBoolean(line[5]),
                    Int32.Parse(line[6])                    
                    );
                Modifiers[line[0]] = modifier;
            }
        }

        [JsonConstructor]
        public ModifierList(Dictionary<string, Modifier> modifiers)
        {
            Modifiers = modifiers;
        }

        public Dictionary<string, Modifier> Modifiers { get; set; }
        string path = @"C:\Users\canav\Documents\ExplorationProject\exploration_classes\csv_files\modifiers.csv";
    }
}
