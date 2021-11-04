using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center
        { get; set; }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Point rectangleLocation = new Point(Center.X - rectangleSize.Width/2, Center.Y - rectangleSize.Height/2);
            var rectangle = new Rectangle(rectangleLocation, rectangleSize);
            return rectangle;
        }
    }

    class Program
    {
        public static void Main()
        {

        }
    }
}
