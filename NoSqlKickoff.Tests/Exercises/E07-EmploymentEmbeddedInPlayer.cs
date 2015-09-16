﻿using System.Collections.Generic;
using System.Linq;

using NoSqlKickoff.Indexes;
using NoSqlKickoff.Indexes.Exercises;
using NoSqlKickoff.Model;

using NUnit.Framework;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Tests.Helpers;

using ServiceStack.Text;

namespace NoSqlKickoff.Tests.Exercises
{
    public class E07_EmploymentEmbeddedInPlayer : RavenTestBase
    {
        private IDocumentStore _store;


        /// <summary>
        /// TODO: Exercise 9b
        /// As a user I want to know what players have been employed by "Borussia Dortmund" in season "2013-2014".
        /// </summary>
        public List<Player> FindPlayersOfDortmundIn1314()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<E07_PlayerIndex.IndexEntry, E07_PlayerIndex>()
                    .Where(e => e.TeamName == "Borussia Dortmund" && e.Season == "2013-2014")
                    .OfType<Player>()
                    .ToList();
            }
        }

        //TODO: add more exercises to show different problems: All employments of a player, Update of employment, Player Search which returns player info and current employment
        //TODO: one more case: Employments in separate collection but share same id prefix with team or player (also possible to do recurse on id list + LoadDocument

        [Test]
        public void FindPlayersOfDortmundIn1314_ShouldReturnAllPlayersOfDortmundIn20132014()
        {
            var players = FindPlayersOfDortmundIn1314();

            WaitForUserToContinueTheTest(_store);

            players.PrintDump();

            Assert.That(players.Count, Is.AtLeast(1));
        }

        [SetUp]
        public void SetUp()
        {
            _store = NewDocumentStore();

            IndexCreation.CreateIndexes(typeof(Player_Index_R03).Assembly, _store);

            var playerDictionary = DataGenerator.CreatePlayerList().ToDictionary(p => p.Id);
            var teamList = DataGenerator.CreateTeamList();
            var employmentList = DataGenerator.CreateEmploymentList();

            foreach (var employment in employmentList)
            {
                playerDictionary[employment.PlayerId].Employments.Add(employment);
            }

            using (var bulkInsert = _store.BulkInsert(null, new BulkInsertOptions { OverwriteExisting = true }))
            {
                foreach (var player in playerDictionary.Values)
                {
                    bulkInsert.Store(player);
                }

                foreach (var team in teamList)
                {
                    bulkInsert.Store(team);
                }
            }

            WaitForIndexing(_store);
        }
    }
}