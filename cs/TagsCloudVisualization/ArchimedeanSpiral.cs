using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class ArchimedeanSpiral
    {
        private double coefficients;

        public ArchimedeanSpiral(double coefficients)
        {
            if (coefficients <= 0)
            {
                throw new ArgumentException("coefficients should be a positive number");
            }

            this.coefficients = coefficients;
        }

        public double GetValuePolar(double angle)
        {
            return coefficients * angle;
        }

        private Point PolarToDecart(double angle, double distance)
        {
            var x = distance * Math.Cos(angle);
            var y = distance * Math.Sin(angle);
            return new Point((int) x, (int) y);
        }

        public IEnumerable<Point> GetIEnumerableDecart(double step)
        {
            if (step <= 0)
                throw new ArgumentException("Step should be a positive number");
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
        public void Constructor_ShouldThrow_ArgumentException_When_notPositiveK()
        {
            Action ctorInvokation = () => new ArchimedeanSpiral(-2);
            ctorInvokation.Should().Throw<ArgumentException>();
        }
    }
}