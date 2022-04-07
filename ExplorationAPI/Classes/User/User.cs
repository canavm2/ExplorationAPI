﻿using System;
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
            TimePoints = TimeSpan.FromDays(2).TotalSeconds;
        }

        [JsonConstructor]
        public User(string userName, Guid Id, Guid companyId, double timePoints, byte[] passwordHash, byte[] passwordSalt)
        {
            UserName = userName;
            id = Id;
            CompanyId = companyId;
            TimePoints = timePoints;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
        #endregion

        #region Dictionaries and Properties
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Guid id { get; set; }
        public Guid CompanyId { get; set; }
        public double TimePoints { get; set; }
        #endregion

        #region Methods
        public string Describe()
        {
            string description = $"{UserName} you have {TimePoints} timepoints, which is about {Convert.ToInt32(TimeSpan.FromSeconds(TimePoints).TotalHours)} hours of realtime.\n\n";
            if (TimeSpan.FromSeconds(TimePoints).TotalHours > 95) description += "You are full on timepoints and should spend some!\n\n";
            return description;
        }
        public void GainTimePoints(double timePoints)
        {
            double MaxTimePoints = TimeSpan.FromDays(4).TotalSeconds;
            if ((this.TimePoints + timePoints) > MaxTimePoints) this.TimePoints = MaxTimePoints;
            else this.TimePoints += timePoints;
        }
        public bool SpendTimePoints(double timePoints)
        {
            if (timePoints < this.TimePoints)
            {
                this.TimePoints -= timePoints;
                return true;
            }
            else return false;
        }
        #endregion
    }
    public class UserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
