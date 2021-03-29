using System;
namespace Excep
{
    class ChainwithStatePatternE
    {

        // Chain of responsibility pattern       Judith Bishop June 2007
        // Sets up the Handlers in a simple linked structure
        // Makes use of a user defined exception if the end of the chain is reached

        class Handler
        {
            Handler next;
            int id;
            public int Limit { get; set; }
            public Handler(int id, Handler handler)
            {
                this.id = id;
                Limit = id * 1000;
                next = handler;
            }

            public string HandleRequest(int data)
            {
                if (data < Limit)
                    return "Request for " + data + " handled at level " + id;
                else if (next != null)
                    return next.HandleRequest(data);
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
        static class WithException
        {
            static void Main()
            {

                Handler start = null;
                for (int i = 5; i > 0; i--)
                {
                    Console.WriteLine("Handler " + i + " deals up to a limit of " + i * 1000);
                    start = new Handler(i, start);
                }
                Console.WriteLine();

                int[] a = { 50, 2000, 1500, 10000, 175, 4500 };
                foreach (int i in a)
                {
                    try
                    {
                        Console.WriteLine(start.HandleRequest(i));
                    }
                    catch (ChainException e)
                    {
                        Console.WriteLine("No facility to handle a request of " + e.Data["Limit"] +
                        "\nPlease break it down into smaller requests");
                    }
                }
            }
        }
    }
    /*
    Handler 5 deals up to a limit of 5000
    Handler 4 deals up to a limit of 4000
    Handler 3 deals up to a limit of 3000
    Handler 2 deals up to a limit of 2000
    Handler 1 deals up to a limit of 1000
    Request for 50 handled at level 1
    Request for 2000 handled at level 3
    Request for 1500 handled at level 2
    Request for 10000 handled BY DEFAULT at level 5
    Request for 175 handled at level 1
    Request for 4500 handled at level 5
    */

}