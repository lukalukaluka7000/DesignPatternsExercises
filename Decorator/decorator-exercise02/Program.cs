using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using Given;
namespace Given
{

    // The original Photo class
    public class Photo : Form
    {
        Image image;
        public Photo()
        {
            image = new Bitmap("jug.jpg");
            this.Text = "Lemonade";
            this.Paint += new PaintEventHandler(Drawer);
        }

        public virtual void Drawer(Object source, PaintEventArgs e)
        {
            e.Graphics.DrawImage(image, 30, 20);
        }
    }
}
namespace decorator_exercise02
{

    class DecoratorPatternExample
    {
        //added other component
        public class Hominid : Photo
        {
            public Hominid()
            {
                this.Paint += new PaintEventHandler(Drawer);
                this.Text = "Hominide";

            }
            public override void Drawer(Object source, PaintEventArgs e)
            {
                Pen pen = new Pen(Color.Blue, 2);
                //e.Graphics.TranslateTransform(100, 100); // needs tags and borders to be transformed also
                //e.Graphics.ScaleTransform(1.4f, 1.4f);
                e.Graphics.DrawEllipse(pen, 23, 1, 14, 14);
                e.Graphics.DrawLine(pen, 18, 16, 42, 16);
                e.Graphics.DrawLine(pen, 50, 40, 44, 42);
                e.Graphics.DrawLine(pen, 38, 25, 37, 42);
                e.Graphics.DrawLine(pen, 45, 75, 37, 75);
                e.Graphics.DrawLine(pen, 30, 50, 23, 75);
                e.Graphics.DrawLine(pen, 16, 75, 23, 42);
                e.Graphics.DrawLine(pen, 22, 25, 16, 42);
                e.Graphics.DrawLine(pen, 10, 40, 18, 16);


            }


        }
        // This simple BorderedPhoto decorator adds a colored BorderedPhoto of fixed size
        class BorderedPhoto : Photo
        {
            Photo photo;
            Color color;
            bool toggleBorder = false;

            public BorderedPhoto(Photo p, Color c)
            {
                photo = p;
                color = c;
 
                this.MouseDown += new MouseEventHandler(Form1_MouseClick);
            }

            public override void Drawer(Object source, PaintEventArgs e)
            {
                photo.Drawer(source, e);
                               
            }
            private void Form1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                Point mousePt = new Point(e.X, e.Y);
                toggleBorder = true ? toggleBorder == false : false;

                if (toggleBorder == true)
                    this.Paint += new PaintEventHandler(Drawer);
                else
                    this.Paint += new PaintEventHandler(BorderRemover);

                Invalidate();
            }

            private void BorderRemover(Object source, PaintEventArgs e)
            {
                photo.Drawer(source, e);

                e.Graphics.DrawRectangle(new Pen(color, 10), 25, 15, 215, 225);
            }
        }

        // The TaggedPhoto decorator keeps track of the tag number which gives it 
        // a specific place to be written

        class TaggedPhoto : Photo
        {
            Photo photo;
            string tag;
            int number;
            static int count;
            List<string> tags = new List<string>();

            public TaggedPhoto(Photo p, string t)
            {
                photo = p;
                tag = t;
                tags.Add(t);
                number = ++count;
            }

            public override void Drawer(Object source, PaintEventArgs e)
            {
                photo.Drawer(source, e);
                e.Graphics.DrawString(tag,
                    new Font("Arial", 16),
                    new SolidBrush(Color.Black),
                    new PointF(80, 100 + number * 20));
            }

            public string ListTaggedPhotos()
            {
                string s = "Tags are: ";
                foreach (string t in tags) s += t + " ";
                return s;
            }
        }



        static void Main()
        {
            // Application.Run acts as a simple client
            Photo photo;
            TaggedPhoto foodTaggedPhoto, colorTaggedPhoto, tag;
            BorderedPhoto composition;

            // Compose a photo with two TaggedPhotos and a blue BorderedPhoto
            photo = new Photo();
            Application.Run(photo);
            foodTaggedPhoto = new TaggedPhoto(photo, "Food");
            colorTaggedPhoto = new TaggedPhoto(foodTaggedPhoto, "Yellow");
            composition = new BorderedPhoto(colorTaggedPhoto, Color.Blue);
            Application.Run(composition);
            Console.WriteLine(colorTaggedPhoto.ListTaggedPhotos());

            // Compose a photo with one TaggedPhoto and a yellow BorderedPhoto
            photo = new Photo();
            tag = new TaggedPhoto(photo, "Jug");
            composition = new BorderedPhoto(tag, Color.Yellow);
            Application.Run(composition);
            Console.WriteLine(tag.ListTaggedPhotos());

            //added Hominid 
            Hominid hom = new Hominid();
            TaggedPhoto personTaggedHominid = new TaggedPhoto(hom, "Littleman");
            BorderedPhoto personTaggedAndBordered = new BorderedPhoto(personTaggedHominid, Color.Pink);
            Application.Run(personTaggedAndBordered);


            Console.ReadKey();
        }
    }
    /* Output

    TaggedPhotos are: Food Yellow                                                                                                  
    TaggedPhotos are: Food Yellow Jug   
    */
}
