using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BasketballTournamentManager.Enum;

namespace BasketballTournamentManager
{
    public class Player 
    {
        public string Name { get;}
        public int Age { get; set; }
        public Sex Sex { get;}
        public int Weight { get; set; }
        public int Height { get; set; }
        public string Team { get; set; }

        public int PointsScored { get; set; }

        public Player(string name, int age, Sex sex, int weight, int height, string team)
        {
            Name = name;
            Age = age;
            Sex = sex;
            Weight = weight;
            Height = height;
            Team = team;
            PointsScored = 0;
        }

    }
}
