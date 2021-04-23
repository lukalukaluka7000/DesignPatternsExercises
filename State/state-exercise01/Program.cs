using System;
using System.Collections.Generic;

namespace StatePattern
{

    // State Pattern               D-J Miller and Judith Bishop  Sept 2007
    // Simple game where the context changes the state based on user input
    // Has four states, each with 6 operations 

    abstract class IState
    {
        public virtual string Move(Context context) { return ""; }
        public virtual string Attack(Context context) { return ""; }
        public virtual string Stop(Context context) { return ""; }
        public virtual string Run(Context context) { return ""; }
        public virtual string Panic(Context context) { return ""; }
        public virtual string CalmDown(Context context) { return ""; }
        
    }

    // There are four States
    class RestingState : IState
    {
        public override string Move(Context context)
        {
            context.State = new MovingState();
            context.Energy--;
            return "You start moving";
        }
        public override string Attack(Context context)
        {
            context.State = new AttackingState();
            return $"You start attacking and dealing damage of your strength {context.Strength}";
        }
        public override string Stop(Context context)
        {
            return "You are already stopped!";
        }
        public override string Run(Context context)
        {
            return "You cannot run unless you are moving";
        }
        public override string Panic(Context context)
        {
            context.State = new PanickingState();
            context.PsychicInstability--;
            return "You start Panicking and your psychic instability increases Soon you will begin seeing things :)";
        }
        public override string CalmDown(Context context)
        {
            return "You are already relaxed";
        }
    }

    class AttackingState : IState
    {
        public override string Move(Context context)
        {
            return "You need to stop attacking first";
        }
        public override string Attack(Context context)
        {
            return "You attack the darkness for " +
                 (new Random().Next(20) + 1) + " damage";
        }
        public override string Stop(Context context)
        {
            context.State = new RestingState();
            return "You are calm down and come to rest";
        }
        public override string Run(Context context)
        {
            context.State = new MovingState();
            context.Stamina += 2;
            
            return $"You Run away from the fray (stamina increased by two : {context.Stamina})";
        }
        public override string Panic(Context context)
        {
            context.State = new PanickingState();
            return "You start Panicking and begin seeing things";
        }
        public override string CalmDown(Context context)
        {
            context.State = new RestingState();
            return "You fall down and sleep";
        }
    }

    class PanickingState : IState
    {
        public override string Move(Context context)
        {
            context.PsychicInstability += 5;
            return "You move around randomly in a blind panic";
        }
        public override string Attack(Context context)
        {
            context.PsychicInstability += 35;
            return "You start attacking in panic, but keep on missing";
        }
        public override string Stop(Context context)
        {
            context.State = new MovingState();
            context.PsychicInstability -= 10;
            return "You are start relaxing, but keep on moving";
        }
        public override string Run(Context context)
        {
            context.PsychicInstability += 25;
            return "You run around in your panic";
        }
        public override string Panic(Context context)
        {
            context.PsychicInstability += 5;
            return "You are already in a panic";
        }
        public override string CalmDown(Context context)
        {
            context.State = new RestingState();
            context.PsychicInstability -= 20;
            return "You relax and calm down. Good Job. Calm your head and instability...";
        }
    }

    class MovingState : IState
    {
        public override string Move(Context context)
        {
            context.Energy -= 2;
            return "You already are in Moving State";
        }
        public override string Attack(Context context)
        {
            return "You need to stop moving first";
        }
        public override string Stop(Context context)
        {
            context.State = new RestingState();
            context.Energy += 15;
            return "You stand still in a dark room";
        }
        public override string Run(Context context)
        {
            context.Energy -= 30;
            context.Stamina += 3;
            return $"You run around in cirles and your stamina increased ({context.Stamina})";
        }
        public override string Panic(Context context)
        {
            context.State = new PanickingState();
            context.PsychicInstability += 25;
            context.Energy += 10;
            return "You start Panicking and begin seeing things";
        }
        public override string CalmDown(Context context)
        {
            context.State = new RestingState();
            context.PsychicInstability -= 15;
            context.Energy += 15;
            return "You stand still and relax";
        }
    }

