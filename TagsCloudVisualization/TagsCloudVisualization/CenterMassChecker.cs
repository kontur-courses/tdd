using System.Collections.Generic;
using System.Drawing;


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
                var square = rectangle.Width * rectangle.Height;
                allSquares += square;
                allX += (rectangle.X + rectangle.Width / 2) * square;
                allY += (rectangle.Y + rectangle.Height / 2) * square;
            }
            return new PointF(allX / allSquares, allY / allSquares);
        }
    }
}
