﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace adapter_exercise02
{
    // Adapter Pattern Example            Judith Bishop Aug 2007
    // Sets up a Coolbook
    // This is D-J's as changed for the book
    class AdapterPattern
    {

        //class SpaceBookSystem {  

        public delegate void InputEventHandler(object sender, EventArgs e, string s);

        //Adapter 
        public class MyCoolBook : MyOpenBook
        {
            
            Interact visuals;
            
            public MyCoolBook(string name) : base(name)
            {
                // create interact on the relavent thread, and start it!
                new Thread(delegate () {
                    visuals = new Interact("CoolBook Beta");
                    visuals.InputEvent += new InputEventHandler(OnInput);
                    visuals.FormClosed += new FormClosedEventHandler(OnFormClosed);
                    Application.Run(visuals);
                }).Start();
                
                

                while (visuals == null)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }

                Add("Welcome to CoolBook " + Name);
            }


            private void OnFormClosed(object sender, FormClosedEventArgs e)
            {
                Community.Remove(Name);
                
            }

            private void OnInput(object sender, EventArgs e, string s)
            {
                if (((Interact)sender).ActiveControl.Text == "Poke")
                {
                    Add("\r\n");
                    Add(s, "Poked you");
                }
                else if (((Interact)sender).ActiveControl.Text == "Super Poke")
                {
                    Add("\r\n");
                    string message = s;
                    foreach (var user in Community)
                    {
                        //broadcasting
                        if (this.Name != user.Key)
                            Add(user.Key, message);
                    }
                }
            }

            public new void Poke(string who)
            {
                Add("\r\n");
                if (Community.ContainsKey(who))
                {
                    Community[who].Add(Name, "Poked you");
                }
                else
                    Add("Friend " + who + " is not part of the community");
            }

            public new void Add(string message)
            {
                visuals.Output(message);
            }
            //public void Print(string messag)
            //{
            //    visuals.Output(messag);
            //}

            public new void Add(string friend, string message)
            {
                if (Community.ContainsKey(friend))
                    Community[friend].Add(Name + " : " + message);
                else
                    Add("Friend " + friend + " is not part of the community");
            }
        }

        //New Implementation (Adaptee)
        public class Interact : Form
        {
            public TextBox Wall { get; set; }
            public Button Poke { get; set; }
            public Button SuperPoke { get; set; }

            public Interact() { }

            public Interact(string title)
            {
                Control.CheckForIllegalCrossThreadCalls = true;
                Poke = new Button();
                Poke.Text = "Poke";

                SuperPoke = new Button();
                SuperPoke.Text = "Super Poke";
                SuperPoke.Location = new Point(100, 0);
                this.Controls.Add(Poke);
                Poke.Click += new EventHandler(Input);

                this.Controls.Add(SuperPoke);
                SuperPoke.Click += new EventHandler(InputSuper);

                Wall = new TextBox();
                Wall.Multiline = true;
                Wall.Location = new Point(0, 30);
                Wall.Width = 300;
                Wall.Height = 200;
                Wall.Font = new Font(Wall.Font.Name, 12);
                Wall.AcceptsReturn = true;
                this.Text = title;
                this.Controls.Add(Wall);
            }

            public event InputEventHandler InputEvent;

            public void Input(object source, EventArgs e)
            {
                string who = Wall.SelectedText;
                if (InputEvent != null)
                    InputEvent(this, EventArgs.Empty, who);
            }
            public void InputSuper(object source, EventArgs e)
            {
                string message = Wall.SelectedText;
                if (InputEvent != null)
                    InputEvent(this, EventArgs.Empty, message);
            }

            public void Output(string message)
            {
                if (this.InvokeRequired)
                    this.Invoke((MethodInvoker)delegate () { Output(message); });
                else
                {
                    Wall.AppendText(message + "\r\n");
                    this.Show();
                }
            }

            protected override void OnFormClosed(FormClosedEventArgs e)
            {
                // Remove the interact and coolbook from the community here!
                base.OnFormClosed(e);
            }
        }

        // The RealSubject (ProxyPattern) 
        // CANNOT CHANGE
        public class SpaceBook
        {
            //public static SortedList<string, SpaceBook> community =
            //      new SortedList<string, SpaceBook>(100);
            public static SortedList<string, SpaceBook> community { get; set; } = new SortedList<string, SpaceBook>(100);
            public string pages;
            string name;
            string gap = "\n\t\t\t\t";

            public bool Unique(string name)
            {
                return community.ContainsKey(name);
            }

            internal SpaceBook(string n)
            {
                name = n;
                community[n] = this;
            }

            internal string Add(string s)
            {
                pages += gap + s;
                return gap + "======== " + name + "'s SpaceBook =========\n" +
                      pages +
                      gap + "\n===================================";
            }

            internal string Add(string friend, string message)
            {
                return community[friend].Add(message);
            }

            internal void Poke(string who, string friend)
            {
                community[who].pages += gap + friend + " poked you";
            }

           
        }

        // Target (Adapter Pattern)
        //CANNOT CHANGE
        public class MyOpenBook
        {

            SpaceBook myOpenBook;
            public string Name { get; set; }
            public static int Users { get; set; }
            public static SortedList<string, SpaceBook> Community;

            public MyOpenBook(string n)
            {
                Name = n;
                Users++;
                myOpenBook = new SpaceBook(Name);
                Community = SpaceBook.community;
            }

            public void Add(string message)
            {
                Console.WriteLine(myOpenBook.Add(message));
            }

            public void Add(string friend, string message)
            {
                Console.WriteLine(myOpenBook.Add(friend, Name + " : " + message));
            }

            public void Poke(string who)
            {
                myOpenBook.Poke(who, Name);
            }

            public void SuperPoke(string who, string what)
            {
                myOpenBook.Add(who, what + " you");
            }
        }

        // The Client
        static void Main()
        {

            MyCoolBook judith = new MyCoolBook("Judith");
            judith.Add("Hello world");

            MyCoolBook tom = new MyCoolBook("Tom");
            tom.Poke("Judith");
            tom.Add("Hey, We are on CoolBook");
            judith.Poke("Tom");
            Console.ReadLine();
        }
    }
}
