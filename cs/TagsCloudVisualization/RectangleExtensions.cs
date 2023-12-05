using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Point GetRectangleCenterPoint(this Rectangle rectangle)
        {
            return new Point((rectangle.Left + rectangle.Right) / 2, (rectangle.Top + rectangle.Bottom) / 2);
        }
    }
}
