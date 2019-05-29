using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BasketballTournamentManager.Enum;

namespace BasketballTournamentManager
{
    class TournamentManager
    {
        private Tournament tournament;
        private string lineToRead = "";

        public TournamentManager(Tournament t)
        {
            tournament = t;
        }

        public async Task StartEdit()
        {
            Console.WriteLine("{0} tournament", tournament.TournamentName);
            Console.WriteLine();
            if (!tournament.HasStarted())
            {
                Console.WriteLine("Tournament is not active yet.");
                await Edit();
            } else
            {
                Console.WriteLine("Tournament is active.");
                await ProcessActiveTournament();
            }      
        }

        public async Task Edit()
        {
            
            bool done = false;
            while (!done)
            {
                Console.WriteLine("\nYou can add category or players:");
                Console.WriteLine("'0' - Back");
                Console.WriteLine("'1' - Add category");
                Console.WriteLine("'2' - Add players");
                Console.WriteLine("'3' - Start tournament");

                lineToRead = Console.ReadLine();

                if (lineToRead.Length != 0)
                {
                    
                    switch (lineToRead[0])
                    {
                        case '1':
                            {
                                var newCategories = new CreateCategories(tournament);
                                newCategories.AddCategories();
                                break;
                            }   
                        case '2':
                            {
                                var addingPlayers = new AddPlayers(tournament);
                                await addingPlayers.StartAdding();
                                break;
                            }
                        case '3': StartTournament(); done = true; break;
                        case '0': done = true; break;
                        default: Console.WriteLine("Wrong input!"); break;
                    }
                }

            }
        }

        public void StartTournament()
        {
            tournament.SetGamePlan();
            tournament.Start();
        }

        public async Task ProcessActiveTournament()
        {
            bool done = false;
            while(!done)
            {
                Console.WriteLine("\n'0' - Back ");
                Console.WriteLine("'1' - Add a result of game");
                Console.WriteLine("'2' - Print schedule in category");
                Console.WriteLine("'3' - Print table in category (Everyone with Everyone system)");
                Console.WriteLine("'4' - Draw next round in category (First loss out)");
                Console.WriteLine("'5' - Print points stats!");
                Console.WriteLine("'6' - Export 'spider' to the file");

                lineToRead = Console.ReadLine();

                if (lineToRead.Length != 0)
                {

                    switch (lineToRead[0])
                    {
                        case '1': AddResult(); break;
                        case '2': PrintSchedule(); break;
                        case '3': PrintTable(); break;
                        case '4': DrawNextRound(); break;
                        case '5': PrintStats(); break;
                        case '6': await ExportSpider(); break;
                        case '0': done = true; break;
                        default: Console.WriteLine("Wrong input!"); break;
                    }
                }
            } 
        }

        public async Task ExportSpider()
        {
            TournamentCategory category = ReadCategory("\nWrite name of category where you want to export spider:");
            if (category.System == Enum.TournamentSystem.EveryoneWithEveryone)
            {
                Console.WriteLine("This category does not have rounds!");
                return;
            }
            await category.ExportSpider();
        }

        public void DrawNextRound()
        {
            TournamentCategory category = ReadCategory("\nWrite name of category where you want to draw next round:");
            if (category.System == Enum.TournamentSystem.EveryoneWithEveryone)
            {
                Console.WriteLine("This category does not have rounds!");
                return;
            }
            category.SetNextRoundFirstLossOut();
        }

        public void PrintTable()
        {
            TournamentCategory category = ReadCategory("\nWrite name of category where you want to print table:");
            if (category.System == Enum.TournamentSystem.FirstLossOut)
            {
                Console.WriteLine("This category does not have table!");
                return;
            }
            category.PrintTable();
        }

        public void PrintSchedule()
        {
            TournamentCategory category = ReadCategory("\nWrite name of category where you want to print Schedule:");
            category.PrintSchedule();
        }

        public void PrintStats()
        {
            tournament.PrintStats();
        }

        public TournamentCategory ReadCategory(string instruction)
        {
            return tournament.GetCategory(StaticMethods.GetCategoryName(tournament, instruction));
        }

        public void AddResult()
        {
            TournamentCategory category = ReadCategory("\nWrite name of category where you want to add result of game:");

            bool done = false;
            string gameName;
            while (!done)
            {
                Console.WriteLine("\n'0' - Cancel adding result");
                Console.WriteLine("'1' - Choose game by number of game");
                Console.WriteLine("'2' - Choose game by opponents");
                Console.WriteLine("'3' - Print schedule of games");
                gameName = Console.ReadLine();

                if (gameName.Length != 0)
                {
                    switch (gameName[0])
                    {
                        case '1': GameResultByNumber(category); break;
                        case '2': GameResultByOpponents(category); break;
                        case '3': category.PrintSchedule(); break;
                        case '0': done = true; break;
                        default: Console.WriteLine("Wrong input!"); break;
                    }
                }
            }
        }

        public void GameResultByNumber(TournamentCategory category) 
        {
            Game game;
            while(true)
            {
                Console.WriteLine();
                int numberOfGame = StaticMethods.ReadNumber("Write number of game:");
                if (category.CanAddResult(numberOfGame))
                {
                    game = category.GetGame(numberOfGame);
                    break;
                }
            }
            if (game.Played)
            {
                Console.WriteLine("\nGame #{0} {1} vs. {2} is played", game.NumberOfMatch, game.HomeTeam.Name, game.AwayTeam.Name);
                return;
            }
            GameScore(game, category);
        }

        public void GameResultByOpponents(TournamentCategory category)
        {
            Game game;
            while (true)
            {
                Console.WriteLine("\nWrite name of first team: ");
                string first = Console.ReadLine();
                Console.WriteLine("\nWrite name of second team: ");
                string second = Console.ReadLine();
                if(category.CanAddResult(first, second))
                {
                    game = category.GetGame(first, second);
                    break;
                }
            }
            if (game.Played)
            {
                Console.WriteLine("\nGame #{0} {1} vs. {2} is played", game.NumberOfMatch, game.HomeTeam.Name, game.AwayTeam.Name);
                return;
            }
            GameScore(game, category);

        }

        public void GameScore(Game game, TournamentCategory category)
        {
            Console.WriteLine("\nWrite scored points by team {0}:", game.HomeTeam.Name);
            int homePoints = StaticMethods.ReadNumber("");
            game.HomePoints = homePoints;

            Console.WriteLine("\nWrite scored points by team {0}:", game.AwayTeam.Name);
            int awayPoints = StaticMethods.ReadNumber("");
            game.AwayPoints = awayPoints;

            game.Winner = homePoints > awayPoints ? game.HomeTeam : game.AwayTeam;
            game.Winner.GamesWin++;
            game.HomeTeam.GamesPlayed++;
            game.AwayTeam.GamesPlayed++;
            game.HomeTeam.PointsScored += homePoints;
            game.AwayTeam.PointsScored += awayPoints;
            game.Played = true;
            
            game.HomeTeam.PlayerScore(homePoints, game.GetHomePlayersPoints());
            game.AwayTeam.PlayerScore(awayPoints, game.GetAwayPlayersPoints());

            if (category.System == TournamentSystem.FirstLossOut)
            {
                category.TeamAdvance(game.Winner);
            }
        }

    }
}
