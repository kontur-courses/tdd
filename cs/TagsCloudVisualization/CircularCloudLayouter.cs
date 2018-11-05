using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point _center;
        private List<Rectangle> _rectanglesList;

        public CircularCloudLayouter(Point center)
        {
            _center = center;
            _rectanglesList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return new Rectangle(new Point(0, 0));
        }
    }
}
