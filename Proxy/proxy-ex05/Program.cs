using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proxy_exercise05
{
    class StreamInterceptor
    {

        static class Log
        {
            public static void LogFile(string filename, string sMessage, string exceptionMessage = "")
            {
                StreamWriter log;
                if (!File.Exists(filename))
                {
                    log = new StreamWriter(filename);
                }
                else
                {
                    log = File.AppendText(filename);
                }

                // Write to the file:
                log.WriteLine("Filename:" + filename);
                log.WriteLine("Data Time:" + DateTime.Now);

                log.WriteLine("Message:" + sMessage);

                if (exceptionMessage != "")
                    log.WriteLine("Exception message: ", exceptionMessage);

                // Close the stream:
                log.Close();
            }
        }

        // The Proxy
        public class MyStream
        {
            // Intercepting Stream calls
            Stream stream;
            public MyStream()
            {
                stream = new FileStream("test.txt", FileMode.OpenOrCreate);
            }

            public int Read(byte[] buffer, int offset, int count)
            {
                Log.LogFile("log.txt", "Executed Stream.Read() function");
                return stream.Read(buffer, offset, count);
            }
            public void Write(byte[] buffer, int offset, int count)
            {
                Log.LogFile("log.txt", "Executed Stream.Write() function");
                stream.Write(buffer, offset, count);
            }
            public void Flush()
            {
                Log.LogFile("log.txt", "Executed Stream.Flush() function");
                stream.Flush();
            }

        }
    }

    // The Client
    class ProxyPattern : StreamInterceptor
    {
        static void Main()
        {
            MyStream ms = new MyStream();
            ms.Read(new byte[] { }, 0, 0);
            ms.Write(new byte[] { }, 0, 0);
            ms.Flush();
            Console.WriteLine("See log.txt for details");

            Console.ReadKey();
        }
    }
}
