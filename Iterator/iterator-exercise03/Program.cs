using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iterator_exercise03
{
    class Program
    {
        class Dice : IEnumerable
        {
            private int[] ThrowsList1;
            private int[] ThrowsList2;
            private int index = 0;
            private static readonly Random random = new Random();
            public Dice()
            {
                ThrowsList1 = new int[5];
                ThrowsList2 = new int[5];
            }

            private class MyEnumerator : IEnumerator
            {
                private int[] ThrowsList1;
                private int[] ThrowsList2;
                int position = -1;
                public MyEnumerator(int[] list1, int[] list2)
                {
                    ThrowsList1 = list1;
                    ThrowsList2 = list2;
                }
                private IEnumerator getEnumerator()
                {
                    return (IEnumerator)this;
                }
                public bool MoveNext()
                {
                    position++;
                    return (position < ThrowsList1.Length || position < ThrowsList2.Length);
                }
                //IEnumerator
                public void Reset()
                {
                    position = -1;
                }
                //IEnumerator
                public object Current
                {
                    get
                    {
                        try
                        {
                            return Tuple.Create(ThrowsList1[position], ThrowsList2[position]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
            }
            public IEnumerator GetEnumerator()
            {
                return new MyEnumerator(ThrowsList1, ThrowsList2);
            }

            internal void ThrowDices()
            {
                int rndI = random.Next(1, 7);
                ThrowsList1[index] = rndI;

                rndI = random.Next(1, 7);
                ThrowsList2[index++] = rndI;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("you have 5 tries");

            Dice dice = new Dice();
            dice.ThrowDices();
            dice.ThrowDices();
            dice.ThrowDices();
            dice.ThrowDices();
            dice.ThrowDices();


            foreach (var d in dice)
            {
                Console.WriteLine(d);
            }
   

            Console.ReadKey();
        }
    }
}
