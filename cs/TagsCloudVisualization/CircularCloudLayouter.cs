using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point _center;

        public CircularCloudLayouter(Point center)
        {
            _center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return new Rectangle(_center, rectangleSize);
        }
    }
}
