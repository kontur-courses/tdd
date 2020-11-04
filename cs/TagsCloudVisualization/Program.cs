using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main()
        {
            var l = new CircularCloudLayouter(new Point(1, 2));
            l.PutNextRectangle(new Size(10, 10));
            var c = new CircularCloudVisualization(l);
            c.SaveImage("Test");

        }
    }
}
