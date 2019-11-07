using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static PointF GetCenter(this Rectangle rect)
        {
            return new PointF(
                rect.Left + (float)rect.Width / 2, 
                rect.Top + (float)rect.Height / 2);
        }

        public static Rectangle OffsetByMassCenter(this Rectangle rect)
        {
            rect.Offset(-rect.Width / 2, -rect.Height / 2);
            return rect;
        }

        public static void ShiftToNewCenter(
            this List<Rectangle> rectangles, Point oldCenter, Point newCenter)
        {
            var offset = new Point(newCenter.X - oldCenter.X, newCenter.Y - oldCenter.Y);
            foreach (var rect in rectangles)
                rect.Offset(offset);
        }

    }
}
