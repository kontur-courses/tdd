using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CenterMassChecker
    {
        public static PointF FindCenterMass(List<Rectangle> rectangles)
        {
            float allSquares = 0;
            float allX = 0;
            float allY = 0;
            foreach (var rectangle in rectangles)
            {
                allSquares += rectangle.Width * rectangle.Height;
                allX += (rectangle.X + rectangle.Width / 2) * rectangle.Width * rectangle.Height;
                allY += (rectangle.Y + rectangle.Height / 2) * rectangle.Width * rectangle.Height;
            }
            return new PointF(allX / allSquares, allY / allSquares);
        }
    }
}
