using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using People;
using Company;

namespace Users
{
    public class UserCache
    {
        #region Constructors
        public UserCache()
        {
            id = Guid.NewGuid();
            Users = new();
            LastSave = DateTime.Now;
        }

        [JsonConstructor]
        public UserCache(Guid Id, Dictionary<string, User> users, DateTime lastSave)
        {
            id = Id;
            Users = users;
            LastSave = lastSave;
        }
        #endregion

        #region Dictionaries and Properties
        public Guid id { get; set; }
        public Dictionary<string, User> Users { get; set; }
        public DateTime LastSave { get; set; }
        #endregion

        #region Methods
        public void CreateNewUser(string userName, CitizenCache citizenCache, CompanyCache companyCache)
        {
            User NewUser =  new User(userName);
            NewUser.CompanyId = companyCache.CreateNewCompany(citizenCache, NewUser);
            this.Users[userName] = NewUser;
        }

        #endregion

        #region Subclasses
        #endregion
    }
}
