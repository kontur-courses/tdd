using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class FirstSector
    {
        private readonly List<Point> _points;

        public FirstSector()
        {
            _points = new List<Point>();
            AddPoint(0, 0);
        }

        public void AddPoint(int x, int y)
        {
            _points.Add(new Point(x, y));
        }

        public List<Point> GetPoints()
        {
            return _points;
        }

        public int Count()
        {
            return _points.Count;
        }
    }
}
