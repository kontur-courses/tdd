using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class ArithmeticSpiral
    {
        private int x, y;
        private double angle;
        private int constant, density;

        public ArithmeticSpiral(Point start, int constant= 1,int density=1 )
        {
            x = start.X;
            y = start.Y;
            this.constant= constant;
            this.density = density;
        }

        public Point GetPoint()
        {
            var nextPoint = new Point((int)(x + Math.Cos(angle) * angle * constant),
                (int)(y + Math.Sin(angle) * angle));
            angle += Math.PI / (360 * density);
            return nextPoint;
        }
    }
}
