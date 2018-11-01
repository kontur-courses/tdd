using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public static void Main(string[] args)
        {

        }

        public Point Center { get; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Point GetLocationAfterCentering(Point location, Size rectangleSize)
        {
            return new Point(location.X - rectangleSize.Width / 2, location.Y - rectangleSize.Height / 2);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = GetLocationAfterCentering(Center, rectangleSize);
            return new Rectangle(location, rectangleSize);
        }
    }
}
