using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly Point _center;
        private int _radius = 0;
        private HashSet<Point> _points = new HashSet<Point>();
        
        public Spiral(Point center)
        {
            _center = center;
        }
        
        public IEnumerable<Point?> GetPoints()
        {
            while (true)
            {
                for (var i = _radius; i >= - _radius; i--)
                for (var j = _radius; j >= - _radius; j--)
                {
                    var point = new Point(i, j);
                    if (Math.Pow(i, 2) + Math.Pow(j, 2) <= Math.Pow(_radius, 2) && !_points.Contains(point))
                    {
                        _points.Add(point);
                        yield return new Point(i + _center.X, j + _center.Y);
                    }
                }
                _radius++;
            }
        }
    }
}