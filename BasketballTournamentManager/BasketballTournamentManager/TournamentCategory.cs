using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using static BasketballTournamentManager.Enum;

namespace BasketballTournamentManager
{
    public class TournamentCategory
    {
        public string CategoryName { get;}
        public Sex Sex { get; }
        public int MinAge { get;}
        public int MaxAge { get;}
        public BallSize Ball {get;}

        public TournamentSystem System { get; }

        private List<Team> teams = new List<Team>();
        private List<Team> teamsToAdvance = new List<Team>();
        private List<Game> gamePlan = new List<Game>();

        public TournamentCategory(string name, Sex sex, int minAge, int maxAge, BallSize ball, TournamentSystem system)
        {
            CategoryName = name;
            Sex = sex;
            MinAge = minAge;
            MaxAge = maxAge;
            Ball = ball;
            System = system;
        }

        public bool HasTeam(string name)
        {
            foreach (var team in teams)
            {
                if (team.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public Team GetTeam(string name)
        {
            foreach (var team in teams)
            {
                if (team.Name == name)
                {
                    return team;
                }
            }
            return null;
        }

        public void AddTeam(Team team)
        {
            teams.Add(team);
        }


        public void SetGamePlan()
        {
            if (System == TournamentSystem.EveryoneWithEveryone)
            {
                SetEveryoneWithEveryone();
            } 
            if (System == TournamentSystem.FirstLossOut)
            {
                SetFirstLossOut();
            }
        }

        public void SetFirstLossOut()
        {
            int numberOfMatch = 0;
            for (int i = 0; i < teams.Count(); i += 2)
            {
                numberOfMatch++;
                var newMatch = new Game(teams[i], teams[i+1], numberOfMatch);
                gamePlan.Add(newMatch);
            }
            if (teams.Count % 2 != 0)
            {
                teamsToAdvance.Add(teams.Last());
            } 
        }

        public void SetNextRoundFirstLossOut()
        {
            if (!AllGamesPlayed())
            {
                Console.WriteLine("\nThere are games that are not played yet");
                return;
            }

            if (teamsToAdvance.Count() == 1)
            {
                Console.WriteLine("\nThe tournament is finished. Winner is {0}, congrats!", teamsToAdvance[0].Name);
                return;
            }
            int numberOfMatch = gamePlan.Last().NumberOfMatch;
            for (int i = 0; i < teamsToAdvance.Count(); i += 2)
            {
                numberOfMatch++;
                var newMatch = new Game(teamsToAdvance[i], teamsToAdvance[i + 1], numberOfMatch);
                gamePlan.Add(newMatch);
            }
            teamsToAdvance.Clear();
            if (teams.Count % 2 != 0)
            {
                Team last = teams.Last();
                teamsToAdvance.Add(last);
            }
            
        }

        public void SetEveryoneWithEveryone()
        {
            if (teams.Count < 2)
            {
                Console.WriteLine("There are less than 2 teams in category!");
                return;
            }

            int numberOfMatch = 0;
            for(int i = 0; i < teams.Count(); i++)
            {
                for (int j = i + 1; j < teams.Count(); j++ )
                {
                    numberOfMatch++;
                    var newMatch = new Game(teams[i], teams[j], numberOfMatch);
                    gamePlan.Add(newMatch);
                }
            }

        }

        public void PrintSchedule()
        {
            Console.WriteLine("\nScheduled games:");
            foreach (var match in gamePlan)
            {
                Console.WriteLine("Match #{0}: {1} vs. {2}       - {3}", match.NumberOfMatch, match.HomeTeam.Name, match.AwayTeam.Name, match.Played ? match.HomePoints.ToString() + " : " + match.AwayPoints.ToString() : "not played");
            }
        }

        public void PrintTable()
        {
            Console.WriteLine("\n---- {0} table ----", CategoryName);
            teams.OrderBy(t => t.PointsScored).OrderBy(t => t.GamesWin);
            for(int i = 1; i <= teams.Count; i++)
            {
                Console.WriteLine("#{0}  {1}     games played: {2},  win: {3},  points scored:{4}" , i, teams[i+1], teams[i + 1].GamesPlayed, teams[i + 1].GamesWin, teams[i + 1].PointsScored);
            }

        }

        public bool MatchExists(Game match)
        {
            foreach(var m in gamePlan)
            {
                if ((m.HomeTeam == match.HomeTeam && m.AwayTeam == match.AwayTeam) || (m.HomeTeam == match.AwayTeam && m.AwayTeam == match.HomeTeam))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanAddResult(int number)
        {

            return (number <= gamePlan.Count() && !gamePlan[number - 1].Played);
        }

        public bool CanAddResult(string first, string second)
        {
            foreach(var game in gamePlan)
            {
                if ((game.HomeTeam.Name == first && game.AwayTeam.Name == second) || (game.HomeTeam.Name == second && game.AwayTeam.Name == first))
                {
                    return true;
                }
            }
            return false;
        }

        public Game GetGame(int number)
        {
            return gamePlan[number - 1];
        }

        public Game GetGame(string first, string second)
        {
            foreach (var game in gamePlan)
            {
                if ((game.HomeTeam.Name == first && game.AwayTeam.Name == second) || (game.HomeTeam.Name == second && game.AwayTeam.Name == first))
                {
                    return game;
                }
            }
            return null;
        }

        public void TeamAdvance(Team team)
        {
            teamsToAdvance.Add(team);
        }

        public bool AllGamesPlayed()
        {
            foreach(var game in gamePlan)
            {
                if (!game.Played) return false;
            }
            return true;
        }

        public async Task ExportSpider()
        {
            string fileName = @"..\..\\OutputData\Spider" + CategoryName + ".txt";
             using (var writer = File.CreateText(fileName))
             {
                await writer.WriteAsync("Category:" + CategoryName + "spider\n");

                int matchesInRound = teams.Count / 2;
                int matchesDone = 0;
                string leftSeparator = " |";
                string rightSeparator = "| ";
                int index = 0;
                int coe = 0;
                while (index < gamePlan.Count)
                {
                    for (int i = 0; i < matchesInRound; i++)
                    {
                        string opponents = GetOpponents(gamePlan[i + matchesDone]);
                        await writer.WriteAsync(leftSeparator + opponents + rightSeparator);
                        index++;
                    }
                    await writer.WriteAsync("\n\n");
                    if (matchesInRound == 1)
                    {
                        await writer.WriteAsync(leftSeparator + GetWinner(gamePlan.Last()) + rightSeparator);
                    }
                    coe++;
                    matchesDone += matchesInRound;
                    matchesInRound = matchesInRound / 2;
                    leftSeparator = new String(' ', 17 * coe) + leftSeparator;
                    rightSeparator = rightSeparator + new String(' ', 17 * coe);


                }

                writer.Flush();
             }

        }


        public int GetSpacesDependOnName(Game game)
        {
            int spaces = (30 - (game.HomeTeam.Name.Length + game.AwayTeam.Name.Length)) / 2;
            return spaces > 0 ? spaces : 0;
        }

        public string GetOpponents(Game game)
        {
            int spaces = GetSpacesDependOnName(game);
            return new String('-', spaces) + game.HomeTeam.Name + " vs. " + game.AwayTeam.Name + new String('-', spaces);
        }

        public string GetWinner(Game game)
        {
            int spaces = (30 - game.Winner.Name.Length) / 2;
            return new String('-', spaces) + game.Winner.Name + new String('-', spaces);
        }

    }
}


