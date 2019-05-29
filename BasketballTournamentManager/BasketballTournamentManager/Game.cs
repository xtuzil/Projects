using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournamentManager
{
    public class Game
    {
        public bool Played { get; set; }
        public int NumberOfMatch { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Team Winner { get; set; }
        public int HomePoints { get; set; }
        public int AwayPoints { get; set; }

        private List<int> homePlayersPoints = new List<int>();
        private List<int> awayPlayersPoints = new List<int>();

        public Game(Team home, Team away, int number)
        {
            HomeTeam = home;
            AwayTeam = away;
            NumberOfMatch = number;
            Played = false;
        }

        public List<int> GetHomePlayersPoints()
        {
            return homePlayersPoints;
        }

        public List<int> GetAwayPlayersPoints()
        {
            return awayPlayersPoints;
        }




    }
}
