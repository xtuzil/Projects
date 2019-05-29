using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BasketballTournamentManager.Enum;

namespace BasketballTournamentManager
{
    class CreateCategories
    {
        private string lineToRead = "";
        private Tournament actualTournament;

        public CreateCategories(Tournament t)
        {
            actualTournament = t;
        }

        public void AddCategories()
        {
            while (true)
            {
                Console.WriteLine("\nYou can add category:");
                Console.WriteLine("'0' - Back");
                Console.WriteLine("'1' - Add category");

                lineToRead = Console.ReadLine();
                if (lineToRead.Length != 0)
                {
                    if (lineToRead[0] == '1')
                    {
                        AddCategory();
                        continue;

                    }
                    if (lineToRead[0] == '0')
                    {
                        break;
                    }
                }
            }
        }

        public void AddCategory()
        {
            string categoryName;
            while (true)
            {
                Console.WriteLine("\nCategory name:");
                categoryName = Console.ReadLine();
                if (!actualTournament.HasCategory(categoryName)) break;
                Console.WriteLine("Category already exists!");
            }
            int minAge = StaticMethods.ReadNumber("Minimum players age:");
            int maxAge = StaticMethods.ReadNumber("Maximum players age");

            bool done = false;
            Sex sex = Sex.Both;
            while (!done)
            {
                Console.WriteLine("\nMale, Female or does not matter category - write 'm' or 'f' or 'b' (both): ");
                lineToRead = Console.ReadLine();
                if (lineToRead.Length != 0)
                {
                    done = true;
                    switch (lineToRead[0])
                    {
                        case 'm': sex = Sex.Male; break;
                        case 'f': sex = Sex.Female; break;
                        case 'b': sex = Sex.Both; break;
                        default: done = false; Console.WriteLine("wrong input!"); continue;
                    }
                }
            }
            done = false;
            BallSize ball = BallSize.Seven;
            while (!done)
            {
                Console.WriteLine("\nSize of ball ('5','6','7'): ");
                lineToRead = Console.ReadLine();
                if (lineToRead.Length != 0)
                {
                    done = true;
                    switch (lineToRead[0])
                    {
                        case '5': ball = BallSize.Five; break;
                        case '6': ball = BallSize.Six; break;
                        case '7': ball = BallSize.Seven; break;
                        default: done = false; Console.WriteLine("wrong input!"); break;
                    }
                }
            }

            TournamentSystem system;
            while (true)
            {
                Console.WriteLine("/nTournament system ('e' - Everyone with everyone, 'f' - First loss out): ");
                lineToRead = Console.ReadLine();
                if (lineToRead.Length != 0)
                {
                    if (lineToRead[0] == 'f')
                    {
                        system = TournamentSystem.FirstLossOut;
                        break;
                    }
                    if (lineToRead[0] == 'e')
                    {
                        system = TournamentSystem.EveryoneWithEveryone;
                        break;
                    }
                }
            }
            Console.WriteLine("\nCategory was successfully created!");
            actualTournament.AddCategory(new TournamentCategory(categoryName, sex, minAge, maxAge, ball, system));
        }
    }
}
