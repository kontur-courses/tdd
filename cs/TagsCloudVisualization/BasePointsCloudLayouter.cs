using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class BasePointsCloudLayouter : ICloudLayouter
    {
        public struct PointWithDistance
        {
            public int Distance;
            public Point Point;
        }
        
        private static readonly Size InitialQuadTreeHalfSize = new Size(500, 500);
        private Point _layoutCenter;
        private LinkedList<PointWithDistance> _basePointsSortedByDistance = new LinkedList<PointWithDistance>();
        private QuadTree<Rectangle> _rectanglesQuadTree;

        public BasePointsCloudLayouter(Point center)
        {
            _layoutCenter = center;
            
            _basePointsSortedByDistance.AddFirst(new PointWithDistance()
            {
                Distance = 0,
                Point = center 
            });
            
            _rectanglesQuadTree = new QuadTree<Rectangle>(
                new Rectangle(
                    _layoutCenter.X - InitialQuadTreeHalfSize.Width,
                    _layoutCenter.Y - InitialQuadTreeHalfSize.Height,
                    InitialQuadTreeHalfSize.Width * 2,
                    InitialQuadTreeHalfSize.Height * 2));
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle resultRectLocation = null;

            // Search good location for rectangle
            foreach (var basePoint in _basePointsSortedByDistance)
            {
                var basePointRect = CreateRectanglesByBasePoint(basePoint.Point, rectangleSize)
                    .Where(r => !_rectanglesQuadTree.HasNodesInside(r))
                    .OrderBy(r => Point.PowDistance(r.CenterPoint, _layoutCenter))
                    .FirstOrDefault();

                if (basePointRect != null)
                {
                    resultRectLocation = basePointRect;
                    break;
                }
            }
            
            // Result rect points
            var resultRectPoints = new Point[4]
            {
                resultRectLocation.Pos,
                resultRectLocation.BottmRightPoint,
                new Point(resultRectLocation.Pos.X + resultRectLocation.Size.Width, resultRectLocation.Pos.Y),
                new Point(resultRectLocation.Pos.X, resultRectLocation.Pos.Y + resultRectLocation.Size.Height)
            }; 
            
            UpdateBasePoints(resultRectPoints);
            
            _rectanglesQuadTree.Insert(resultRectLocation, resultRectLocation);

            return resultRectLocation;
        }

        public IEnumerable<Rectangle> GetLayout()
        {
            return _rectanglesQuadTree.Nodes;
        }

        private void UpdateBasePoints (IEnumerable<Point> points)
        {
            var newPointsWithDistance = points
                .Select(p => new PointWithDistance() { Distance = Point.PowDistance(p, _layoutCenter), Point = p })
                .OrderBy(p => p.Distance)
                .ToList();

            var i = 0;
            var currentBasePointNode = _basePointsSortedByDistance.First;
            
            while (i < newPointsWithDistance.Count)
            {
                var newPointWithDistance = newPointsWithDistance[i];

                if (_basePointsSortedByDistance.Count == 0)
                {
                    _basePointsSortedByDistance.AddFirst(newPointWithDistance);
                    i++;
                    currentBasePointNode = _basePointsSortedByDistance.First;
                    continue;
                }
                
                if (currentBasePointNode.Next == null)
                {
                    _basePointsSortedByDistance.AddLast(newPointWithDistance);
                    i++;
                    continue;
                }

                if (newPointWithDistance.Point.Equals(_layoutCenter) && _rectanglesQuadTree.Nodes.Count() < 4)
                {
                    i++;
                    continue;
                }
                 
                if (currentBasePointNode.Value.Point.Equals(newPointWithDistance.Point))
                {
                    var nextNode = currentBasePointNode.Next;
                    _basePointsSortedByDistance.Remove(currentBasePointNode);
                    i++;
                    currentBasePointNode = nextNode;
                    continue;
                }
                
                if (newPointWithDistance.Distance <= currentBasePointNode.Value.Distance)
                {
                    _basePointsSortedByDistance.AddBefore(currentBasePointNode, newPointWithDistance);
                    i++;
                    continue;
                }

                currentBasePointNode = currentBasePointNode.Next;
            }
        }

        private List<Rectangle> CreateRectanglesByBasePoint(Point basePoint, Size size)
        {
            var result = new List<Rectangle>();
            result.Add(new Rectangle(basePoint, size));
            result.Add(new Rectangle(new Point(basePoint.X - size.Width, basePoint.Y), size));
            result.Add(new Rectangle(new Point(basePoint.X - size.Width, basePoint.Y - size.Height), size));
            result.Add(new Rectangle(new Point(basePoint.X, basePoint.Y - size.Height), size));
            return result;
        }
    }
}