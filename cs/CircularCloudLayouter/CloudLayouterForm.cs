using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class CloudLayouterForm : Form
    {
        private readonly ICloudLayouterDrawer cloudLayouterDrawer;
        private readonly string path = "result//result.png";

        public CloudLayouterForm(ICloudLayouterDrawer cloudLayouterDrawer, int numberRectangles)
        {
            this.cloudLayouterDrawer = cloudLayouterDrawer;
            this.cloudLayouterDrawer
                .CloudLayouter
                .ChangeCenterPoint(new Point(Size.Width / 2, Size.Height / 2));
            AddRectangles(numberRectangles);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            using (var bitmap = new Bitmap(1920, 1080))
            {
                var g = Graphics.FromImage(bitmap);

                cloudLayouterDrawer
                    .CloudLayouter
                    .ChangeCenterPoint(new Point(bitmap.Width / 2, bitmap.Height / 2));

                cloudLayouterDrawer.Draw(g);

                if (!Directory.Exists("result"))
                    Directory.CreateDirectory("result");

                bitmap.Save(path, ImageFormat.Png);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            cloudLayouterDrawer.CloudLayouter.ChangeCenterPoint(new Point(Size.Width / 2, Size.Height / 2));
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            cloudLayouterDrawer.Draw(e.Graphics);
        }

        private void AddRectangles(int count)
        {
            var random = new Random();
            for (var i = 0; i < count; i++)
            {
                cloudLayouterDrawer
                    .CloudLayouter
                    .PutNextRectangle(new Size(random.Next(30, 30), random.Next(30, 100)));
            }
        }
    }
}