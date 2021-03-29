using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank
{
    public static class Extensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
    class ChainPatternExample
    {

        // Chain of responsibility pattern - Example       Judith Bishop September 2007
        // Sets up the Handlers as level based structure
        // Makes use of a user defined exception if the end of the chain is reached

        class Handler
        {
            Levels level;
            int id;

            public Handler(int id, Levels level)
            {
                this.id = id;
                this.level = level;
            }
            
            public string HandleRequest(int data)
            {
                int currLimit = (int) level;
                if (data < currLimit)
                {
                    return "Request for " + data + " handled by " + level + " " + id;
                }
                else if (level < First)
                {
                    Levels nextLevel = level.Next<Levels>();
                    int which = choice.Next(structure[nextLevel].Positions);
                    return handlersAtLevel[nextLevel][which].HandleRequest(data);
                }
                else
                {
                    Exception chainException = new ChainException();
                    chainException.Data.Add("Limit", data);
                    throw chainException;
                }
            }
        }

        public class ChainException : Exception
        {
            public ChainException() { }
        }

        enum Levels { Manager = 9000, Supervisor=4000, Clerk = 1000}
        static Random choice = new Random(11);

        static Levels First
        {
            get {
                //Console.WriteLine(((Levels[])Enum.GetValues(typeof(Levels)))[2]);
                return Enum.GetValues(typeof(Levels)).Cast<Levels>().Max(); 
            }
        }

        static Dictionary<Levels, Structure> structure =
                 new Dictionary<Levels, Structure> {
       {Levels.Manager, new Structure { Positions = 1}},
       {Levels.Supervisor, new Structure { Positions = 3}},
       {Levels.Clerk, new Structure { Positions = 10}}};


        static Dictionary<Levels, List<Handler>> handlersAtLevel =
                 new Dictionary<Levels, List<Handler>> {
      {Levels.Manager, new List <Handler>()},
      {Levels.Supervisor, new List <Handler>()},
      {Levels.Clerk, new List <Handler>()}};

        class Structure
        {
            public int Positions { get; set; }
        }
        static class Bank
        {
            static void Main()
            {

                Console.WriteLine("Trusty Bank opens with");
                foreach (Levels level in Enum.GetValues(typeof(Levels)))
                {
                    for (int i = 0; i < structure[level].Positions; i++)
                    {
                        handlersAtLevel[level].Add(new Handler(i, level));
                    }
                    int currLimit = (int)level;
                    Console.WriteLine(structure[level].Positions + " " + level +
                         "(s) who deal up to a limit of " + currLimit);
                }
                Console.WriteLine();

                int[] amounts = { 50, 2000, 1500, 10000, 175, 4500, 2000 };
                foreach (int amount in amounts)
                {
                    try
                    {
                        int which = choice.Next(structure[Levels.Clerk].Positions);
                        Console.Write("Approached Clerk " + which + ". ");
                        Console.WriteLine(handlersAtLevel[Levels.Clerk][which].HandleRequest(amount));
                    }
                    catch (ChainException e)
                    {
                        Console.WriteLine("\nNo facility to handle a request of " + e.Data["Limit"] +
                        "\nTry breaking it down into smaller requests\n");
                    }
                }
                Console.ReadKey();
            }
        }
    }
    /*Output
    Trusty Bank opens with
    1 Manager(s) who deal up to a limit of 9000
    3 Supervisor(s) who deal up to a limit of 4000
    10 Clerk(s) who deal up to a limit of 1000

    Approached Clerk 4. Request for 50 handled by Clerk 4
    Approached Clerk 0. Request for 2000 handled by Supervisor 1
    Approached Clerk 9. Request for 1500 handled by Supervisor 0
    Approached Clerk 1. 
    No facility to handle a request of 10000
    Try breaking it down into smaller requests

    Approached Clerk 3. Request for 175 handled by Clerk 3
    Approached Clerk 3. Request for 4500 handled by Manager 0
    Approached Clerk 1. Request for 2000 handled by Supervisor 1
    */

}