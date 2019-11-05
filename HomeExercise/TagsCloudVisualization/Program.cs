using System.Drawing;
using System.Linq;
using FluentAssertions;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static CircularCloudLayouter _circularCloudLayouter;
        
        public static void Main(string[] args)
        {
            _circularCloudLayouter = new CircularCloudLayouter(new Point());
            RectanglesShouldNotIntersect();
        }
        
        public static void RectanglesShouldNotIntersect()
        {
            var rectangleSize = new Size(5, 2);
            for (var i = 0; i < 5; i++)
                _circularCloudLayouter.PutNextRectangle(rectangleSize);
            var a = 1;
        }
    }
}