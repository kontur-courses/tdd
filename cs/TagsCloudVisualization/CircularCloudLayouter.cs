using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        private readonly List<Rectangle> layout = new List<Rectangle>();
        private Point lastPoint = Point.Empty;
        
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Point Center { get; }
        public IEnumerable<Rectangle> Layout => layout;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rect = default(Rectangle);
            if (layout.Count == 0)
            {
                rect = new Rectangle(Center - rectangleSize.Divide(2),rectangleSize);
                lastPoint = rect.Location + rect.Size.WidhtSize();
            }
            else
            {
                rect = new Rectangle(lastPoint, rectangleSize);
                lastPoint += rectangleSize.WidhtSize();
            }
            layout.Add(rect);
            return rect;
        }    
    }
}
