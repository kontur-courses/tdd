using System.Drawing;
using System.Windows.Forms;
using System;

// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var size = new Size(1500, 700);
            var center = new Point(size.Width / 2, size.Height / 2);
            var spiral = new Spiral(1, center);
            var drawer = new CloudLayouterDrawer(Color.Black);
            var form = new CloudLayouterForm(size, GetRectangles(100, spiral), drawer);
            Application.Run(form);
        }

        private static Rectangle[] GetRectangles(int count, ICurve curve)
        {
            var random = new Random();
            var cloud = new CloudLayouter(curve);
            for (var i = 0; i < count; i++)
            {
                cloud.PutNextRectangle(new Size(random.Next(30, 30), random.Next(30, 100)));
            }

            return cloud.Rectangles;
        }
    }
}