using System;

class CommandPattern
{
    // Command Pattern           Judith Bishop June 2007
    //
    // Uses a single delegate for the single type of commands that the client invokes.

    delegate void Invoker();
    static Invoker Execute, Undo, Redo;

    class Command
    {
        public Command(Receiver receiver)
        {
            Execute = receiver.Action;
            Redo = receiver.Action;
            Undo = receiver.Reverse;
        }
    }

    public class Receiver
    {
        string build, oldbuild;
        string s = "some string ";

        public void Action()
        {
            oldbuild = build;
            build += s;
            Console.WriteLine("Receiver is adding " + build);
        }

        public void Reverse()
        {
            build = oldbuild;
            Console.WriteLine("Receiver is reverting to " + build);
        }
    }

    static void Main()
    {
        new Command(new Receiver());
        Execute();
        Redo();
        Undo();
        Execute();
        Console.ReadKey();
    }
}
/* Output
Receiver is adding some string 
Receiver is adding some string some string 
Receiver is reverting to some string 
Receiver is adding some string some string
*/


