using System.Drawing;

namespace TagsCloudVisualization.Core.Extensions
{
    public static class RectangleExtension
    {   
        public static bool IntersectsWith(this Rectangle sourceRectangle, IEnumerable<Rectangle> destRectangles) 
            => destRectangles.Any(p => p.IntersectsWith(sourceRectangle));
        
        public static Point GetCenter(this Rectangle rectangle)
            => new(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
        
    }
}