using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BasketballTournamentManager.Enum;

namespace BasketballTournamentManager
{
    class CreateTournament
    {
        Tournament actualTournament;

        private List<Tournament> tournaments;

        public CreateTournament(List<Tournament> ts)
        {
            tournaments = ts;
        }

        public async Task Create()
        {
            string name;

            while (true)
            {
                Console.WriteLine("\nName of the tournament:");
                name = Console.ReadLine();
                if (!TournamentExists(name)) break;
                Console.WriteLine("Tournament with this name already exists");
            }
            int minPlayers = StaticMethods.ReadNumber("\nMinimum players per team:");
            int maxPlayers = StaticMethods.ReadNumber("\nMaximum players per team:");

            Console.WriteLine("\nArea of the tournament:");
            string area = Console.ReadLine();

            actualTournament = new Tournament(name, minPlayers, maxPlayers, area);

            var newCategories = new CreateCategories(actualTournament);
            newCategories.AddCategories();

            Console.WriteLine("\nYou can add players:");
            var newPlayers = new AddPlayers(actualTournament);
            await newPlayers.StartAdding();

            tournaments.Add(actualTournament);
        }




        public bool TournamentExists(string name)
        {
            foreach (var tournament in tournaments)
            {
                if (tournament.TournamentName == name)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
