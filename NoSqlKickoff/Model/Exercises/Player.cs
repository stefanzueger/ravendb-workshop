﻿using System.Collections.Generic;

namespace NoSqlKickoff.Model.Exercises
{
    public class Player
    {
        public Player()
        {
            Employments = new List<Employment>();
        }


        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Nationality Nationality { get; set; }

        public List<Employment> Employments { get; set; }
    }
}