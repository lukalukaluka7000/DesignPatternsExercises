using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

class ObserverPattern
{

    //Observer Pattern            Judith Bishop Sept 2007
    // Demonstrates Blog updates. Observers can subscribe and unsubscribe
    // online through a GUI.

    // State type
    public class Blogs
    {
        public string Name { get; set; }
        public string Topic { get; set; }

        public Blogs(string name, string topic)
        {
            Name = name;
            Topic = topic;
        }
    }

    public delegate void PushChanges(Blogs blog);
    public delegate void OnlyNotification();
    // The Subject runs in a thread and changes its state
    // independently by calling the Iterator. At each change, 
    // it notifies its Observers
    // The Callbacks are in a collection based on blogger name

    class Subject
    {
        Dictionary<string, OnlyNotification> Notify = new Dictionary<string, OnlyNotification>();
        Dictionary<int, Blogs> Data = new Dictionary<int, Blogs>();
        Simulator simulator = new Simulator();
        const int speed = 4000;
        int i = 0;

        public void Go()
        {
            new Thread(new ThreadStart(Run)).Start();
        }

        internal string GetNameOfBlog()
        {
            return Data.Values.Last().Name;
        }
        internal string GetTopicOfBlog()
        {
            return Data.Values.Last().Topic;
        }

        void Run()
        {
            foreach (Blogs blog in simulator)
            {
                Register(blog.Name); // if necessary
                SaveData(blog);
                Notify[blog.Name](); // publish changes
                Thread.Sleep(speed); // millisconds
            }
        }

        private void SaveData(Blogs blog)
        {
            if (!Data.ContainsKey(i))
            {
                Data[i++] = blog;
            }
        }

        // Adds to the blogger list if unknown
        void Register(string blogger)
        {
            if (!Notify.ContainsKey(blogger))
            {
                Notify[blogger] = delegate { };
            }
        }

        public void Attach(string blogger, OnlyNotification Update)
        {
            Register(blogger);
            Notify[blogger] += Update;
        }

        public void Detach(string blogger, OnlyNotification Update)
        {
            // Possible problem here
            Notify[blogger] -= Update;
        }


    }

    class Interact : Form
    {
        public TextBox wall;
        public Button subscribeButton, unsubscribeButton;
        public TextBox messageBox;
        string name;

        public Interact(string name, EventHandler Input)
        {

            Control.CheckForIllegalCrossThreadCalls = true;
            // wall must be first!
            this.name = name;
            wall = new TextBox();
            wall.Multiline = true;
            wall.Location = new Point(0, 30);
            wall.Width = 300;
            wall.Height = 200;
            wall.AcceptsReturn = true;
            wall.Dock = DockStyle.Fill;
            this.Text = name;
            wall.Font = new Font(wall.Font.Name, 12);
            this.Controls.Add(wall);

            // Panel must be second
            Panel p = new Panel();
            messageBox = new TextBox();
            messageBox.Width = 120;
            p.Controls.Add(messageBox);
            subscribeButton = new Button();
            subscribeButton.Left = messageBox.Width;
            subscribeButton.Text = "Subscribe";
            subscribeButton.Click += new EventHandler(Input);
            p.Controls.Add(subscribeButton);
            unsubscribeButton = new Button();
            unsubscribeButton.Left = messageBox.Width + subscribeButton.Width;
            unsubscribeButton.Text = "Unsubscribe";
            unsubscribeButton.Click += new EventHandler(Input);
            p.Controls.Add(unsubscribeButton);

            p.Height = subscribeButton.Height;
            p.Height = unsubscribeButton.Height;
            p.Dock = DockStyle.Top;
            this.Controls.Add(p);
        }

