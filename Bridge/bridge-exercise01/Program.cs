using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bridge_exercise01
{
    // Bridge Pattern Example                           Judith Bishop Dec 2006, Aug 2007
    // Extending SpaceBook with a second implementation via a Portal

    // Abstraction  
    class Portal
    {
        Bridge bridge;
        public Portal(Bridge aSpaceBook)
        {
            bridge = aSpaceBook;
        }
        public void Add(string message)
        { 
            bridge.Add(message); 
        }
        public void Add(string friend, string message)
        { 
            bridge.Add(friend, message); 
        }
        public void Poke(string who)
        {
            bridge.Poke(who); 
        }
        public void MakeADeal()
        {
            //bridge.MakeADeal();
            Console.WriteLine("Added method to portal, but not to bridge, so i dont have to impleemnt it everwhere.");
        }
    }

    //Bridge
    interface Bridge
    {
        void Add(string message);
        void Add(string friend, string message);
        void Poke(string who);
        
        //void MakeADeal(); //needs change in every bridge implementation
    }

    class SpaceBookSystem
    {

        // The Subject
        private class SpaceBook
        {
            static SortedList<string, SpaceBook> community =
                new SortedList<string, SpaceBook>(100);
            string pages;
            string name;
            string gap = "\n\t\t\t\t";

            static public bool IsUnique(string name)
            {
                return community.ContainsKey(name);
            }

            internal SpaceBook(string n)
            {
                name = n;
                community[n] = this;
            }

            internal string Add(string s)
            {
                pages += gap + s;
                return gap + "======== " + name + "'s SpaceBook =========" +
                          pages + "\n" +
                        gap + "================================";
            }

            internal string Add(string friend, string message)
            {
                return community[friend].Add(message);
            }

            internal void Poke(string who, string friend)
            {
                community[who].pages += gap + friend + " poked you";
            }
            internal void MakeADeal()
            {
                Console.WriteLine("MakeADeal in SpaceBook class");
            }
        }


        // The Proxy (CANNOT CHANGE - we made a deal)
        public class MySpaceBook// : Bridge
        {
            // Combination of a virtual and authentication proxy
            SpaceBook mySpaceBook;
            string password;
            string name;
            bool loggedIn = false;



            void Register()
            {
                Console.WriteLine("Let's register you for SpaceBook");
                do
                {
                    Console.WriteLine("All SpaceBook names must be unique");
                    Console.Write("Type in a user name: ");
                    name = Console.ReadLine();
                } while (SpaceBook.IsUnique(name));
                Console.Write("Type in a password: ");
                password = Console.ReadLine();
                Console.WriteLine("Thanks for registering with SpaceBook");
            }

            bool Authenticate()
            {
                Console.Write("Welcome " + name + ". Please type in your password: ");
                string supplied = Console.ReadLine();
                if (supplied == password)
                {
                    loggedIn = true;
                    Console.WriteLine("Logged into SpaceBook");
                    if (mySpaceBook == null)
                        mySpaceBook = new SpaceBook(name);
                    return true;
                }
                Console.WriteLine("Incorrect password");
                return false;
            }

            public void Add(string message)
            {
                Check();
                Console.WriteLine("Added to my wall through mySpaceBook");
                if (loggedIn) mySpaceBook.Add(message);
            }

            public void Add(string friend, string message)
            {
                Check();
                Console.WriteLine("Sent to wall through mySpaceBook");
                if (loggedIn)
                    Console.WriteLine(mySpaceBook.Add(friend, name + " said: " + message));
            }
            //public void MakeADeal()
            //{
            //    Check();
            //    Console.WriteLine("Inside MySpaceBook : Bridge calling SpaceBook.MakeADeal");
            //    mySpaceBook.MakeADeal();
            //}

            public void Poke(string who)
            {
                Check();
                Console.WriteLine("Poked through mySpaceBook");
                if (loggedIn)
                    mySpaceBook.Poke(who, name);
            }

            void Check()
            {
                if (!loggedIn)
                {
                    if (password == null)
                        Register();
                    if (mySpaceBook == null)
                        Authenticate();
                }
            }
        }

        // A Proxy with little to do
        // Illustrates an alternative implementation in the Bridge pattern
        public class MyOpenBook : Bridge
        {
            // Combination of a virtual and authentication proxy
            SpaceBook myOpenBook;
            string name;
            static int users;

            public MyOpenBook(string n)
            {
                name = n;
                users++;
                myOpenBook = new SpaceBook(name + "-" + users);
            }

            public void Add(string message)
            {
                Console.WriteLine(myOpenBook.Add(message));
            }

            public void Add(string friend, string message)
            {
                Console.WriteLine(myOpenBook.Add(friend, name + " : " + message));
            }

            public void Poke(string who)
            {
                myOpenBook.Poke(who, name);
            }
            public void MakeADeal()
            {
                Console.WriteLine("I made a deal with developers of SpaceBook (function: MakeADeal() in myOpenBook)");
                Console.WriteLine("If I call this function just as is, won't work. In fact I can't even access this from client");
                
                Console.WriteLine("Now you cal call this. Calling SpaceBook.MakeADeal()");
                myOpenBook.MakeADeal();
            }
            //compiler accept this, but how do we call this
            //bcause in the main  program tom is referenced to Portal and SuperPoke is not in Portal
            //solutions:
            //1 add new operation to Portal, but not to Bridge, tako da ne treba svako implementirat
            //2 cannot update Portal, create extension method to extend it
            public void SuperPoke(string who, string what)
            {
                myOpenBook.Add(who, what + " you");
            }
        }
        public class ExtendingSpaceSystemWithAnotherBridgeImplementations : Bridge
        {
            //TODO:
            //want to implement new behaviour of add

            //TODO:
            //want to be able to use existing implementations in Portal or SpaceBook
            public void Add(string message)
            {
                throw new NotImplementedException();
            }

            public void Add(string friend, string message)
            {
                throw new NotImplementedException();
            }

            public void Poke(string who)
            {
                throw new NotImplementedException();
            }
        }
    } // end class SpaceBookSystem

    static class OpenBookExtensions
    {
        public static void SuperPoke(this Portal me, string who, string what)
        {
            me.Add(who, what + " you");
        }
    }

    // The Client
    class BridgePattern : SpaceBookSystem
    {
        static void Main()
        {
            Portal me = new Portal(new MyOpenBook("Judith"));
            me.Add("Hello world");
            me.Add("Today I worked 18 hours");
            
            

            Portal tom = new Portal(new MyOpenBook("Tom"));
            tom.Poke("Judith-1");
            tom.SuperPoke("Judith-1", "hugged");
            tom.Add("Judith-1", "Poor you");
            tom.Add("Hey, I'm on OpenBook - it was so easy!");

            Portal test = new Portal(new MyOpenBook("Test"));
            test.MakeADeal();
            
            //Portal test2 = new Portal(new MySpaceBook());
            //test2.MakeADeal();

            Portal extended = new Portal(new ExtendingSpaceSystemWithAnotherBridgeImplementations());
            extended.MakeADeal();

            Console.ReadKey();
        }
    }
    /* Output
    Let's register you for SpaceBook
    All SpaceBook names must be unique
    Type in a user name: Judith
    Type in a password: yey
    Thanks for registering with SpaceBook
    Welcome Judith. Please type in your password: yey
    Logged into SpaceBook

            ======== Judith's SpaceBook =========
            Hello world
            Today I worked 18 hours
            Tom poked you
            Tom : hugged you

            ================================

            ======== Judith's SpaceBook =========
            Hello world
            Today I worked 18 hours
            Tom poked you
            Tom : hugged you
            Tom : Poor you

            ================================

            ======== Tom-1's SpaceBook =========
            Hey, I'm on OpenBook - it was so easy!

            ================================
    */
}
