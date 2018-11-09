using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleExtensions
    {
        public static Point Center(this Rectangle rectangle)=>
            rectangle.Location + rectangle.Size.Divide(2);
        
        public static IEnumerable<Point> Points(this Rectangle rect)
        {
            yield return rect.Location;
            yield return rect.Location + rect.Size.HeightSize();
            yield return rect.Location + rect.Size.WidthSize();
            yield return rect.Location + rect.Size;
        }

    }
}