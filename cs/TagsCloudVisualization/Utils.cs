using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Utils
    {
        private static bool IsApproximatelyEquals(this double me, double val)
        {
            return Math.Abs(me - val) < 1E-6;
        }

        private static bool IsApproximatelyMoreThan(this double me, double val)
        {
            return me > val || me.IsApproximatelyEquals(val);
        }

        private static bool IsApproximatelyLessThan(this double me, double val)
        {
            return me < val || me.IsApproximatelyEquals(val);
        }

        private class Segment
        {
            public Point p1, p2;
            public Segment(Point p1, Point p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }
        }

        /// <summary>
        /// Пересекает ли указанный прямоугольник луч из центра координат под углом rayAngle
        /// (проверяются дальние от начала координат стороны прямоугольника)
        /// </summary>
        /// <param name="rayAngle">Угол ПО ЧАСОВОЙ стрелке, т.к. ордината идет вниз, рад</param>
        public static bool IsRayIntersectsRectangle(Rectangle rect, double rayAngle, out Point IntersectionPoint)
        {
            var vLeftTop = new Point(rect.Left, rect.Top);
            var vRightTop = new Point(rect.Right, rect.Top);
            var vRightBottom = new Point(rect.Right, rect.Bottom);
            var vLeftBottom = new Point(rect.Left, rect.Bottom);

            Segment hSeg, vSeg;
            if (rayAngle >= 1.5 * Math.PI)
            {
                hSeg = new Segment(vLeftTop, vRightTop);
                vSeg = new Segment(vRightTop, vRightBottom);
            }
            else if (rayAngle >= Math.PI)
            {
                hSeg = new Segment(vLeftTop, vRightTop);
                vSeg = new Segment(vLeftTop, vLeftBottom);
            }
            else if (rayAngle >= 0.5 * Math.PI)
            {
                hSeg = new Segment(vLeftBottom, vRightBottom);
                vSeg = new Segment(vLeftTop, vLeftBottom);
            }
            else
            {
                hSeg = new Segment(vLeftBottom, vRightBottom);
                vSeg = new Segment(vRightTop, vRightBottom);
            }

            //IsRayIntersectsHorizontal
            if (rayAngle.IsApproximatelyEquals(1.5 * Math.PI) && hSeg.p1.Y < 0)
            {
                if (hSeg.p1.X <= 0 && hSeg.p2.X >= 0)
                {
                    IntersectionPoint = new Point(0, hSeg.p1.Y);
                    return true;
                }
            }
            else if (rayAngle.IsApproximatelyEquals(0.5 * Math.PI) && hSeg.p1.Y > 0)
            {
                if (hSeg.p1.X <= 0 && hSeg.p2.X >= 0)
                {
                    IntersectionPoint = new Point(0, hSeg.p1.Y);
                    return true;
                }
            }
            else if ((rayAngle > Math.PI && hSeg.p1.Y < 0) || (rayAngle < Math.PI && hSeg.p1.Y > 0))
            {
                double x = hSeg.p1.Y / Math.Tan(rayAngle);
                if (x.IsApproximatelyMoreThan(hSeg.p1.X) && x.IsApproximatelyLessThan(hSeg.p2.X))
                {
                    IntersectionPoint = new Point((int)x, hSeg.p1.Y);
                    return true;
                }
            }

            //IsRayIntersectsVertical
            if (rayAngle.IsApproximatelyEquals(Math.PI) && vSeg.p1.X < 0)
            {
                if (vSeg.p1.Y <= 0 && vSeg.p2.Y >= 0)
                {
                    IntersectionPoint = new Point(vSeg.p1.X, 0);
                    return true;
                }
            }
            else if (rayAngle.IsApproximatelyEquals(0) && vSeg.p1.X > 0)
            {
                if (vSeg.p1.Y <= 0 && vSeg.p2.Y >= 0)
                {
                    IntersectionPoint = new Point(vSeg.p1.X, 0);
                    return true;
                }
            }
            else if ((rayAngle > Math.PI && vSeg.p1.Y < 0) || (rayAngle < Math.PI && vSeg.p1.Y > 0))
            {
                double x = vSeg.p1.Y / Math.Tan(rayAngle);
                if (x.IsApproximatelyMoreThan(vSeg.p1.X) && x.IsApproximatelyLessThan(vSeg.p2.X))
                {
                    IntersectionPoint = new Point((int)x, vSeg.p1.Y);
                    return true;
                }
            }

            IntersectionPoint = default;
            return false;
        }

        /// <summary>
        /// Длина отрезка под углом rayAngle из центра прямоугольника до пересечения со стороной
        /// </summary>
        /// <param name="rayAngle">Угол ПО ЧАСОВОЙ стрелке, т.к. ордината идет вниз, рад</param>
        public static double GetRayLengthFromCenter(Rectangle rect, double rayAngle)
        {
            rect.X = -rect.Width / 2;
            rect.Y = -rect.Height / 2;
            if (IsRayIntersectsRectangle(rect, rayAngle, out Point point))
                return Math.Sqrt(point.X * point.X + point.Y * point.Y);
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Конвертация полярных координат в декартовы
        /// </summary>
        /// <param name="rayAngle">Угол ПО ЧАСОВОЙ стрелке, т.к. ордината идет вниз, рад</param>
        /// <param name="dist">Расстояние</param>
        /// <returns></returns>
        public static Point GetPointByAngleAndDistance(double rayAngle, double dist)
        {
            return new Point((int)(dist * Math.Cos(rayAngle)), (int)(dist * Math.Sin(rayAngle)));
        }

        public static Point GetFathestPointFromCenter(Rectangle rect, out double dist)
        {
            var vLeftTop = new Point(rect.Left, rect.Top);
            var vRightTop = new Point(rect.Right, rect.Top);
            var vRightBottom = new Point(rect.Right, rect.Bottom);
            var vLeftBottom = new Point(rect.Left, rect.Bottom);

            double distSquared(Point p) => p.X * p.X + p.Y * p.Y;

            List<KeyValuePair<Point, double>> vertices = new List<KeyValuePair<Point, double>>()
            {
                new KeyValuePair<Point, double>(vLeftTop, distSquared(vLeftTop)),
                new KeyValuePair<Point, double>(vRightTop, distSquared(vRightTop)),
                new KeyValuePair<Point, double>(vLeftBottom, distSquared(vLeftBottom)),
                new KeyValuePair<Point, double>(vRightBottom, distSquared(vRightBottom))
            };

            double maxDistSquared = vertices.Max(kv => kv.Value);
            dist = Math.Sqrt(maxDistSquared);
            return vertices.First(kv => kv.Value == maxDistSquared).Key;
        }
    }

    [TestFixture]
    public class Utils_Should
    {
        [TestCase(50, -5, 0)]
        [TestCase(-5, 50, Math.PI / 2)]
        [TestCase(-50, -5, Math.PI)]
        [TestCase(-5, -50, 1.5 * Math.PI)]
        [TestCase(50, 50, Math.PI / 4)]
        [TestCase(-50, 50, Math.PI * 3 / 4)]
        [TestCase(-50, -50, Math.PI * 1.25)]
        [TestCase(50, -50, 1.76 * Math.PI)]
        public void IsRayIntersectsRectangle_ShouldReturnTrue(int rectX, int rectY, double rayAngle)
        {
            Utils.IsRayIntersectsRectangle(new Rectangle(rectX, rectY, 10, 10), rayAngle, out Point _).Should().BeTrue();
        }

        [TestCase(50, 20, 0, ExpectedResult = 25)]
        [TestCase(10, 10, Math.PI / 4, ExpectedResult = 7)]
        public int GetRayLengthFromCenter_CorrectCalculation(int width, int height, double rayAngle)
        {
            return (int)Utils.GetRayLengthFromCenter(new Rectangle(0, 0, width, height), rayAngle);
        }

        [TestCase(0, 10, 10, 0)]
        [TestCase(Math.PI, 10, -10, 0)]
        [TestCase(Math.PI / 4, 10, 7, 7)]
        public void GetPointByAngleAndDistance_CorrectCalculation(double rayAngle, double dist, int x, int y)
        {
            Utils.GetPointByAngleAndDistance(rayAngle, dist).Should().Be(new Point(x, y));
        }

        [TestCase(50, 50, 60, 60)]
        [TestCase(-50, 50, -50, 60)]
        [TestCase(-50, -50, -50, -50)]
        [TestCase(50, -50, 60, -50)]
        public void GetFathestPointFromCenter_CorrectCalculation(int rectX, int rectY, int pointX, int pointY)
        {
            Utils.GetFathestPointFromCenter(new Rectangle(rectX, rectY, 10, 10), out double _).Should().Be(new Point(pointX, pointY));
        }
    }
}
