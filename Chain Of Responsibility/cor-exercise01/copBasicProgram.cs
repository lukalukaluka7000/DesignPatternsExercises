using System;
namespace Basic
{
    class ChainofResponsibilityPattern
    {

        // Chain of responsibility pattern       Judith Bishop June 2007

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
                else return ("Request for " + data + " handled BY DEFAULT at level " + id);
            }
        }
        static class Basic
        {
            static void Main()
            {

                Handler start = null;
                for (int i = 5; i > 0; i--)
                {
                    Console.WriteLine("Handler " + i + " deals up to a limit of " + i * 1000);
                    start = new Handler(i, start);
                }

                int[] a = { 50, 2000, 1500, 10000, 175, 4500 };
                foreach (int i in a)
                    Console.WriteLine(start.HandleRequest(i));
                Console.ReadKey();
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