        public void Output(string message)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate () { Output(message); });
            else
            {
                wall.AppendText(message + "\r\n");
                this.Show();
            }
        }
    }

    // Useful if more observer types
    interface IObserver
    {
        void Update();
        string GetName();
        string GetTopic();
    }

    class Observer : IObserver
    {
        string name;
        Subject subject;
        Interact visuals;
        Random rnd = new Random();

        public Observer(Subject subject, string name, string onlyFrom = "")
        {
            this.subject = subject;

            this.name = name;
            visuals = new Interact(name, Input);
            new Thread((ParameterizedThreadStart)delegate (object o) {
                Application.Run(visuals);
            }).Start(this);

            // Wait to load the GUI
            while (visuals == null || !visuals.IsHandleCreated)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }
            subject.Attach("Jim", Update);
            subject.Attach("Eric", Update);
            subject.Attach("Judith", Update);
        }

        public string GetName()
        {
            return subject.GetNameOfBlog();
        }

        public string GetTopic()
        {
            return subject.GetTopicOfBlog();
        }

        public void Input(object source, EventArgs e)
        {
            // Subscribe to the specified blogger
            if (source == visuals.subscribeButton)
            {
                subject.Attach(visuals.messageBox.Text, Update);
                visuals.wall.AppendText("Subscribed to " + visuals.messageBox.Text + "\r\n");
            }
            else
            //Unsubscribe to the blogger
            if (source == visuals.unsubscribeButton)
            {
                subject.Detach(visuals.messageBox.Text, Update);
                visuals.wall.AppendText("Unsubscribed from " + visuals.messageBox.Text + "\r\n");
            }
        }
        //I already noted the disadvantages of the 'pull' model. The observers would have to know things
        //about the subject in order to query the right information, which leads A- to downcasting (ugly),
        //or B- favorably to more specific Observable interfaces, that offer more specific accessor methods.
        //For example AgeObservable offers a getAge() method.
        public void Update() //this is oush model, observer knows all about whole shared object
        {
            Console.WriteLine(rnd.Next(0, 10));
            if(rnd.Next(0,10) < 5)
            {
                visuals.Output("Blog from " + GetName());
            }
            else
            {
                visuals.Output("New Blog on topic " + GetTopic());
            }
            
        }
    }

    #region First exercise, little tweaking needed, I advise not
    //interface IObserverConsole
    //{
    //    void Update(Blogs state);

    //}
    //class ObserverConsole :  IObserverConsole

    //{
    //    string name;
    //    Subject blogs;
    //    public ObserverConsole(Subject subject, string name)
    //    {
    //        this.blogs = subject;
    //        this.name = name;

    //        new Thread((ParameterizedThreadStart)delegate (object o)
    //        {
    //            Application.Run();
    //        }).Start(this);
    //        blogs.Attach("Jim", Update);
    //        blogs.Attach("Eric", Update);
    //        blogs.Attach("Judith", Update);
    //    }
        
    //    public void Update(Blogs state)
    //    {
    //        Console.WriteLine("Blog from " + state.Name + " on " + state.Topic + "(Android View)");
    //    }
    //}
    #endregion

    // Iterator to supply the data
    class Simulator : IEnumerable
    {

        Blogs[] bloggers = {new Blogs ("Jim","UML diagrams"),
      new Blogs("Eric","Iterators"),
      new Blogs("Eric","Extension Methods"),
      new Blogs("Judith","Delegates"),
      new Blogs("Eric","Type inference"),
      new Blogs("Jim","Threads"),
      new Blogs("Eric","Lamda expressions"),
      new Blogs("Judith","Anonymous properties"),
      new Blogs("Eric","Generic delegates"),
      new Blogs("Jim","Efficiency")};

        public IEnumerator GetEnumerator()
        {
            foreach (Blogs blog in bloggers)
                yield return blog;
        }
    }

    static void Main()
    {
        Subject subject = new Subject();
        Observer Observer = new Observer(subject, "Thabo");
        Observer observer2 = new Observer(subject, "Ellen");
        //ObserverConsole obsCons = new ObserverConsole(subject, "AndroidUser");

        subject.Go();
    }
}
