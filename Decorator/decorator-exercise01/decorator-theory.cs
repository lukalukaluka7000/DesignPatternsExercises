using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace decorator_exercise01
{
    interface IComponent
    {
        string Operation();
    }

    class Component : IComponent
    {
        public string Operation()
        {
            return "I am walking ";
        }
    }

    class DecoratorA : IComponent
    {
        IComponent component;

        public DecoratorA(IComponent c)
        {
            component = c;
        }

        public string Operation()
        {
            string s = component.Operation();
            s += "and listening to Classic FM ";
            return s;
        }
    }

    class DecoratorB : IComponent
    {
        IComponent component;
        public string addedState = "past the Coffee Shop ";

        public DecoratorB(IComponent c)
        {
            component = c;
        }

        public string Operation()
        {
            string s = component.Operation();
            s += "to school ";
            return s;
        }

        public string AddedBehavior()
        {
            return "and I bought a cappucino ";
        }
    }
    class DecoratorC : IComponent
    {
        IComponent component;
        public DecoratorC(IComponent comp)
        {
            component = comp;
        }
        public string Operation()
        {
            string ComponentMainText = component.Operation();
            ComponentMainText += " plus DecoratorC is awesome";
            return ComponentMainText;
        }
    }
    class StreamTrackbarDecorator : Stream
    {
        private readonly Stream _bstream;
        public StreamTrackbarDecorator(BufferedStream bStream)
        {
            _bstream = bStream;
        }
        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }
        public int ReadText()
        {
            byte[] ba = new byte[_bstream.Length];
            _bstream.Position = 0;
            //_bstream.Read(ba, 0, (int)_bstream.Length);

            long byteSize = 20 + (2 * _bstream.Length);
            int byteCount = 0, ctr = 0, byteCountCtr = 0;
            Console.WriteLine(byteSize);
            Console.Write("|");
            using (StreamReader sr = new StreamReader(_bstream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Console.Write(line);
                    byteCount = System.Text.ASCIIEncoding.Unicode.GetByteCount(line);
                    //Console.WriteLine(byteCount);
                    ctr += 1;
                    byteCountCtr += byteCount;
                    Console.Write(new string('-', (int)(byteCount/10)));
                    if(ctr % 10 == 0)
                    {
                        Console.Write(byteCountCtr + "/" + byteSize);
                    }
                    Thread.Sleep(200);
                }
                Console.Write("|");
            }
            //foreach (byte b in ba)
            //{
            //    Console.Write("{0,-3}", Convert.ToChar(b));
            //}
            return 0;
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for(int i = 0; i < 100; i++)
            {
                _bstream.WriteByte(5);
            }
        }
    }

    class ConsoleDecorator : IComponent //better solution ?
    {
        public string Operation()
        {
            throw new NotImplementedException();
        }
        public static void Write(string TextToProcess, int size)
        {
            int br = 0;
            string tempText = TextToProcess;
            while(TextToProcess.Length > br + size)
            {
                Console.WriteLine(tempText.Substring(0, size));
                tempText = tempText.Substring(size, tempText.Length - size );
                br += size;
            }
            Console.WriteLine(tempText);
        }
        public static void WriteLine(string TextToProcess, int size)
        {
            Write(TextToProcess, size);
        }
    }
    class Client
    { 
        static void Display(string s, IComponent c)
        {
            ConsoleDecorator.WriteLine(s + c.Operation(), 40);
        }

        static void Main()
        {
            //Console.WriteLine("Decorator Pattern\n");

            //IComponent component = new Component();
            //Display("1. Basic component: ", component);
            //Display("2. A-decorated : ", new DecoratorA(component));
            //Display("3. B-decorated : ", new DecoratorB(component));
            //Display("4. B-A-decorated : ", new DecoratorB(
            //     new DecoratorA(component)));
            //// Explicit DecoratorB
            //DecoratorB b = new DecoratorB(new Component());
            //Display("5. A-B-decorated : ", new DecoratorA(b));
            ////Invoking its added state and added behaviour
            //Console.WriteLine("\t\t\t" + b.addedState + b.AddedBehavior());

            //DecoratorC c = new DecoratorC(component);
            //Display("6. C-decorated : ", c);


            ////ConsoleDecorator.Write("ja sam manistra koja se neda dirati, razumijes li ti mene alo sta je bilo", 15);
            //ConsoleDecorator.WriteLine("SIZE 15 on ConsoleDecorator: ja sam manistra koja se neda dirati, razumijes li ti mene alo sta je bilo", 15);


            //Stream stream;
            //BufferedStream bf; 
            ////FileStream fs;
            //MemoryStream ms;
            //System.Net.Sockets.NetworkStream ns;
            //System.Security.Cryptography.CryptoStream cs;

            FileStream fs = new FileStream("amen.txt", FileMode.Open, FileAccess.Read);
            StreamTrackbarDecorator std = new StreamTrackbarDecorator(new BufferedStream(fs));
            std.ReadText();

            Console.ReadKey();
        }
    }

    /* Output:
    Decorator Pattern

    1. Basic component: I am walking 
    2. A-decorated : I am walking and listening to Classic FM 
    3. B-decorated : I am walking to school 
    4. B-A-decorated : I am walking and listening to Classic FM to school 
    5. A-B-decorated : I am walking to school and listening to Classic FM 
          past the Coffee Shop and I bought a cappucino 
    */
}
