using System;

namespace Strategy
{
    //public interface IPerson<T> where T : IComparable<T>
    //{
    //    int RedniBroj { get; set; }
    //    string Name { get; set; }
    //    string Surname { get; set; }

    //}
    //interface SortStrategy<T> where T : IComparable<T>
    //{
    //    event Action<IEnumerable<T>> UpdateUI;
    //    void Sort(IEnumerable<T> input);
    //}

    //// Strategy 1
    //class MergeSorter<T> : SortStrategy<T> where T : IComparable<T>
    //{
    public class Person : IComparable<Person>
    {
        public int RedniBroj { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }


        public int CompareTo(Person other)
        {
            return Name.CompareTo(other.Name);
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}