using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var rr = new Rectangle(100,100,100,100);
            Console.WriteLine(rr.X+rr.Y+rr.Width+rr.Height);
            rr.X = 99999;
            Console.WriteLine(rr.X + rr.Y + rr.Width + rr.Height);
            var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));

            circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            circularCloudLayouter.PutNextRectangle(new Size(50, 50));

            circularCloudLayouter.Generate();
        }
    }
}
