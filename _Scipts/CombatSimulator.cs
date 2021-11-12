using System;
using System.Collections;

namespace combatSimulator
{
    class CombatSimulator
    {
        const int BASE_HEALTH = 100;
        const int BASE_ARMOR = 50;

        static int playerNum = 1;
        static void InitializePlayer(out ArrayList player)
        {
            //ArrayList index 0: Name
            //ArrayList index 1: Health
            //ArrayList index 2: Armor
            player = new ArrayList();
            player.Add(ReadString($"Give the name of player #{playerNum}"));
            player.Add(BASE_HEALTH);
            player.Add(BASE_ARMOR);

            playerNum++;
        }

        static void Main(string[] args)
        {

            ArrayList player1;
            ArrayList player2;

            InitializePlayer(out player1);
            InitializePlayer(out player2);

            Console.WriteLine();

            GameLoop(player1, player2);
        }

        static void GameLoop(ArrayList player1, ArrayList player2)
        {
            ArrayList scoreboard = new ArrayList();

            int pl1Health, pl2Health;
            int pl1Armor, pl2Armor;

            //ArrayList index 0: Name
            //ArrayList index 1: Health
            //ArrayList index 2: Armor

            while (true)
            {
                //Player 1 attacks player 2
                ExecuteAttack(player2);
                pl2Health = int.Parse(player2[1].ToString());
                pl2Armor = int.Parse(player2[2].ToString());

                AddEntry(scoreboard, player2[0].ToString(), pl2Health, pl2Armor);

                if (IsDead(player2))
                {
                    Console.WriteLine($"{player2[0]} is dead.");
                    break;
                }

                //Player 2 attacks player 1
                ExecuteAttack(player1);
                pl1Health = int.Parse(player1[1].ToString());
                pl1Armor = int.Parse(player1[2].ToString());

                AddEntry(scoreboard, player1[0].ToString(), pl1Health, pl1Armor);
                if (IsDead(player1))
                {
                    Console.WriteLine($"{player1[0]} is dead.");
                    break;
                }
                PrintSeparatorLines();

                while (!WaitForEnter()) ;
            }

            PrintScoreboard(scoreboard);

            //Only purpose is for the exe to not close at the end
            Console.ReadKey();
        }

        static void ExecuteAttack(ArrayList defender)
        {
            //cache the defenders health and armor
            int defHealth = int.Parse(defender[1].ToString());
            int defArmor = int.Parse(defender[2].ToString());

            Random randomizer = new Random();
            int damage = randomizer.Next(5, 11);

            //If armor is below 0, subtract damage from health
            if (defArmor <= 0)
            {
                defHealth -= damage;

                //Keep the value at 0 so it does not print as a negative
                if (defHealth <= 0)
                {
                    defHealth = 0;
                }

                Console.WriteLine($"Player {defender[0]} was dealt {damage} damage to his health");
            }
            else //else if armor is present, subtract 1 from damage and then subtract this value from the armor
            {
                damage -= 1;
                defArmor -= damage;

                //Keep the value at 0 so it does not print as a negative
                if (defArmor <= 0)
                {
                    defArmor = 0;
                }

                Console.WriteLine($"Player {defender[0]} was dealt {damage} damage to his armor");
            }

            //Put the health and armor back to the defender
            defender[1] = defHealth;
            defender[2] = defArmor;
        }

        /// <summary>
        /// Check to see if the player who got attacked is still alive
        /// </summary>
        /// <param name="defender">The defender ArrayList reference</param>
        /// <returns>False if defender.Health > 0, true is defender.Health <= 0</returns>
        static bool IsDead(ArrayList defender)
        {
            int defHealth = int.Parse(defender[1].ToString());

            if (defHealth <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static int roundNum = 1;
        /// <summary>
        /// Creates a string consisting of the Defenders name, his health value and his armor value,
        /// then adds it to the scoreboard ArrayList
        /// </summary>
        /// <param name="scoreboard">The scoreboard reference</param>
        /// <param name="defenderName">The defenders name</param>
        /// <param name="health">The defenders health</param>
        /// <param name="armor">The defenders armor</param>
        static void AddEntry(ArrayList scoreboard, string defenderName, int health, int armor)
        {
            string completedEntry = $"At round {roundNum}:\n\tPlayer {defenderName} was attacked\n" +
                $"\tHealth: {health}\tArmor: {armor}";
            scoreboard.Add(completedEntry);

            roundNum++;
        }

        /// <summary>
        /// Prints the scoreboard at the end of the game
        /// </summary>
        /// <param name="scoreboard">The ArrayList to print</param>
        static void PrintScoreboard(ArrayList scoreboard)
        {
            PrintSeparatorLines();
            Console.WriteLine("\t\tSCOREBOARD");
            PrintSeparatorLines();

            foreach (string entry in scoreboard)
            {
                Console.WriteLine(entry);
            }
        }

        /// <summary>
        /// Continuously ask the user for input until the input is not empty of null
        /// </summary>
        /// <param name="prompt">What the user will see in the console</param>
        /// <returns>A string of the user input</returns>
        static string ReadString(string prompt)
        {
            string tempStr = "";
            do
            {
                Console.WriteLine(prompt);
                tempStr = Console.ReadLine();
            } while (string.IsNullOrEmpty(tempStr) || string.IsNullOrWhiteSpace(tempStr));

            return tempStr;
        }

        /// <summary>
        /// Continuously ask the user to press enter and wait for his input
        /// </summary>
        /// <returns>Returns true when ENTER is pressed, False when any other key is pressed</returns>
        static bool WaitForEnter()
        {
            ConsoleKeyInfo ckInfo;
            while (true)
            {
                Console.WriteLine("Press ENTER to continue to the next round.");
                ckInfo = Console.ReadKey(false);

                if (ckInfo.Key == ConsoleKey.Enter)
                {
                    //Continue to the next round
                    PrintSeparatorLines();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        static void PrintSeparatorLines()
        {
            for (int i = 0; i < 50; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }
    }
}
