using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Strategy
{

    // Strategy Pattern    by  Judith Bishop and D-J Miller Sept 2007
    // Gives a choice of sort routines to display
    // Algorithms and GUI adapted from a Java system at
    // http://www.geocities.com/SiliconValley/Network/1854/Sort1.html
    public enum TipPod
    {
        Big,
        Others
    };
    static class StartSetGenerator<T>
    {
        private static readonly string[] sampleNames = {  "abe", "aguilar", "abigail", "abel", "brenda", "abraham", "aaron", "abby", "adam", "adan", "abdul", "adkins", "adolfo", "adolph", "adrian",  "adele", "adrian", "abbott", "acosta", "adams" };
        private static readonly string surnameForAll = "Bušić";

        private static List<T> myList;
        public static TipPod tipPodatka = (TipPod)Enum.Parse(typeof(TipPod), "Others");

        // Check the Iterator Pattern for a different version
        public static IEnumerable<T> GetStartSet()
        {
            const int n = 200; // how many values to generate
            myList = new List<T>();
            int index = 0;
            if (typeof(T) == typeof(Person))
            {
                IList<Person> list = new List<Person>();
                foreach (string name in sampleNames)
                {
                    //return (T)(object)String.Empty;
                    list.Add(new Person() { RedniBroj = index+1, Name = name, Surname = surnameForAll});
                    index++;
                }
                myList = (List<T>)list;
            }
            
            tipPodatka = TipPod.Others;
            return myList;
        }
        
    }

    class StrategyView<T> : Form where T : IComparable<T>
    {
        Func<IEnumerable<T>> Generator;

        // Constructor to set up the GUI
        public StrategyView(Func<IEnumerable<T>> generator)
        {
            Generator = generator;

            this.SuspendLayout();
            this.Text = "Sort Comparer (press button and check console)";

            TableLayoutPanel p = new TableLayoutPanel();
            p.RowCount = 1;
            p.ColumnCount = 1;
            p.Dock = DockStyle.Top;
            this.Controls.Add(p);

            Button b = new Button();
            b.Name = "LargeItems";
            b.Click += new EventHandler(ButtonClick);
            b.Text = "Objects";
            p.Controls.Add(b);

            p.Height = b.Height + 4;
            this.DoubleBuffered = true;
            this.ResumeLayout(true);
        }

        public void DrawGraph(IEnumerable<T> list)
        {
            foreach(T v in list)
            {
                Console.Write(v.ToString() + " ");
            }
            Console.WriteLine();

            this.Refresh();
            Thread.Sleep(100);
            Application.DoEvents();
        }

        // Selecting the Strategy
        static SortStrategy<T> SelectStrategy(string name)
        {
            switch (name)
            {
                case "LargeItems": return new MergeSorter<T>();
                //case "SmallItems": return new QuickSorter<T>();
                //case "ReversedList": return new MergeSorter<T>();
                //case "PartiallySorted": return new MergeSorter<T>();
                //case "VeryBigData": return new MergeSorter<T>();
                default: return null;
            }
        }
        void SetGenerator(string typeOfData)
        {
            switch (typeOfData)
            {
                case "LargeItems"://return StartSetGenerator<int>.GetVeryBigData;
                    //Generator = StartSetGenerator<int>.GetVeryBigData;
                    Func<IEnumerable<T>> func = StartSetGenerator<T>.GetStartSet;
                    Generator = func;
                    StartSetGenerator<T>.tipPodatka = TipPod.Others;
                    return;
                //case "LargeItems":
                //    Func<IEnumerable<int>> func2 = StartSetGenerator.GetStartSet;
                //    Generator = (Func<IEnumerable<T>>)func2;
                //    StartSetGenerator.tipPodatka = TipPod.Others;
                //    return;

                default:
                    return;

            }
        }
        // The Context
        void ButtonClick(object sender, EventArgs e)
        {
            Button control = sender as Button;

            SetGenerator(control.Name);
            IEnumerable<T> newList = Generator();
            SortStrategy<T> strategy = SelectStrategy(control.Name);

            DrawGraph(newList);
            if (strategy == null)
                return;

            // DrawGraph will be invoked during sorting when the UpdateUI event is triggered
            strategy.UpdateUI += new Action<IEnumerable<T>>(DrawGraph);
            strategy.Sort(newList);
        }
    }

    // Strategy interface
    interface SortStrategy<T> where T : IComparable<T>
    {
        event Action<IEnumerable<T>> UpdateUI;
        void Sort(IEnumerable<T> input);
    }

    // Strategy 1
    class MergeSorter<T> : SortStrategy<T> where T : IComparable<T>
    {
        public event Action<IEnumerable<T>> UpdateUI;

        List<T> aux;
        int opCount = 0;
        public void Sort(IEnumerable<T> input)
        {
            opCount++;
            List<T> sorteditems = new List<T>(input);
            aux = new List<T>(sorteditems.Count);
            for (int i = 0; i < sorteditems.Count; i++)
                aux.Add(default(T));
            MergeSort(ref sorteditems, 0, sorteditems.Count - 1);
            UpdateUI(sorteditems);
        }

        private void Merge(ref List<T> a, int l, int m, int r)
        {
            int i;
            int j;

            for (i = m + 1; i > l; i--)
            {
                aux[i - 1] = a[i - 1];
                opCount++;
            }

            for (j = m; j < r; j++)
            {
                aux[r + m - j] = a[j + 1];
                opCount++;
            }

            for (int k = l; k <= r; k++)
            {
                // Less Than
                if (aux[j].CompareTo(aux[i]) == -1)
                {
                    a[k] = aux[j--];
                }
                else
                {
                    a[k] = aux[i++];
                }
                opCount++;
            }
        }

        private void MergeSort(ref List<T> a, int l, int r)
        {
            if (r <= l) return;
            int m = (r + l) / 2;
            MergeSort(ref a, l, m);
            // count every movement of elements
            if (opCount > 50)
            {
                opCount = opCount - 50;
            }
            MergeSort(ref a, m + 1, r);
            // count every movement of elements
            if (opCount > 50)
            {
                opCount = opCount - 50;
            }
            Merge(ref a, l, m, r);
            // count every movement of elements
            if (opCount > 50)
            {
                opCount = opCount - 50;
            }
        }
    }

    // Strategy 2
    class QuickSorter<T> : SortStrategy<T>
      where T : IComparable<T>
    {

        public event Action<IEnumerable<T>> UpdateUI;

        int opCount = 0;
        public void Sort(IEnumerable<T> input)
        {
            opCount++;
            List<T> sorteditems = new List<T>(input);

            QuickSort(ref sorteditems, 0, sorteditems.Count - 1);
            UpdateUI(sorteditems);
        }

        private int Partition(ref List<T> a, int l, int r)
        {
            T tmp;
            int i = l - 1;
            int j = r;
            T v = a[r]; // Partition point
            for (; ; )
            {
                // scan up to find first item greater than v
                // won't go past end because v = last item in array
                while (a[++i].CompareTo(v) == -1)
                {
                    opCount++;
                }
                // scan down down to find first item less than v
                // or quit if there are none
                while (v.CompareTo(a[--j]) == -1)
                {
                    opCount++;
                    if (j == l) break;
                }
                // if scan points cross, quit
                if (i >= j) break;

                // exchange the elements
                tmp = a[i];
                a[i] = a[j];
                a[j] = tmp;

                opCount++;
            }

            // final swap
            a[r] = a[i];
            a[i] = v;

            if (opCount > 50)
            {
                UpdateUI(a);
                opCount = opCount - 50;
            }
            return i;
        }


        private void QuickSort(ref List<T> a, int l, int r)
        {
            opCount++;
            if (r <= l) return;
            int i = Partition(ref a, l, r);
            QuickSort(ref a, l, i - 1);
            QuickSort(ref a, i + 1, r);
        }
    }

    static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StrategyView<Person>(StartSetGenerator<Person>.GetStartSet));
        }
    }
}
//ex02: thanks but no thanks
//ex03: input of each field using Strategy?! - will implement this when have undesranding of what they mean by this
// strategies of inputing email and password? or which to use?
