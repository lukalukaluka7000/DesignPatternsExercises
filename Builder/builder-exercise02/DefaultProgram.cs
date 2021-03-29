using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace DefaultBuilderPattern
{
    //  Builder Pattern    D-J Miller and Judith Bishop Sept 2007
    // Abstract Factory : Builder Implementation

    interface IBuilder<Brand> where Brand : IBrand
    {
        IBag CreateBag();
    }

    // Abstract Factory now the Builder
    class Builder<Brand> : IBuilder<Brand> where Brand : IBrand, new()
    {
        Brand myBrand;
        public Builder()
        {
            myBrand = new Brand();
        }

        public IBag CreateBag()
        {
            return myBrand.CreateBag();
        }
    }

    // Product 1
    interface IBag
    {
        string Properties { get; set; }
    }

    // Concrete Product 1
    class Bag : IBag
    {
        public string Properties { get; set; }
    }

    // Directors
    interface IBrand
    {
        IBag CreateBag();
    }

    class Gucci : IBrand
    {
        public IBag CreateBag()
        {
            Bag b = new Bag();
            DefaultProgram.DoWork("Cut Leather", 250);
            DefaultProgram.DoWork("Sew leather", 1000);
            b.Properties += "Leather";
            DefaultProgram.DoWork("Create Lining", 500);
            DefaultProgram.DoWork("Attach Lining", 1000);
            b.Properties += " lined";
            DefaultProgram.DoWork("Add Label", 250);
            b.Properties += " with label";
            return b;
        }
    }

    class Poochy : IBrand
    {
        public IBag CreateBag()
        {
            Bag b = new Bag();
            DefaultProgram.DoWork("Hire cheap labour", 200);
            DefaultProgram.DoWork("Cut Plastic", 125);
            DefaultProgram.DoWork("Sew Plastic", 500);
            b.Properties += "Plastic";
            DefaultProgram.DoWork("Add Label", 100);
            b.Properties += " with label";
            return b;
        }
    }

    class Client<Brand> where Brand : IBrand, new()
    {
        public void ClientMain()
        { //IFactory<Brand> factory) 
            IBuilder<Brand> factory = new Builder<Brand>();

            DateTime date = DateTime.Now;
            Console.WriteLine("I want to buy a bag!");
            IBag bag = factory.CreateBag();

            Console.WriteLine("I got my Bag which took " + DateTime.Now.Subtract(date).TotalSeconds * 5 + " days");
            Console.WriteLine("  with the following properties " + bag.Properties + "\n");
        }
    }

    static class DefaultProgram
    {
        static void Main()
        {
            // Call Client twice
            new Client<Poochy>().ClientMain();
            new Client<Gucci>().ClientMain();

            Console.ReadKey();
        }

        public static void DoWork(string workitem, int time)
        {
            Console.Write("" + workitem + ": 0%");
            Thread.Sleep(time);
            Console.Write("....25%");
            Thread.Sleep(time);
            Console.Write("....50%");
            Thread.Sleep(time);
            Console.WriteLine("....100%");
        }
    }
}
/*Output
I want to buy a bag!
Hire cheap labour: 0%....25%....50%....100%
Cut Plastic: 0%....25%....50%....100%
Sew Plastic: 0%....25%....50%....100%
Add Label: 0%....25%....50%....100%
I got my Bag which took 14.02016 days
  with the following properties Plastic with label

I want to buy a bag!
Cut Leather: 0%....25%....50%....100%
Sew leather: 0%....25%....50%....100%
Create Lining: 0%....25%....50%....100%
Attach Lining: 0%....25%....50%....100%
Add Label: 0%....25%....50%....100%
I got my Bag which took 45.0648 days
  with the following properties Leather lined with label

*/
