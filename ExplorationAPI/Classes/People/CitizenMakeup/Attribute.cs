﻿using System.Text.Json.Serialization;

namespace People
{
    public class Attribute
    {
        #region Constructors
        public Attribute()
        {
            Full = 100;
            Unmodified = 100;
            Max = 300;
        }

        [JsonConstructor]
        public Attribute(int full, int unmodified, int max)
        {
            Full = full;
            Unmodified = unmodified;
            Max = max;
        }
        #endregion
        public int Full { get; set; }
        public int Unmodified { get; set; }
        public int Max { get; set; }
    }
}