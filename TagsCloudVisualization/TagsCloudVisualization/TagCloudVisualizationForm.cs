using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    

    internal class TagCloudVisualizationForm : Form
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly CircularCloudLayouter cloud;
        public TagCloudVisualizationForm()
        {
            var center = new Point(900, 500);
            cloud = new CircularCloudLayouter(center);
            var random = new Random();
            for (var i = 0; i < 200; i++)
            {
                var word = new Size(30*random.Next(1,4), 10 * random.Next(1,4));
                rectangles.Add(cloud.PutNextRectangle(word));
            }
            var timer = new Timer { Interval = 10 };
            timer.Tick += TimerTick;
            timer.Start();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var rectangle in rectangles)
            e.Graphics.DrawRectangle(new Pen(Color.Black), rectangle);
            e.Graphics.DrawEllipse(new Pen(Color.Blue),cloud.center.X - (float)cloud.radius,
                cloud.center.Y - (float)cloud.radius, (float)cloud.radius*2, (float)cloud.radius*2);
            e.Graphics.DrawRectangle(new Pen(Color.Red), new Rectangle(cloud.center,new Size(1,1)));


        }
        private void TimerTick(object sender, EventArgs e)
        {
            Invalidate();
            Update();
        }
    }
}