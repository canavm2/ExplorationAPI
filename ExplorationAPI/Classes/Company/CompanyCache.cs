using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using People;
using Users;

namespace Company
{
    public class CompanyCache
    {
        #region Constructor
        public CompanyCache()
        {
            id = Guid.NewGuid();
            PlayerCompanies = new();
        }

        [JsonConstructor]
        public CompanyCache(Guid ID, Dictionary<Guid, PlayerCompany> playerCompanies)
        {
            id=ID;
            PlayerCompanies = playerCompanies;
        }
        #endregion

        #region Dictionaries and Properties
        public Guid id { get; set; }
        public Dictionary<Guid, PlayerCompany> PlayerCompanies { get; set; }
        #endregion

        #region Methods
        public Guid CreateNewCompany(CitizenCache citizenCache, User user)
        {
            List<Citizen> advisors = new List<Citizen>();
            for (int i = 0; i < 7; i++)
                advisors.Add(citizenCache.GetRandomCitizen());
            Citizen master = citizenCache.GetRandomCitizen();
            PlayerCompany newCompany = new(user.UserName, master, advisors, user, citizenCache);
            this.PlayerCompanies[newCompany.id] = newCompany;
            return newCompany.id;
        }
        #endregion


    }
}
