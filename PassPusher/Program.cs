using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassPusher
{
    class Program
    {
        static void Main(string[] args)
        {
            string login,
                   password;

            if (!DBService.TryConnection())
            {
                Console.WriteLine("Cannot open connection!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Connection open successfully.");
            Console.Write("Enter user login: ");
            login = Console.ReadLine();
            Console.Write("Enter user password: ");
            PasswordEnterLoop(out password);
            Console.WriteLine("\nPushing data...");

            if (!DBService.Push(password, login))
                Console.WriteLine("An error occurred.");
            Console.WriteLine("Successfully pushed.");
            Console.WriteLine("Press something to continue.");
            Console.ReadKey();
        }

        private static void PasswordEnterLoop(out string password)
        {
            password = "";
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if(keyInfo.Key != ConsoleKey.Backspace)
                {
                    if(keyInfo.Key == ConsoleKey.Enter)
                        break;

                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            } while (true);
        }
    }
}
