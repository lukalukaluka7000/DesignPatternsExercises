using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CarConstrutingPattern
{
    public interface IDirector
    {
        Product Construct(IBuilder builder);
    }

    class Director : IDirector
    {
        public Product Construct(IBuilder builder)
        {
            DateTime start = DateTime.Now;
            //order matters, you dont wanna paint car first :)
            builder.InstallAirConditioning();
            builder.WheelUpgrade();
            
            builder.Upholstering();
            builder.PaintFinish();

            var finishedProdcut  = builder.HeyBuilderGiveMeWhatYouGot();
            Console.WriteLine("\nBuilder took " + Math.Round(DateTime.Now.Subtract(start).TotalSeconds * 5, 2) + " days to finish" +
                " working on " + finishedProdcut.NameOfProduct);

            return finishedProdcut;
        }
    }
    public interface IBuilder
    {
        void PaintFinish();
        void Upholstering();
        void InstallAirConditioning();
        void WheelUpgrade();
        Product HeyBuilderGiveMeWhatYouGot();
    }
    public class EconomyBuilder : IBuilder
    {
        private Product WeBuiltThisModel = new Product("Economy Model");
        public void InstallAirConditioning()
        {
            WeBuiltThisModel.Add("ManualAC", 600);
        }

        public void PaintFinish()
        {
            WeBuiltThisModel.Add("BasicColor", 400); //todo: neki enum novi klass koji ce imat timeCOnsuming i u addu se to kompenzira
        }

        public void Upholstering()
        {
            WeBuiltThisModel.Add("Cloth", 500);
        }

        public void WheelUpgrade()
        {
            WeBuiltThisModel.Add("Steel Wheels with plastic hubcaps", 1000);
        }
        public Product HeyBuilderGiveMeWhatYouGot()
        {
            return WeBuiltThisModel;
        }
    }
    public class MediumBuilder : IBuilder
    {
        private Product WeBuiltThisModel = new Product("Medium Model");
        public void InstallAirConditioning()
        {
            WeBuiltThisModel.Add("AutomaticAC", 2500);
        }

        public void PaintFinish()
        {
            WeBuiltThisModel.Add("Metallic", 800);
        }

        public void Upholstering()
        {
            WeBuiltThisModel.Add("Leather", 600);
        }

        public void WheelUpgrade()
        {
            WeBuiltThisModel.Add("Alloys", 1100);
        }
        public Product HeyBuilderGiveMeWhatYouGot()
        {
            return WeBuiltThisModel;
        }
    }
    public class LuxuryBuilder : IBuilder
    {
        private Product WeBuiltThisModel = new Product("Luxury Model");
        public Product HeyBuilderGiveMeWhatYouGot()
        {
            return WeBuiltThisModel;
        }

        public void InstallAirConditioning()
        {
            WeBuiltThisModel.Add("Automatic With Seperate Zones AC", 4700);
        }

        public void PaintFinish()
        {
            WeBuiltThisModel.Add("Pearl Color With Matte Finish", 1900);
        }

        public void Upholstering()
        {
            WeBuiltThisModel.Add("Alcantara", 600);
        }

        public void WheelUpgrade()
        {
            WeBuiltThisModel.Add("Magnesium Alloy", 1200);
        }
    }

    public class Product
    {
        private bool StartedOnProduct = false;
        public string NameOfProduct = "";
        private List<string> parts { get; set; } = new List<string>();
        public Product(string nop)
        {
            NameOfProduct = nop;
        }
        internal void Add(string feature, int timeConsuming)
        {
            DateTime startedOnFeature = DateTime.Now;
            if (!StartedOnProduct)
            {
                Console.WriteLine("\n\n-----------------------------------------------\nStarting to work on product " + NameOfProduct);
                StartedOnProduct = true;
            }
            //todo get action from name of function -> reflection
            Console.Write("\n" + feature + " installing: 0%");
            Thread.Sleep(timeConsuming);
            Console.Write("....25%");
            Thread.Sleep(timeConsuming);
            Console.Write("....50%");
            Thread.Sleep(timeConsuming);
            Console.Write("....75%");
            Thread.Sleep(timeConsuming);
            Console.WriteLine("...100%");
            parts.Add(feature);
            Console.WriteLine("Finished working on " + feature + " (days took: " + 
                Math.Round(DateTime.Now.Subtract(startedOnFeature).TotalSeconds* 5,1)  + " days)" );
        }
    }
    class Client
    {
        public void ClientMain()
        {
            IDirector director = new Director();

            //direktor has a builder
            var economyCar = director.Construct(new EconomyBuilder());
            var mediumCar = director.Construct(new MediumBuilder());
            var luxuryCar = director.Construct(new LuxuryBuilder());

            Console.ReadKey();
        }
    }

    

    static class CarConstructProgram
    {
        static void Main()
        {
            Console.WriteLine("Car models constructing...");
            new Client().ClientMain();
            Console.ReadKey();
        }

    }
}

