using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CloudConstruction;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public const double StepAngle = Math.PI / 36;
        public const double ParameterArchimedesSpiral = 10 / (2 * Math.PI);
        public Size WindowSize { get; set; }
        public Point Center { get; set; }
        public double Angle { get; set; }
        public List<Rectangle> Rectangles { get; set; }

        public CircularCloudLayouter(Point center)
        {
            WindowSize = new Size(2000, 2000);
            if (center.X < 0 || center.Y < 0 || center.X > WindowSize.Width || center.Y > WindowSize.Height)
                throw new ArgumentException("Center coordinates must not exceed the window size");
            Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size size)
        {
            var rectangleGenerator= new RectangleGenerator(this);
            var resultRect = rectangleGenerator.GetNextRectangle(size);
            var cloudCompactor = new CloudCompactor(this);
            resultRect = cloudCompactor.ShiftRectangleToTheNearest(resultRect);
            Rectangles.Add(resultRect);
            return resultRect;
        }
    }
}
