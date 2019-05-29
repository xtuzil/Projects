using BasketballTournamentManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BasketballTournamentManager.Enum;

namespace BasketballTournamentsManager
{
    public class TournamentsManager 
    {
        private bool end = false;
        private string lineToRead = "";
        private List<Tournament> tournaments = new List<Tournament>();
        private Tournament actualTournament;

        public async Task StartTournamentManager()
        {
            Console.WriteLine("Welcome! This is Baskteball Tournament Manager.\n");

            PrintHelp();
            while (!end)
            {
                Console.WriteLine("Choose action - number + enter ('9' for help):");

                lineToRead = Console.ReadLine();
                Console.WriteLine();

                if (lineToRead.Length != 0)
                {
                    switch (lineToRead[0])
                    {
                        case '1': ShowCreatedTournaments(); break;
                        case '2': await ChooseTournament(); break;
                        case '3': await CreateTournament(); break;
                        case '9': PrintHelp(); break;
                        case '0': end = true; break;
                        default: Console.WriteLine("Wrong input!"); continue;
                    }
                }
            }
        }

        public void ShowCreatedTournaments()
        {
            Console.WriteLine("Active tournaments: ");
            foreach (var tournament in tournaments)
            {
                Console.WriteLine(tournament.TournamentName);
            }
            Console.WriteLine();
        }

        public async Task ChooseTournament()
        {
            bool done = false;
            while (!done)
            {
                Console.WriteLine("Write the exact name of tournament or '0' to back to main menu:");
                lineToRead = Console.ReadLine();
                if (lineToRead.Length != 0 && lineToRead[0] == '0') break;
                foreach (var tournament in tournaments)
                {
                    if (tournament.TournamentName == lineToRead)
                    {
                        actualTournament = tournament;
                        await ProcessToTournament(tournament);
                        done = true;
                        break;
                    }
                }
                if (done) break;
                Console.WriteLine("Tournament does not exist.\n");
            }
        }

        public async Task CreateTournament()
        {
            var newTournament = new CreateTournament(tournaments);
            await newTournament.Create();

            Console.WriteLine("Tournament was successfully created!\n");


            while (true)
            {
                Console.WriteLine("'0' - Exit to main menu");
                Console.WriteLine("'1' - Process to tournament");
                lineToRead = Console.ReadLine();
                if (lineToRead.Length != 0)
                {
                    if (lineToRead[0] == '0') break;
                    else if (lineToRead[0] == '1') await ProcessToTournament(tournaments.Last());
                }
                Console.WriteLine();
            }
        }

        public void PrintHelp()
        {
            Console.WriteLine("\n'0' - Exit");
            Console.WriteLine("'1' - Show created tournaments");
            Console.WriteLine("'2' - Choose tournament");
            Console.WriteLine("'3' - Create tournamenr");
            Console.WriteLine("'9' - Help");
        }

        public async Task ProcessToTournament(Tournament t)
        {
            var newTournamentEdit = new TournamentManager(t);
            await newTournamentEdit.StartEdit();
        }

    }
}
