using System;
using System.Collections.Generic;


namespace proxy_exercise03
{
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
            List<string> myFriends = new List<string>();
            List<string> pendingFriends = new List<string>();
            static public bool IsUnique(string name)
            {
                return community.ContainsKey(name);
            }
            public bool AreWeFriends(string nameToCheck)
            {
                return myFriends.Contains(nameToCheck);
            }

            internal SpaceBook(string n)
            {
                name = n;
                community[n] = this;
            }

            internal void Add(string s)
            {
                pages += gap + s;
                Console.Write(gap + "======== " + name + "'s SpaceBook =========");
                Console.Write(pages);
                Console.WriteLine(gap + "===================================");
            }

            internal void Add(string friend, string message)
            {
                community[friend].Add(message);
            }

            internal void Poke(string who, string friend)
            {
                community[who].pages += gap + friend + " poked you";
            }

            internal bool SendFriendRequest(string whoToAdd, string me) //judith,tom
            {
                
                if (whoToAdd != null && !myFriends.Contains(whoToAdd))
                {
                    community[whoToAdd].pages += gap + me + " sent you a friend request";
                    
                    community[whoToAdd].pendingFriends.Add(me);
                    
                    return false;
                }
                return false;
            }
            internal bool AcceptFriendRequests(string inputed)
            {

                community[name].Add("");

                if (pendingFriends.Contains(inputed))
                {
                    community[name].pages += gap + "You have become friend with " + inputed;
                    community[inputed].pages += gap + "You have become friend with " + name;
                    myFriends.Add(inputed);
                    community[inputed].myFriends.Add(name);
                    pendingFriends.Remove(inputed);
                    community[name].Add("");
                    return true;
                }
                return false;
            }
        }

        // The Proxy
        public class MySpaceBook
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
                if (loggedIn) mySpaceBook.Add(message);
            }

            public void Add(string friend, string message)
            {
                Check();
                if (loggedIn)
                    mySpaceBook.Add(friend, name + " said: " + message);
            }

            public void Poke(string who)
            {
                Check();
                
                if (loggedIn)
                {
                    if (mySpaceBook.AreWeFriends(who))
                        mySpaceBook.Poke(who, name);
                    else
                        mySpaceBook.Add(who + " is not your friend yet");
                }
            }

            void Check()
            {
                if (!loggedIn)
                    if (password == null)
                        Register();
                if (mySpaceBook == null)
                    Authenticate();
            }


            public bool AddFriend(string whoToAdd)
            {
                if (mySpaceBook.SendFriendRequest(whoToAdd, name))
                    return true;
                return false;
            }
            public void AcceptRequests()
            {
                Console.WriteLine("Type in name of potential friend and press Enter or type esc and press Enter to exit");


                var inputed = Console.ReadLine();

                mySpaceBook.AcceptFriendRequests(inputed);
            }
        }
    }

    // The Client
    class ProxyPattern : SpaceBookSystem
    {
        static void Main()
        {
            MySpaceBook me = new MySpaceBook();
            me.Add("Hello world");
            me.Add("Today I worked 18 hours");

            MySpaceBook tom = new MySpaceBook();
            tom.Poke("Judith"); //will NOT pass
            tom.AddFriend("Judith");
            me.AcceptRequests(); // accepts as judith
            tom.Poke("Judith"); // will pass

            tom.Add("Judith", "Poor you");
            tom.Add("Off to see the Lion King tonight");
            Console.ReadKey();
        }
    }

}
