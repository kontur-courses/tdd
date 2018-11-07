using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class ArchimedeanSpiral
    {
        private int k;

        public ArchimedeanSpiral(int k)
        {
            this.k = k;
            

        }

        public double GetValuePolar(double angle)
        {
            return k * angle;
        }

        private Point PolarToDecart(double angle, double distance)
        {
            var x = distance * Math.Cos(angle);
            var y = distance * Math.Sin(angle);
            return new Point((int) x, (int) y);
        }

        public IEnumerable<Point> GetIEnumerableDecart(double step)
        {
            double currentAngle = 0;
            while (true)
            {
                yield return PolarToDecart(currentAngle, GetValuePolar(currentAngle));
                currentAngle += step;
            }
        }

        public IEnumerator<Point> GetIenumeratorDecart(double step)
        {
            return GetIEnumerableDecart(step).GetEnumerator();
        }
        
    }


    [TestFixture]
    public class ArchimedeanSpiral_Tests
    {
        [Test]
        public void DoSomething_WhenSomething()
        {
        }
    }
}