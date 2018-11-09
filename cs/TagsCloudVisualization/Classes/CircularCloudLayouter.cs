using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Classes
{
    public class CircularCloudLayouter
    {
        private Point Center { get; set; }
        private RectangleF Surface { get; set; }
        private SpiralGenerator SpiralGenerator { get; set; }
        public List<Rectangle> Rectangles { get;}
      
        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates of the center must be positive numbers");
            Center = center;
            SpiralGenerator = new SpiralGenerator(Center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = SpiralGenerator.GetNextRectangleOnSpiral(rectangleSize);
            
            Rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        public List<Rectangle> GenerateTestLayout()
        {
            var x = 53;
            var y = 10;
            for (var i = 1; i < 50; i++)
            {
                var size = new Size(x, y);
                Rectangles.Add(PutNextRectangle(size));
            }

            return Rectangles;
        }
        
    }
}
