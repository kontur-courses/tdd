using System.Collections.Generic;
using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private static readonly Size InitialQuadTreeHalfSize = new Size(500, 500);
        private const double AngleStep = 0.1;
        private const double SpiralStep = 3;
        private Point _layoutCenter;
        private IEnumerator<Point> _pointsEnumerator;
        private QuadTree<Rectangle> _rectanglesQuadTree;

        public CircularCloudLayouter(Point center)
        {
            _pointsEnumerator = GetSpiralPointsEnumerator();
            _layoutCenter = center;
            _rectanglesQuadTree = new QuadTree<Rectangle>(
                new Rectangle(
                    _layoutCenter.X - InitialQuadTreeHalfSize.Width,
                    _layoutCenter.Y - InitialQuadTreeHalfSize.Height,
                    InitialQuadTreeHalfSize.Width * 2,
                    InitialQuadTreeHalfSize.Height * 2));
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException("Size should be positive");
            }
            
            Rectangle currentRect;
            
            while (true)
            {
                _pointsEnumerator.MoveNext();
                var nextPoint = _pointsEnumerator.Current;
                
                var rectangleCenterPoint = new Point(
                    nextPoint.X - rectangleSize.Width / 2,
                    nextPoint.Y - rectangleSize.Height / 2);

                currentRect = new Rectangle(rectangleCenterPoint, rectangleSize);
                
                if (!_rectanglesQuadTree.HasNodesInside(currentRect))
                {
                    _rectanglesQuadTree.Insert(currentRect, currentRect);
                    break;
                }
            }
            
            return currentRect;
        }
        
        public IEnumerable<Rectangle> GetLayout()
        {
            return _rectanglesQuadTree.Nodes;
        }
        
        private IEnumerator<Point> GetSpiralPointsEnumerator()
        {
            yield return _layoutCenter;
            
            var angle = 0.0;
            var radius = 0.0;
            
            while (true)
            {
                var x = _layoutCenter.X + (int)(radius * Math.Cos(angle));
                var y = _layoutCenter.Y + (int)(radius * Math.Sin(angle));
                angle += AngleStep;
                radius += SpiralStep;

                yield return new Point(x,y);
            }
        }
    }
}