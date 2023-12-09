using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> _rectangles;
        private readonly Spiral _spiral;
        private readonly Point _center;

        public CircularCloudLayouter(Point center)
        {
            _center = center;
            _rectangles = new List<Rectangle>();
            _spiral = new Spiral(center, 2);
        }

        public Rectangle PutNextRectangle(Size sizeRectangle)
        {
            if (sizeRectangle.Width < 0 || sizeRectangle.Height < 0 || sizeRectangle.IsEmpty)
                throw new ArgumentException("Width and Height Size should positive");

            Rectangle rectangle;

            while(true)
            {
                rectangle = new Rectangle(_spiral.NextPoint(), sizeRectangle);
                if(rectangle.IsIntersectOthersRectangles(_rectangles))
                    break;
            }
            MoveRectangleToCenter(ref rectangle);
            _rectangles.Add(rectangle);
            return rectangle;
        }
        

        private void MoveRectangleToCenter(ref Rectangle rectangle)
        {
            MoveRectangleAxis(ref rectangle, rectangle.GetCenter().X, _center.X, 
                new Point(rectangle.GetCenter().X < _center.X ? 1 : -1, 0));
            MoveRectangleAxis(ref rectangle, rectangle.GetCenter().Y, _center.Y, 
                new Point(0, rectangle.GetCenter().Y < _center.Y ? 1 : -1));
        }

        private void MoveRectangleAxis(ref Rectangle newRectangle, int currentPosition, int desiredPosition, Point stepPoint)
        {
            
            while (newRectangle.IsIntersectOthersRectangles(_rectangles)  &&  desiredPosition != currentPosition)
            {
                currentPosition += currentPosition < desiredPosition ? 1 : -1;
                newRectangle.Location = newRectangle.Location.DecreasingCoordinate(stepPoint);
            }
            
            if (!newRectangle.IsIntersectOthersRectangles(_rectangles))
            {
                newRectangle.Location = newRectangle.Location.IncreasingCoordinate(stepPoint);
            }
        }
        public List<Rectangle> Rectangles()
        {
            return _rectangles;
        }
    }
}