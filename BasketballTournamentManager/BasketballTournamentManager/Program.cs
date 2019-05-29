using BasketballTournamentsManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournamentManager
{
    class Program
    {


        static async Task Main(string[] args)
        {
            var tournamentManager = new TournamentsManager();
            await tournamentManager.StartTournamentManager();
 
        }
    }
}
