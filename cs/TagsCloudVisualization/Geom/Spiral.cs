using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class Spiral
    {
        private const double ThetaDelta = 0.001;


        public readonly Point Center;
        private IEnumerator<PointF> locations;

        public Spiral(Point center)
        {
            Center = center;
            locations = GetLocations();
        }

        private IEnumerator<PointF> GetLocations()
        {
            yield return Center;

            double theta = 0;
            double a = 1;

            while (true)
            {
                float y = (float) (a * theta * Math.Sin(theta) + Center.Y);
                float x = (float) (a * theta * Math.Cos(theta) + Center.X);

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