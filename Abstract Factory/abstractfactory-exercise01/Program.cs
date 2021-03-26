using System;

namespace AbstractFactoryPattern
{
    //  Abstract Factory        D-J Miller and Judith Bishop Sept 2007
    //  Uses generics to simplify the creation of factories

    interface IFactory<T> where T : IBrand
    {
        IBag CreateBag();
        IShoes CreateShoes();
        IBelts CreateBelts();
    }

    // Conctete Factories (both in the same one)
    class Factory<T> : IFactory<T> where T : IBrand, new()
    {
        public IBag CreateBag() {
            return new Bag<T>();
        }

        public IShoes CreateShoes() {
            return new Shoes<T>();
        }
        public IBelts CreateBelts()
        {
            return new Belts<T>();
        }
    }

    // Product 1
    interface IBag{
        string Material { get; }
    }
    // Concrete Product 1
    class Bag<T> : IBag where T : IBrand, new()
    {
        private T myBrand;
        public Bag() {
            myBrand = new T();
        }
        public string Material { get { return myBrand.Material; } }
    }


    // Product 2
    interface IShoes {
        int Price { get; }
    }
    // Concrete Product 2
    class Shoes<T> : IShoes where T : IBrand, new()
    {
        private T myBrand;
        public Shoes() {
            myBrand = new T();
        }
        public int Price { get { return myBrand.Price; } }
    }

    // added Proudct 3
    interface IBelts
    {
        int Length { get; }
    }
    //concrete product 3
    class Belts<T> : IBelts where T : IBrand, new()
    {
        private readonly T myBrand;
        public Belts()
        {
            myBrand = new T();
        }
        public int Length { get { return myBrand.Length; } }
    }


    interface IBrand
    {
        int Price { get; }
        string Material { get; }
        int Length { get; }
    }

    class Gucci : IBrand
    {
        public int Price { get { return 1000; } }
        public string Material { get { return "Crocodile skin"; } }
        public int Length { get { return 46; } }
    }

    class Poochy : IBrand
    {
        public int Price { get { return new Gucci().Price / 3; } }
        public string Material { get { return "Plastic"; } }
        public int Length { get { return 54; } }
    }

    class Groundcover : IBrand
    {
        public int Price { get { return 2000; } }
        public string Material { get { return "South african leather"; } }
        public int Length { get { return 47; } }
    }

    class Client<T> where T : IBrand, new()
    {
        public void ClientMain()
        { //IFactory<Brand> factory)
            IFactory<T> factory = new Factory<T>();

            IBag bag = factory.CreateBag();
            IShoes shoes = factory.CreateShoes();
            IBelts belts = factory.CreateBelts();

            Console.WriteLine("I bought a Bag which is made from " + bag.Material);
            Console.WriteLine("I bought some shoes which cost " + shoes.Price);
            Console.WriteLine("I bought a belt with length " + belts.Length);
        }
    }

    static class Program
    {
        static void Main()
        {
            // Call Client twice
            new Client<Poochy>().ClientMain();
            new Client<Gucci>().ClientMain();
            new Client<Groundcover>().ClientMain();

            Console.ReadKey();
        }
    }
}
/* Output
I bought a Bag which is made from Plastic
I bought some shoes which cost 333
I bought a Bag which is made from Crocodile skin
I bought some shoes which cost 1000
I bought a Bag which is made from South african leather
I bought some shoes which cost 2000
*/
/*
 Exerise 02: Avocados would become candidate for AFP when there would be some more products like Grape, Asparagus etc.
Generic type like Brand in this one would be maybe country or city hwere it originates from (Asparagus from different countries are different sizes, right ? :))
There is exercise on page 114 where they say to implement them as prototype and extend Factory method in that way
That is on my todo list but this i dont feel like implemneting right now...
Exercise 03: Why exactly would i break this beautifull generic implementation :) ? Maybe another time...
 */
