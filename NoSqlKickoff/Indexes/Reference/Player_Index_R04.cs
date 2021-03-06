﻿using System.Linq;

using NoSqlKickoff.Model.Exercises;

using Raven.Client.Indexes;

namespace NoSqlKickoff.Indexes.Reference
{
    public class Player_Index_R04 : AbstractIndexCreationTask<Player>
    {
        public class IndexEntry
        {
            public string Name { get; set; }

            public string FirstNameRenamed { get; set; }

            // This underscore Syntax can be used for property chains in Indexes
            public string Nationality_Name { get; set; }

            // A better option is to store the field as normal property and use type coercion in the query
            public string Nationality { get; set; }
        }

        public Player_Index_R04()
        {
            Map = players => from player in players
                             select new IndexEntry
                             {
                                 Name = player.FirstName + " " + player.LastName,
                                 FirstNameRenamed = player.FirstName,
                                 Nationality_Name = player.Nationality.Name,
                                 Nationality = player.Nationality.Name
                             };
        }
    }
}
