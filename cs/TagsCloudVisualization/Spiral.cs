using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Spiral
    {
        public Double CurrentAngle { get; set; }
        public Double Step { get; set; }
        public Double AngleStep { get; set; }

        public Point Central { get; set; }

        public Spiral(Point centralPoint)
        {
            Central = centralPoint;
            CurrentAngle = 0;
        }

        public Point GetNextPoint()
        {
            var nextX = Central.X + CurrentAngle * Step * Math.Cos(CurrentAngle);
            var nextY = Central.Y + CurrentAngle * Step * Math.Sin(CurrentAngle);
            var roundNextX = Convert.ToInt32(Math.Round(nextX));
            var roundNextY = Convert.ToInt32(Math.Round(nextY));
            var nextPoint = new Point(roundNextX, roundNextY);
            CurrentAngle += AngleStep;
            return nextPoint;

        }
    }
}
