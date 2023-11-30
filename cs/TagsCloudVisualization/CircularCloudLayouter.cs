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
        public readonly Point CenterPoint;
        public CircularCloudLayouter(Point center)
        {
            CenterPoint = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Rectangle can't have negative width or height");

            var locationForRect = new Point(CenterPoint.X - rectangleSize.Width / 2,
                CenterPoint.Y - rectangleSize.Height / 2);
            return new Rectangle(locationForRect, rectangleSize);
        }
    }
}
