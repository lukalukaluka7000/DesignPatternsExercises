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

        #region First improvization (comment/uncomment)
        //class Dice : IEnumerable
        //{
        //    private int[] ThrowsList1;
        //    private int[] ThrowsList2;
        //    private int index = 0;
        //    private static readonly Random random = new Random();
        //    public Dice()
        //    {
        //        ThrowsList1 = new int[5];
        //        ThrowsList2 = new int[5];
        //    }

        //    private class MyEnumerator : IEnumerator
        //    {
        //        private int[] ThrowsList1;
        //        private int[] ThrowsList2;
        //        int position = -1;
        //        public MyEnumerator(int[] list1, int[] list2)
        //        {
        //            ThrowsList1 = list1;
        //            ThrowsList2 = list2;
        //        }
        //        private IEnumerator getEnumerator()
        //        {
        //            return (IEnumerator)this;
        //        }
        //        public bool MoveNext()
        //        {
        //            position++;
        //            return (position < ThrowsList1.Length || position < ThrowsList2.Length);
        //        }
        //        //IEnumerator
        //        public void Reset()
        //        {
        //            position = -1;
        //        }
        //        //IEnumerator
        //        public object Current
        //        {
        //            get
        //            {
        //                try
        //                {
        //                    return Tuple.Create(ThrowsList1[position], ThrowsList2[position]);
        //                }
        //                catch (IndexOutOfRangeException)
        //                {
        //                    throw new InvalidOperationException();
        //                }
        //            }
        //        }
        //    }
        //    public IEnumerator GetEnumerator()
        //    {
        //        return new MyEnumerator(ThrowsList1, ThrowsList2);
        //    }

        //    internal void ThrowDices()
        //    {
        //        int rndI = random.Next(1, 7);
        //        ThrowsList1[index] = rndI;

        //        rndI = random.Next(1, 7);
        //        ThrowsList2[index++] = rndI;
        //    }

        //}
        #endregion

        #region Second improvization (comment/uncomment)
        public interface IAggregate
        {
            int Count { get;}
            IMyIterator GetIterator(Random rnd);
            int this[int itemIndex] { get;set; }
            int NumberOfThrows { get; }
        }
        public interface IMyIterator
        {
            int CurrentItem { get; }
            bool IsDone { get; }
            int NextItem { get; }
            int FirstItem { get; }
            
        }
        public class Aggregate : IAggregate
        {
            private List<int> _values = null;
            public int Count => _values.Count;
            public int NumberOfThrows { get; }

            public Aggregate(int numberOFThrows)
            {
                NumberOfThrows = numberOFThrows;
                _values = new List<int>(NumberOfThrows);
            }
            public IMyIterator GetIterator(Random rnd)
            {
                return new MyIterator(this, rnd);
            }
            public int this[int itemIndex]
            {
                get
                {
                    if (itemIndex < _values.Capacity)
                    {
                        return _values[itemIndex];
                    }
                    else
                    {
                        return int.MinValue;
                    }
                }
                set
                {
                    _values.Add(value);
                }
            }
        }
        public class MyIterator : IMyIterator
        {
            private IAggregate _collection = null;
            private int _currentIndex = 0;
            private Random _rnd;
            public MyIterator(IAggregate collection, Random Rnd)
            {
                _collection = collection; _rnd = Rnd;
            }

            public int CurrentItem
            {
                get
                {
                    return _collection[_currentIndex];
                }
            }
            public bool IsDone
            {
                get
                {
                    if (_currentIndex == _collection.NumberOfThrows)
                        return true;
                    return false;
                    //if (_currentIndex > _collection.Count -1) 
                    //    return true;
                    //return false;
                }
            }

            public int NextItem
            {
                get
                {
                    int rInt = _rnd.Next(1, 7);
                    _collection[_currentIndex] = rInt;
                    return _collection[_currentIndex++];
                }
            }
            public int FirstItem
            {
                get
                {
                    _currentIndex = 0;
                    return _collection[_currentIndex];
                }
            }
            
        }
        #endregion
        static void Main(string[] args)
        {
            #region First improvization (comment/uncomment)
            //Console.WriteLine("you have 5 tries");

            //Dice dice = new Dice();
            //dice.ThrowDices();
            //dice.ThrowDices();
            //dice.ThrowDices();
            //dice.ThrowDices();
            //dice.ThrowDices();


            //foreach (var d in dice)
            //{
            //    Console.WriteLine(d);
            //}
            #endregion

            #region Second improvization (comment/uncomment)
            int numberOfThrows = 5;
            IAggregate clientCollectionObject = new Aggregate(numberOfThrows);
            IAggregate clientCollectionObjectOtherDice = new Aggregate(numberOfThrows);

            //IMyIterator clientIteratorDice1 = new MyIterator(clientCollectionObject, random);
            //IMyIterator clientIteratorDice2 = new MyIterator(clientCollectionObjectOtherDice, random);
            Random rnd = new Random();
            IMyIterator clientIteratorDice1 = clientCollectionObject.GetIterator(rnd);
            IMyIterator clientIteratorDice2 = clientCollectionObjectOtherDice.GetIterator(rnd);
            int broj = 0;
            for(; clientIteratorDice1.IsDone == false; ){
                broj = clientIteratorDice1.NextItem;
                Console.WriteLine(broj);
            }
            Console.WriteLine();
            for (broj=0; clientIteratorDice2.IsDone == false;)
            {
                broj = clientIteratorDice2.NextItem;
                Console.WriteLine(broj);
            }
            #endregion

            Console.ReadKey();
        }
    }
}
