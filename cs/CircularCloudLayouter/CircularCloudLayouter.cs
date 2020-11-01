using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CircularCloudLayouter
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> _rectangles;
        private double _spiralAngle;
        private const double SpiralStep = 0.5;
        public readonly Point Center;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            _rectangles = new List<Rectangle>();
        }
        
        public Rectangle GetCurrentRectangle => _rectangles.Last();

        public List<Rectangle> GetRectangles => _rectangles;

        public void PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height == 0 || rectangleSize.Width == 0)
                throw new ArgumentException("Size width and height must be not zero");
            while (true)
            {
                var currentSpiralPosition = GetNewSpiralPoint();
                var rectangle = new Rectangle(currentSpiralPosition, rectangleSize);
                _rectangles.Add(rectangle);
                break;
            }
        }
        
        private Point GetNewSpiralPoint()
        {
            var position = new Point((int)(Center.X + SpiralStep*_spiralAngle * Math.Cos(_spiralAngle)), 
                (int)(Center.Y+SpiralStep*_spiralAngle  * Math.Sin(_spiralAngle)));
            // _spiralParameter += 40 / Math.Log2(_rectangles.Count+2)*Math.PI/180;
            _spiralAngle += 0.017;
            return position;
        } 
    }
}