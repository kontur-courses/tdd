using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            var vs = new LayoutVisualizator();
            var layouter = new CircularCloudLayouter(new Point(0, 0));

            layouter.PutNextRectangle(new Size(10, 10));
            layouter.PutNextRectangle(new Size(50, 100));
            layouter.PutNextRectangle(new Size(40, 40));

            var rectangles = layouter.GetRectangles();
            vs.VisualizeCloud(rectangles);
            vs.SaveToDesktop();
        }
    }
}
