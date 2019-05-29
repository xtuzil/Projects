using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BasketballTournamentManager
{
    class AddPlayers
    {
        private string lineToRead = "";
        private Tournament tournament;

        public AddPlayers(Tournament t)
        {
            tournament = t;
        }

        public async Task StartAdding()
        {
            bool done = false;
            while (!done)
            {
                Console.WriteLine("\n'0' - Back");
                Console.WriteLine("'1' - Add players one by one");
                Console.WriteLine("'2' - Add players from file");

                lineToRead = Console.ReadLine();

                if (lineToRead.Length != 0)
                {

                    switch (lineToRead[0])
                    {
                        case '1': AddPlayer(); break;
                        case '2': await AddPlayersFromFile(); break;
                        case '0': done = true; break;
                        default: Console.WriteLine("Wrong input!"); continue;
                    }
                }
            }
        }

        public async Task AddPlayersFromFile()
        {
            string categoryName = StaticMethods.GetCategoryName(tournament, "Write name of category where you want to add player:");

            string path;

            while (true)
            {
                Console.WriteLine("'0' - Back");
                Console.WriteLine("Write file name in JSON (ex. CuncaMuzi.json): ");

                lineToRead = Console.ReadLine();

                if (lineToRead.Length != 0)
                {
                    if (lineToRead == "0") return;
                    else
                    {
                        path = @"..\..\\InputData\" + lineToRead;
                        if (File.Exists(path)) 
                        {
                            break;
                        }
                        Console.WriteLine("File name does not exist!");
                    }
                   
                }

            }
            var teams = await Task.Run(() => JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText(path)));

            foreach (var team in teams)
            {
                tournament.AddTeamToCategory(team, categoryName);
                foreach (var player in team.players)
                {
                    tournament.AddPlayer(player);
                }
            }

            Console.WriteLine("Adding players from file was successful!");
        }

        public void AddPlayer()
        {
            string categoryName = StaticMethods.GetCategoryName(tournament, "Write name of category where you want to add player:");
            Console.WriteLine();
            string teamName;

            Console.WriteLine("\n'0' - Cancel adding players");
            Console.WriteLine();
            Console.WriteLine("Write name of team where you want to add player:");
            teamName = Console.ReadLine();
            if (teamName.Length == 1 && teamName[0] == '0') return;

            Team team = tournament.AddTeamToCategory(teamName, categoryName);
            
            while (true)
            {
                Console.WriteLine("\n'0' - Cancel adding players");
                Console.WriteLine("Write player with this format:");
                Console.WriteLine("Name age Sex(male/female) weight height");
                Console.WriteLine("Example: Matej Tuzil 21 male 80 188");
                Console.WriteLine();
                string playerInfo = Console.ReadLine();
                if (playerInfo.Length == 1 && playerInfo[0] == '0') break;
                
                if (StaticMethods.TryParsePlayer(playerInfo, team, tournament))
                {
                    Console.WriteLine("Player was successfully added");
                } else
                {
                    Console.WriteLine("Wrong format!");
                }
                Console.WriteLine();

            }



        }

    }
}
