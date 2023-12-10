using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class SpiralDistribution : IDistribution
    {
        public  Point Center { get; private set; }
        public double Radius { get; private set; }
        public double Angle { get; private set; }


        public SpiralDistribution(Point center)
        {
            Center = center;
            Radius = 0;
            Angle = 0;
        }
        public Point GetNextPoint()
        {
            var x = Radius * Math.Cos(Angle) + Center.X;
            var y = Radius * Math.Sin(Angle) + Center.Y;
            
            Angle += 0.1;

            if (Angle >= Math.PI * 2)
            {
                Angle = 0;
                Radius += 0.1;
            }

            return new Point((int)x, (int)y);
        }
    }
}
