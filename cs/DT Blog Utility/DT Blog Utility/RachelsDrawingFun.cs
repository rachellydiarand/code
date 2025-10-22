using DT_Blog_Utility.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DT_Blog_Utility
{
    public partial class RachelsDrawingFun : Form
    {
        public static RachelsDrawingFun Instance { get; set; }
        private Timer mTimer { get; set; }
        private List<RachelsLineObject> DrawingObjects = new List<RachelsLineObject>();
        public int ResizeSkipStep { get; set; } = 25;
        public int ResizeSkipOverflowStep { get; set; } = 25;
        private bool UseTimer { get; set; } = false;



        public RachelsDrawingFun()
        {
            InitializeComponent();

            Instance = this;


            if (false)
            {
                CreateLineObjects();
                mTimer = new Timer();
                mTimer.Interval = 100;
                mTimer.Tick += new EventHandler(EnterFrame);
                mTimer.Start();
            }
            else
            {
                CreateTriangleCircles();
            }


            this.FormClosing += new FormClosingEventHandler(Destroy);
            this.Resize += new EventHandler(DoResize);

        }

        private void RachelsDrawingFun_Load(object sender, EventArgs e)
        {
            BackColor = Color.Black;
            CreateTriangleCircles();
        }

        private void CreateTriangleCircles()
        {
            // we are drawing a randomish triangle (three connected points)
            // and then using each side of the triangle as the radius in a circle we will draw
            // and we'll use trig to draw the points of the circle (dots)
            // let's get started and time this little exercise
            // my clock says 7:57pm
            // stepping through the code, this should be working at 8:27pm
            // just drawing the three points.  Likely interference from outside sources.
            // prove it?  Move on?  What to do?  Resentment?  Fear?  Surrender.  Rest?  Choose another project?  Dinner?  30 minutes elapsed time.
            // 10 minutes of fixing math errors.  10 minutes of fighting something that should work.  Appoximately.
            // Test of my honesty?  Certainly.
            // will see the error of my ways tomorrow?  Unknown.
            // further reflection....
            // I used random.next in the new Point, but could not check the math.  NO, the points were checked.  Each was a valid point in the bounds of the window.
            // continueing on at 8:32pm after reflection, going to do graphics.fillrectangles, so will have to define rectangle objects
            // 8:39pm the fillrectangles method checks out mathematically but still doesn't draw on the form.graphics
            // could try this project in javascript on the canvas
            // why am I using visual studio?
            // couldn't make a living with Flash/AIR anymore
            // why?
            // went out of style, unions/world didn't find it useful enough, needed to move people on to other activities more in line with needs of society whatever those may be
            // or just personally needed to frustrate me for presidential learning experience.  or both most likely.
            // mars mission?  that's what I train for.  Are you just being a ass hole dick to your neighbor?
            // I don't see it like that.  I've never really known any of my neighbors.
            // you make a lot of music/noise, right?
            // yes, I'm a musician
            // why don't you play in clubs?
            // all kinds of reasons, likely mars training.
            // all alone is space, we'd do it if we could, I'd make millions in my 20/30's with music if I could, I'm an engineer now.
            // I don't care about money like that anymore.  I'm perfect for a space mission.  Absolutely perfect to do that alone.  We all take some risks in life.
            // 8:44pm  What to do next?  Well if they block this.  Hmm.... Try another activity?  Take a nap?  See programming error tomorrow morning at 3am on fresh start?  Get lucky?
            // food problem?  Yes, it's a big problem.  Can neighbor help?  Likely.  Will neighbor help?  Possibly.  Will church help?  Possibly.  Trust?
            // I trust my skills.  Trust my work.  I continue to do my work.  Will do that to the end.
            // Will they force you to change?  Possibly.  I don't have control over other people.
            // Just zen and child innocence, right?  Yes, presidential innocense/trust.  An inquiry into morals I suppose.  A display of 3d god.  And 4d higher power humanity.
            // And a visual example of retirement surrender in the western world (whatever it is).
            // why?  State of education and electronics.  A changing world.  Communication without physical devices and their limitations.  Lack of electronic video, sound and tactile interaction.
            // Test of love and compassion?  Test of romance on my end.  Male female?  Should I change my dating profile to be more accurate for my feelings today?
            // my 4d reality of impermanent male/female desires
            // my human limitations and desire to not accept everything
            // ideas that longer life isn't the only thing there is.
            // some things worth fighting for and choices we make
            // ok to die, ok for others to not support you
            // ok to follow me anyway.  To join me.  Concepts of leadership.  Display of leadership powers and capabilities.
            // bending over for computer is bad.  Solution?  Raise monitor to straight ahead back height.  Espresso machine as the grounding block?  Yes, it is heavy enough.
            // online meeting desire?  Yes, on my end.  Reality is that it is not working.  Why?  I don't know.  Others are not receptive.  Solution?
            // go with what is working.  My isolation and writing is working obviously.  Theory of end of computers and need for physical book copies.  Amazon problem.
            // Solution download Adobe Trial?  I guess.  I paid them thousands of dollars for software that is hard to make but likely they all became rich while I sturggled to pay them.
            // Resentment?  I don't know?  Kinda.  It's a stressor that makes me stronger looking for possible solutions.  The DT Blog Utility is working well.  Camera program is
            // frustrating.  Why would you rather continue fighting this coding thing than play music with others?  I don't know.  It just is.
            // Willing to fight for you culture eh?  Yes.  It is my baby like that.  I feel alone.  It is an introvert culture.  An online culture.
            // If people do not like my music performances/practice software thing as is, then ideally I'd move to someplace more isolated with a good internet connection.
            // Ideally there would be fun online meetings to attend to do my music thing.
            // I enjoy talking psychology and doing my music in the background.  I could do little shows like that all day.  Playing music while I hear new people talk about their lives.
            // Walking door to door (shoe leather) is ridiculous.  I learned that in 2014 and then reinforced in 2018.
            // Hate directed at your decisions?  It does feel that way sometimes.  It's all bittersweet and passive/aggressive.  Retirement psychology is interesting.
            // how many forced LTRs have you had?  Well, less than ideal, I'd say at least 5 of 7.  I got used to them pretty quick.  I can learn to love most people.  It gets tiring though.
            // Transgender hate?  Even from Sting?  I don't know.  Too hard to tell.  I wish this code worked.  Should I put in a work order to my case manager???

            // lets define a range for these to keep them in the window of opportunity
            // it's a Rectangle, right?
            int x1 = (int)((decimal)this.Width * (decimal).25);
            int x2 = (int)((decimal)this.Width * ((decimal)((decimal).25 + (decimal).5)));
            int y1 = (int)((decimal)this.Height * (decimal).25);
            int y2 = (int)((decimal)this.Height * ((decimal)((decimal).25 + (decimal).5)));

            // OinK!@  Let's make a rectangle | ^
            var rect = new Rectangle(x1, y1, x2 - x1, y2 - y1);// thank you auto whatever for completing the y2 - y1!  so helpful!  Good response right now too. oh, I'm on the clock!


            var r = new Random();
            Point p1 = new Point(r.Next(rect.X, rect.X + rect.Width), r.Next(rect.Y, rect.Y + rect.Height));
            Point p2 = new Point(r.Next(rect.X, rect.X + rect.Width), r.Next(rect.Y, rect.Y + rect.Height));
            Point p3 = new Point(r.Next(rect.X, rect.X + rect.Width), r.Next(rect.Y, rect.Y + rect.Height));

            // make rectanges of 1,1 or 2,2 for each of these points
            Rectangle r1 = new Rectangle(p1, new Size(1, 1));
            Rectangle r2 = new Rectangle(p2, new Size(1, 1));
            Rectangle r3 = new Rectangle(p3, new Size(1, 1));

            // graph em!
            // plot them
            //var pb = new PictureBox();
            //pb.Width = this.Width;
            //pb.Height = this.Height;
            var g = this.CreateGraphics();
            //g.Clear(Color.Black);
            g.DrawRectangle(new Pen(Color.Red), r1);
            g.DrawRectangle(new Pen(Color.Red), r2);
            g.DrawRectangle(new Pen(Color.Red), r3);

            g.FillRectangle(Brushes.Red, r1);
            g.FillRectangle(Brushes.Red, r2);
            g.FillRectangle(Brushes.Red, r3);

            //this.Controls.Add(pb);

            //return;
            // draw the connections.  We'll just use draw line for this
            g.DrawLine(new Pen(Color.LightBlue), p1, p2);
            g.DrawLine(new Pen(Color.LightBlue), p1, p3);
            g.DrawLine(new Pen(Color.LightBlue), p2, p3);

            // check our work with the lovely coding
        }

        private void CreateLineObjects()
        {
            Random r = new Random();
            for (int i = 0; i < 7; i++)
            {
                var line = new RachelsLineObject(this, Width, Height);
                line.X = Width / 2 - (30 * i);
                line.Y = Height / 2 - (30 * i);
                line.RotationSpeed = ((decimal)(r.Next(10)) + 12);
                line.Rotation = (decimal)(r.Next(360));
                line.RegistrationPercentage = (decimal)r.Next(int.MaxValue) / int.MaxValue;
                line.Length = 100;
                line.Direction = (decimal)(r.Next(360));
                line.Speed = ((decimal)(r.Next(10)) + 8);
                DrawingObjects.Add(line);
            }
        }

        private void RenderTriangleCircles()
        {
            ResizeSkipStep++;
            if (ResizeSkipStep > ResizeSkipOverflowStep)
            {
                ResizeSkipStep = 0;
            }
            if (ResizeSkipStep != 0) return;

            CreateTriangleCircles();
            foreach (var line in DrawingObjects)
            {
                line.StageHeight = Height;
                line.StageWidth = Width;
            }
        }

        private void ClearCanvas()
        {
            var g = this.CreateGraphics();
            g.Clear(Color.Black);
        }

        public void DoResize(object sender, EventArgs e)
        {
            RenderTriangleCircles();
        }

        private void EnterFrame(object sender, EventArgs e)
        {

            //foreach(var line in DrawingObjects)
            //{
            //    line.RenderFrame(g);
            //}

            RenderTriangleCircles();
        }

        private void StopTimer()
        {
            if (mTimer == null) return;
            mTimer.Stop();
            mTimer.Dispose();
            mTimer = null;
        }

        private void StartTimer()
        {
            StopTimer();

            mTimer = new Timer();
            mTimer.Interval = 100;
            mTimer.Tick += new EventHandler(EnterFrame);
            mTimer.Start();
        }

        private void Destroy(object sender, EventArgs e)
        {
            StopTimer();
        }

        private void useTimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopTimer();
            UseTimer = !UseTimer;
            if (UseTimer)
            {
                StartTimer();
                menuStrip1.Items.Find("useTimerToolStripMenuItem", true).ElementAt(0).Text = "Use Resize";
            }
            else
            {
                menuStrip1.Items.Find("useTimerToolStripMenuItem", true).ElementAt(0).Text = "Use Timer";
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearCanvas();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormRachelsDrawingFunSettings();
            form.Show();
        }
    }
}
