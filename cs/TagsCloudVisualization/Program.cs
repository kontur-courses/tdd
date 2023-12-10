using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        private static void Main()
        {
            var p = new Point(0, 0);
            var a = new CircularCloudLayouter(p,new SpiralDistribution(p));
            var random = new Random();

            for (int i = 0; i < 350; i++)
            {

                a.PutNextRectangle(new Size(5,5));
            }
            CloudLayouterDrawer.DrawCloudLayout(a,"example3.png");

        }
    }
}