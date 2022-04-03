using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using FileTools;

namespace People
{
    //Stores the citizens that are not in companies
    public class CitizenCache
    {
        #region Constructors
        //Creates a cache with full lists of every citizen sex
        public CitizenCache(int size = 0)
        {
            Name = "Testing";
            id = Guid.NewGuid();
            NameList nameList = new();
            FemaleCitizens = new();
            MaleCitizens = new();
            NBCitizens = new();
            for (int i = 0; i < size; i++)
            {
                Citizen femaleCitizen = new(nameList.generateName("female"), "female");
                FemaleCitizens.Add(femaleCitizen);
                Citizen maleCitizen = new(nameList.generateName("male"), "male");
                MaleCitizens.Add(maleCitizen);
                Citizen nbCitizen = new(nameList.generateName("non-binary"), "non-binary");
                NBCitizens.Add(nbCitizen);
            }
        }

        [JsonConstructor]
        public CitizenCache(Guid Id, string name, List<Citizen> femalecitizens, List<Citizen> malecitizens, List<Citizen> nbcitizens)
        {
            id=Id;
            Name=name;
            FemaleCitizens = femalecitizens;
            MaleCitizens = malecitizens;
            NBCitizens = nbcitizens;
        }
        #endregion

        #region Dictionaries and Properties
        public Guid id { get; set; }
        public string Name { get; set; }
        public List<Citizen> FemaleCitizens { get; set; }
        public List<Citizen> MaleCitizens { get; set; }
        public List<Citizen> NBCitizens { get; set; }
        #endregion

        #region Methods

        //Stores a citizen in the appropriate list by sex
        public void CacheCitizen(Citizen citizen)
        {
            if (citizen.Gender == "female") FemaleCitizens.Add(citizen);
            else if (citizen.Gender == "male") MaleCitizens.Add(citizen);
            else NBCitizens.Add(citizen);
        }

        //Retrieves a random citizen and removes them from the cache to prevent duplication
        public Citizen GetRandomCitizen(string gender = "random")
        {
            Citizen returncitizen;
            Random random = new Random();
            int index;
            if (gender == "random")
            {
                string[] genders = new string[] { "female", "male", "non-binary" };
                index = random.Next(genders.Length);
                gender = genders[index];
            }
            if (gender == "female")
            {
                index = random.Next(FemaleCitizens.Count);
                returncitizen = FemaleCitizens[index];
                FemaleCitizens.RemoveAt(index);
            }
            else if (gender == "male")
            {
                index = random.Next(MaleCitizens.Count);
                returncitizen = MaleCitizens[index];
                MaleCitizens.RemoveAt(index);
            }
            else
            {
                index = random.Next(NBCitizens.Count);
                returncitizen = NBCitizens[index];
                NBCitizens.RemoveAt(index);
            }
            return returncitizen;
        }
        #endregion
    }
}
