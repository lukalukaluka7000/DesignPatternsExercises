using System;
using System.Collections.Generic;

namespace CarConstrutingPattern
{
    public static class ProductCarHelper
    {
        public static void PrintProduct(this List<string> partsOfProduct)
        {
            foreach (var v in partsOfProduct)
                Console.WriteLine(v);
        }
    }
}