using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournamentManager
{
    public class Team 
    {
        public string Name { get; set;}
        public List<Player> players;
        public int GamesPlayed { get; set; }
        public int GamesWin { get; set; }
        public int PointsScored { get; set; }

        public Team(string name)
        {
            Name = name;
            players = new List<Player>();
            GamesPlayed = 0;
            GamesWin = 0;
            PointsScored = 0;
        }

        public bool AddPlayer(Player player)
        {
            players.Add(player);
            return true;
        }

        public void PlayerScore(int teamPoints, List<int> list)
        {
            int total = 0;
            foreach(var player in players)
            {
                Console.WriteLine("\nPlayer {0} scored (team {1}): ", player.Name, player.Team);
                int points = StaticMethods.ReadNumber("Player scored: ");
                player.PointsScored += points;
                total += points;
                list.Add(points);
            }
            if (total != teamPoints)
            {
                Console.WriteLine("\nTeam score is {0}, but players scored {1}!", teamPoints, total);
                Console.WriteLine("We have to reapeat it");
                list.Clear();
                PlayerScore(teamPoints, list);
            }
        }
        
    }
}
