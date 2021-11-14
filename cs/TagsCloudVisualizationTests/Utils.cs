using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudTests
{
    public class Utils
    {
        public Rectangle[] GetRectangles(int count, Func<int, Rectangle> rectanglesFactory)
        {
            return Enumerable.Range(1, count).Select(rectanglesFactory).ToArray();
        }
        
        public Rectangle[] GetRectangles(int count, Func<Rectangle> rectanglesFactory)
        {
            return Enumerable.Range(1, count).Select(_ => rectanglesFactory.Invoke()).ToArray();
        }
    }
}