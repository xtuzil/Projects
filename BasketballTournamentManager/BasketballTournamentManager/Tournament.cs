using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournamentManager
{
    public class Tournament
    {
        public string TournamentName { get; set; }
        public int MinPlayersPerTeam { get; set; }
        public int MaxPlayersPerTeam {get; set;}
        public string Area { get; set; }

        private bool started = false;

        private List<TournamentCategory> categories = new List<TournamentCategory>();
        private List<Player> players = new List<Player>();

        public Tournament(string name, int minPlayers, int maxPlayers, string area)
        {
            TournamentName = name;
            MinPlayersPerTeam = minPlayers;
            MaxPlayersPerTeam = maxPlayers;
            Area = area;
        }

        public bool AddCategory(TournamentCategory category)
        {
            if (!HasCategory(category.CategoryName))
            {
                categories.Add(category);
                return true;
            }
            return false;
        }

        public bool HasCategory(string category)
        {
            foreach (var cat in categories)
            {
                if (cat.CategoryName == category) return true;
            }

            return false;
        }

        public TournamentCategory GetCategory(string category)
        {
            foreach (var cat in categories)
            {
                if (cat.CategoryName == category) return cat;
            }
            return null;
        }

        public bool HasStarted()
        {
            return started;
        }

        public void Start()
        {
            Console.WriteLine("Tournament {0} start now!", TournamentName);
            started = true;
        }

        public void PrintCategories()
        {
            Console.WriteLine("\nExisting categories are:");
            foreach (var category in categories)
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        public Team AddTeamToCategory(string team, string categoryName)
        {
            foreach(var category in categories)
            {
                if (category.CategoryName == categoryName)
                {
                    if (category.HasTeam(team))
                    {
                        return category.GetTeam(team);
                    }
                    var newTeam = new Team(team);
                    category.AddTeam(newTeam);
                    return newTeam;
                }
            }
            return null;
        }

        public void AddTeamToCategory(Team team, string categoryName)
        {
            foreach (var category in categories)
            {
                if (category.CategoryName == categoryName)
                {
                    category.AddTeam(team);
                }
            }
        }

        public bool PlayerExists(string playerName)
        {
            foreach (var player in players)
            {
                if (player.Name == playerName) return true;
            }
            return false;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void SetGamePlan()
        {
            foreach(var category in categories)
            {
                category.SetGamePlan();
            }
        }

        public void PrintStats()
        {
            Console.WriteLine("\nMost scored points players:");
            players.OrderBy(t => t.PointsScored);
            foreach(var player in players)
            {
                Console.WriteLine("{0}: {1}", player.Name, player.PointsScored);
            }
        }
    }
}
