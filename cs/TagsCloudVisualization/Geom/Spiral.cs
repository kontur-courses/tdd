using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Geom
{
    public class Spiral
    {
        private const double ThetaDelta = 0.1;

        private IEnumerator<PointF> locations;
        public readonly Point Center;

        public Spiral(Point center)
        {
            Center = center;
            locations = GetLocations();
        }

        private IEnumerator<PointF> GetLocations()
        {
            double theta = 0;
            while (true)
            {
                float x = (float) (theta * Math.Cos(theta) + Center.X);
                float y = (float) (theta * Math.Sin(theta) + Center.Y);

                yield return new PointF(x, y);

                theta += ThetaDelta;
            }
        }

        public PointF GetNextLocation()
        {
            locations.MoveNext();
            return locations.Current;
        }
    }
}