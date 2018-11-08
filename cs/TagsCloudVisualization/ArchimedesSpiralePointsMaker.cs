using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class ArchimedesSpiralePointsMaker
    {

        public static IEnumerable<Point> PointsMaker(Point center, double spiraleStep)
        {
            yield return center;
            double angle = 0;
            double angleDelta = Math.PI / 90;
            while (true)
            {
                angle += angleDelta;
                var p = spiraleStep * angle;
                var x = p * Math.Sin(angle);
                var y = p * Math.Cos(angle);
                yield return new Point((int)x + center.X, (int)y + center.Y);
            }
            
        }
    }
}
