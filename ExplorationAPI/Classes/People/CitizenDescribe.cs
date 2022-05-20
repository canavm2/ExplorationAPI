﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People
{
    public partial class Citizen
    {
        // Describes the Citizen, full can be passed as true to give detailed info for devs only.
        public string Describe(Boolean full = false)
        {
            string returnDescription =
                $"\n{Name}, a {Age} year old {Gender} {Race.Name}.\n" +
                $"\nTheir stats are:\n\n" +
                DescribeStats(full) +
                $"\nTheir skills are:\n\n" +
                DescribeSkills(full) +
                $"\nThis citizen's ID: {id}\n\n";

            return returnDescription;
        }

        public string DescribeStats(Boolean full = false)
        {
            //Iterates over all the Primary stats, and provides a string that describes it
            string primaryDesc = "";
            foreach (KeyValuePair<string, Stat> stat in PrimaryStats)
            {
                if (stat.Value.Known || full)
                {
                    primaryDesc += $"{stat.Key.ToUpper()}: {stat.Value.Full.ToString()}";
                    // Adds information for devs
                    if (stat.Value.RacialModifier != 0 & full) primaryDesc += $", {this.Race.Name}: {stat.Value.RacialModifier}";
                        primaryDesc += "\n";
                }
                else primaryDesc += $"{stat.Key.ToUpper()}: ?? \n";
            }
            string derivedDesc = "";
            foreach (KeyValuePair<string, DerivedStat> stat in DerivedStats)
            {
                derivedDesc += $"{stat.Key.ToUpper()}: {stat.Value.Full.ToString()}\n";
            }
            string description =
                $"Primary Stats:\n" +
                primaryDesc +
                $"\nDerived Stats:\n" +
                derivedDesc;
            ;
            return description;
        }

        public string DescribeSkills(Boolean full = false)
        {
            string primaryDesc = "";
            foreach (var skill in Skills)
            {
                if (skill.Value.Known || full) primaryDesc += $"{skill.Key.ToUpper()}({skill.Value.pStat},{skill.Value.sStat}): {skill.Value.Full}\n";
                if (full) primaryDesc += $"     Unmod: {skill.Value.Unmodified}, StatAdj: {skill.Value.StatAdjustment}\n";
            }
            return primaryDesc;
        }

    }
}