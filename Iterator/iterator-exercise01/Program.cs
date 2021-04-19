using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iterator_exercise01
{
    class Directory
    {
        //svaku directory ce imat listu fileova, ali svaki dir moze imat jos dirova
        //e al provjera jeli ima dirove ako nema, to znaci da nema ni djece

        public string FolderName { get; set; }
        public IEnumerable<File> Files{ get; set; }
        Directory() { }
        public Directory(string fn, IEnumerable<File> files)
        {
            FolderName = fn; Files = files;
        }
        public override string ToString()
        {
            var folder = String.Format(FolderName + ":");
            string temp = "";
            foreach(var v in Files)
            {
                temp += String.Format(v.ToString());
            }
            return "Folder " + folder + temp + "\n";
        }

    }
    class File
    {
        public string FileName { get; set; }
        public int KbSize { get; set; }
        public string Format { get; set; }
        public File() {}
        public File(string fn, int kbsize, string format)
        {
            FileName = fn; KbSize = kbsize; Format = format;
        }
        public override string ToString()
        {
            return String.Format("\n"+FileName + "." + Format + " ({0})", KbSize);
        }
    }
    class Node<T>
    {
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public T DirectoryData { get; set; }
        public Node(T directoryData, Node<T> left, Node<T> right)
        {
            DirectoryData = directoryData; Left = left; Right = right;
        }
    }
    class Tree<T>
    {
        Node<T> root;
        public Tree() { }
        public Tree(Node<T> head) { root = head; }

        public IEnumerable<T> PreOrder // root L R
        {
            get
            {
                return ScanPreorder(root);
            }
        }
        private IEnumerable<T> ScanPreorder(Node<T> root)
        {
            yield return root.DirectoryData;
            if(root.Left != null)
            {
                foreach (T node in ScanPreorder(root.Left))
                    yield return node;
            }
            if(root.Right != null)
            {
                foreach (T node in ScanPreorder(root.Right))
                    yield return node;
            }
        }


        public IEnumerable<T> InOrder // L root R
        {
            get
            {
                return ScanInOrder(root);
            }
        }
        private IEnumerable<T> ScanInOrder(Node<T> root)
        {
            
            if (root.Left != null)
            {
                foreach (T node in ScanInOrder(root.Left))
                    yield return node;
            }

            yield return root.DirectoryData;
            
            if (root.Right != null)
            {
                foreach (T node in ScanInOrder(root.Right))
                    yield return node;
            }
        }
    }
    class IteratorPattern01
    {
        //COnsider the file directory illustration. Problem is almost same as family tree. 
        //Realize data from figure 9-2 in the form of the initializer given for the family tree.
        //Then rework client program to get print of various views of hierarchy (Node and Tree you can use)
        //        Patterns book
        //              .      .    ..
        //              .       .       ..
        //              .        .          ..
        //     ChaptersJuly   ChaptersJune   ChaptersSeptember
        //                                  .
        //                                 .
        //                                .
        //            Images Odds  Programs UmlDiagrams  Appendix.doc... 
        static void Main(string[] args)
        {
            //confusing definition of left and right but ok
            var directories = new Tree<Directory>(new Node<Directory>
                (new Directory("Patterns book", new List<File>()), //data
                    new Node<Directory> // left
                        (new Directory("Chapters July", new List<File>()), null,
                        new Node<Directory>
                        (
                            new Directory("Chapters June", new List<File>()), null,
                            new Node<Directory>(new Directory("Chapters September", new List<File>() {
                            new File("Appendix", 68, "doc"),
                            new File("CSDP Figs 2-7", 1800, "zip"),
                            new File("CSDPChap2Aug07", 788, "doc"),
                            new File("CSDPChap3Sep07", 848, "doc"),
                            new File("CSDPChap4Sept07", 484, "doc"),
                            new File("CSDPChap5Sept07", 440, "doc"),
                            new File("CSDPChap6Sept07", 208, "doc"),
                            new File("CSDPChap7Sept07", 272, "doc"),
                            new File("CSDPChap8Sept07", 536, "doc"),
                            new File("CSDPChap9Sep07", 36, "doc"),
                            new File("Fronts", 44, "doc")
                        }), new Node<Directory>(new Directory("Images", new List<File>()), null,
                        new Node<Directory>(new Directory("Odds", new List<File>()), null,
                        new Node<Directory>(new Directory("Programs", new List<File>()), null,
                        new Node<Directory>(new Directory("UmlDiagrams", new List<File>()), null,
                        new Node<Directory>(new Directory("VPProjects", new List<File>()), null, null)
                        )))), null))
                        ), null
                    )

            );
            //printout may differ from expected cause this is not strictly binary tree
            Console.WriteLine("Preorder ( root L R ):");
            foreach (Directory d in directories.PreOrder)
                Console.WriteLine(d);
            Console.WriteLine("\nInorder ( L root R ):");
            foreach (Directory d in directories.InOrder)
                Console.WriteLine(d);
     
            Console.ReadKey();
        }
    }
}