    class Context
    {
        public IState State { get; set; }
        public int Strength { get; set; }
        public int Stamina { get; set; }
        public int CountPsy = 0;
        private int _energy;
        public int Energy 
        {
            get
            {
                return _energy;
            }
            set
            {
                if (value > Stamina)
                {
                    _energy = Stamina;
                    Console.WriteLine($"Cannot go beyond Stamina:{Stamina} with your Energy");
                }
                else if (value < 0)
                {
                    _energy = 0;
                    Console.WriteLine($"Cannot go bellow zero with your Energy");
                }
                else
                {
                    _energy = value;
                }
            }
        }
        private int _psychicInstability;
        public int PsychicInstability
        {
            get
            {
                return _psychicInstability;
            }
            set
            {
                if(value < 0)
                {
                    _psychicInstability = 0;
                    Console.WriteLine("You are OK psychic wise");
                }
                else if(value > 100)
                {
                    _psychicInstability = 100;
                    Console.WriteLine("Canot go beyond 100. Calm down or you'll die from neurosis...");
                    CountPsy++;
                }
                else
                {
                    _psychicInstability = value;
                    CountPsy = 0;
                }
            }
        }
        public Context(int Strength, int Stamina, int Energy = 50, int PsychicInstability = 0)
        {
            this.Strength = Strength;
            this.Stamina = Stamina;

            this.Energy = Energy;
            this.PsychicInstability = PsychicInstability;
        }
        public string PrintoutOverallCurrentHealth(Context context)
        {
            return $"------Energy------Psychic Instability------\n" +
                   $"------  {context.Energy}  ------     {context.PsychicInstability}             ------";
        }

        public void Request(char c)
        {
            string result;
            switch (char.ToLower(c))
            {
                case 'm':
                    result = State.Move(this);
                    Console.WriteLine(PrintoutOverallCurrentHealth(this));
                    break;
                case 'a':
                    result = State.Attack(this);
                    Console.WriteLine(PrintoutOverallCurrentHealth(this));
                    break;
                case 's':
                    result = State.Stop(this);
                    Console.WriteLine(PrintoutOverallCurrentHealth(this));
                    break;
                case 'r':
                    result = State.Run(this);
                    Console.WriteLine(PrintoutOverallCurrentHealth(this));
                    break;
                case 'p':
                    result = State.Panic(this);
                    Console.WriteLine(PrintoutOverallCurrentHealth(this));
                    break;
                case 'c':
                    result = State.CalmDown(this);
                    Console.WriteLine(PrintoutOverallCurrentHealth(this));
                    break;
                case 'e':
                    result = "Thank you for playing \"The RPC Game\"";
                    break;
                case 'y':
                    result = "Died from psycho killah...";
                    break;
                default:
                    result = "Error, try again";
                    break;
            }
            Console.WriteLine(result);
        }
    }

    static class Program
    {
        // The user interface
        static void Main()
        {
            // context.s are States
            // Decide on a starting state and hold onto the Context thus established
            Random rnd = new Random();
            int strength = rnd.Next(50, 150);
            int stamina = rnd.Next(50, 150);
            Context context = new Context(strength, stamina);
            context.State = new RestingState();
            char command = ' ';
            Console.WriteLine("Welcome to \"The State Game\"!");
            Console.WriteLine("You are standing here looking relaxed!");
            Console.WriteLine("Your strength throught a game is " + strength + "/150 and your current stamina is " + stamina + "/150");
            Console.WriteLine("By moving and running your stamina will sporadically increase...");

            while (char.ToLower(command) != 'e' && char.ToLower(command) != 'y')
            {
                if (context.CountPsy > 2)
                {
                    context.Request('y');
                }
                Console.WriteLine("\nWhat would you like to do now?");
                Console.Write("   Move    Attack    Stop    Run    Panic    CalmDown    Exit the game: ==>");
                string choice;
                do
                    choice = Console.ReadLine();
                while (choice == null);
                command = choice[0];
                context.Request(command);
                
            }
            Console.ReadKey();
        }
    }
}


