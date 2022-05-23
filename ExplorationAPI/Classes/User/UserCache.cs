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
    public interface IUserCache
    {
        public Guid id { get; set; }
        public Dictionary<string, User> Users { get; set; }
        public bool CheckforUser(string userName);
        public void CreateNewUser(string username, byte[] hash, byte[] salt, CitizenCache citizenCache, CompanyCache companyCache);
    }

    public class UserCache : IUserCache
    {
        #region Constructors
        public UserCache()
        {
            id = Guid.NewGuid();
            Users = new();
        }

        [JsonConstructor]
        public UserCache(Guid Id, Dictionary<string, User> users)
        {
            id = Id;
            Users = users;
        }
        #endregion

        #region Dictionaries and Properties
        public Guid id { get; set; }
        public Dictionary<string, User> Users { get; set; }
        #endregion

        #region Methods
        public bool CheckforUser(string userName)
        {
            if (Users.ContainsKey(userName)) return true;
            else return false;
        }
        public void CreateNewUser(string username, byte[] hash, byte[] salt , CitizenCache citizenCache, CompanyCache companyCache)
        {
            if (CheckforUser(username)) throw new Exception("error, user already exists");
            User NewUser =  new User(username, hash, salt);
            NewUser.CompanyId = companyCache.CreateNewCompany(citizenCache, NewUser);
            this.Users[username] = NewUser;
        }

        #endregion

        #region Subclasses
        #endregion
    }
}
