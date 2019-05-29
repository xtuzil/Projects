using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BasketballTournamentManager.Enum;

namespace BasketballTournamentManager
{
    public static class StaticMethods
    {


        public static int ReadNumber(string instruction)
        {
            int minPlayers;
            while (true)
            {
                Console.WriteLine(instruction);
                string lineToRead = Console.ReadLine();
                if (int.TryParse(lineToRead, out minPlayers)) break;
                Console.WriteLine("Write number!\n");
            }
            return minPlayers;
        }

        public static string GetCategoryName(Tournament tournament, string instruction)
        {
            string categoryName;

            while (true)
            {
                tournament.PrintCategories();
                Console.WriteLine(instruction);
                categoryName = Console.ReadLine();
                if (tournament.HasCategory(categoryName)) return categoryName;
            }
        }

        public static bool TryParsePlayer(string playerInfo, Team team, Tournament tournament)
        {
            var pattern = new Regex("(?<name>.*) (?<age>\\d+) (?<sex>(male|female)) (?<weight>\\d+) (?<height>\\d+)");
            Match match = pattern.Match(playerInfo);
            if (match.Success)
            {
                if (!tournament.PlayerExists(match.Groups["name"].Value))
                {
                    Sex sex = (match.Groups["sex"].Value == "male") ? Sex.Male : Sex.Female;
                    var newPlayer = new Player(match.Groups["name"].Value, int.Parse(match.Groups["age"].Value), sex, int.Parse(match.Groups["weight"].Value), int.Parse(match.Groups["height"].Value), team.Name);
                    tournament.AddPlayer(newPlayer);
                    team.AddPlayer(newPlayer);
                    return true;
                }
            }
            return false;
        }



    }
}
