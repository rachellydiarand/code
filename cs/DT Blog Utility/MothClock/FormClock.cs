using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MothClock
{
    public partial class FormClock : Form
    {
        public Timer ClockTimer { get; set; }
        private decimal dg = (decimal)(Math.PI / 180);
        private decimal rd = (decimal)(180 / Math.PI);

        public FormClock()
        {
            InitializeComponent();
        }

        private void FormClock_Load(object sender, EventArgs e)
        {
            ClockTimer = new Timer();
            ClockTimer.Interval = 100;
            ClockTimer.Tick += new EventHandler(UpdateClock);
            ClockTimer.Start();
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            var w = this.ClientRectangle.Width;
            var h = this.ClientRectangle.Height;

            var squareSize = w;
            if (h < w) squareSize = h;

            using(Graphics g = this.CreateGraphics())
            {
                g.Clear(Color.Black);


            }
        }
    }
}
