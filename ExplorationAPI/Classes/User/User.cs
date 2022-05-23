using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Company;
using People;

namespace Users
{
    public class User
    {
        #region Constructors
        public User(string username, byte[] hash, byte[] salt)
        {
            UserName = username;
            PasswordHash = hash;
            PasswordSalt = salt;
            id = new Guid();
            Admin = false;
        }

        [JsonConstructor]
        public User(string userName, Guid Id, Guid companyId, byte[] passwordHash, byte[] passwordSalt, bool admin)
        {
            UserName = userName;
            id = Id;
            CompanyId = companyId;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Admin = admin;
        }
        #endregion

        #region Dictionaries and Properties
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Guid id { get; set; }
        public Guid CompanyId { get; set; }
        public bool Admin { get; set; }
        #endregion

        #region Methods
        //public string Describe()
        //{
        //    string description = $"{UserName} you have {TimePoints} timepoints, which is about {Convert.ToInt32(TimeSpan.FromSeconds(TimePoints).TotalHours)} hours of realtime.\n\n";
        //    if (TimeSpan.FromSeconds(TimePoints).TotalHours > 95) description += "You are full on timepoints and should spend some!\n\n";
        //    return description;
        //}
        
        #endregion
    }
    
}
