using System;

class CommandPattern
{
    // Command Pattern           Judith Bishop June 2007
    //
    // Uses a single delegate for the single type of commands that the client invokes.

    delegate void Invoker();
    delegate void ParameterInvoker(string komanda);

    static Invoker Execute, Undo, Redo;
    static ParameterInvoker Invoke;

    class Command
    {
        public Command(Receiver receiver)
        {
            Execute = receiver.Action;
            Redo = receiver.ReverseForward;
            Undo = receiver.ReverseBackward;

            Invoke = receiver.Action;
        }
    }

    public class Receiver
    {
        string build, oldbuild;
        string s = "additional string";

        string undodBuild;
        bool didUndo = false;

        public void Action()
        {
            oldbuild = build;
            build += s; didUndo = false;
            Console.WriteLine("Receiver ADDING: " + s);
            Console.WriteLine("Current state: {0}", build);
        }
        public void ReverseForward()
        {
            if (didUndo)
            {
                build = undodBuild;
                Console.WriteLine("Receiver REDOing: ");
                Console.WriteLine("Current state: {0}", build);
                didUndo = false;
            }
            else
            {
                Console.WriteLine("Cannot redo if there was no undo...");
            }
        }
        public void ReverseBackward()
        {
            undodBuild = build;
            build = oldbuild;
            Console.WriteLine("Receiver UNDOing: ");
            Console.WriteLine("Current state: {0}", build);
            didUndo = true;
        }
        public void Action(string komanda)
        {
            oldbuild = build; build += komanda; didUndo = false;
            Console.WriteLine("Receiver is adding string parameter from client : {0}", komanda);
            Console.WriteLine("Current state: {0}", build);
        }
    }

    static void Main()
    {
        new Command(new Receiver());
        Execute();
        Redo();
        Undo();
        Execute();

        Invoke("maci nadodaj mi ovo");
        Undo();
        Redo();
        Undo();
        Redo();
        Console.ReadKey();
    }
}
/* Output
Receiver is adding some string 
Receiver is adding some string some string 
Receiver is reverting to some string 
Receiver is adding some string some string
*/


