using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Drawing;

namespace BowlingGame
{
    class Spiral
    {
        double t = 0;
        double angleSpeed;
        double counter;
        double linearSpeed;
        Point center;
        public Spiral(Point Center, double angleSpeed = 0.108, double linearSpeed = 0.032)
        {
            this.linearSpeed = linearSpeed;
            this.angleSpeed = angleSpeed;
            center = Center;
        }
        public void Increase()
        {
            counter += linearSpeed;
            t += angleSpeed;
        }
        public double X
        {
            get
            {
                return Math.Cos(t) * counter + center.X;
            }
        }
        public double Y
        {
            get
            {
                return Math.Sin(t) * counter + center.Y;
            }
        }
        public Point CurrentPoint
        {
            get
            {
                return new Point((int)X, (int)Y);
            }
        }
    }
    class CircularCloudLayouter
    {
        Spiral spiral;
        Point Center;
        List<Rectangle> rectangles = new List<Rectangle>();

        public bool IsCrossingAny(Rectangle rectangle)
        {
            return rectangles.Select(x => IsCrossing(x, rectangle)).Any(x => x);
        }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiral = new Spiral(center);
        }
        public CircularCloudLayouter(double angleSpeed, double linearSpeed, int size)
        {
            Center = new Point(size/2, size/2);
            spiral = new Spiral(Center, angleSpeed, linearSpeed);
        }
        
        public Rectangle CreateRectangleFromSpiral(Size rectangleSize)
        {
            var locationX = spiral.CurrentPoint.X - rectangleSize.Width / 2;
            var locationY = spiral.CurrentPoint.Y - rectangleSize.Height / 2;
            var point = new Point(locationX, locationY);
            return new Rectangle(point, rectangleSize);
        }
        public Rectangle PutNewRectangle(Size rectangleSize)
        {
            var rectangle = CreateRectangleFromSpiral(rectangleSize);
            while (IsCrossingAny(rectangle))
            {
                spiral.Increase();
                rectangle = CreateRectangleFromSpiral(rectangleSize);
            }
            rectangles.Add(rectangle);
            return rectangle;
        }
        public bool IsCrossing(Rectangle r1, Rectangle r2)
        {
            var topAndBottomOfR1IsDownToR2 = r1.Top < r2.Top && r1.Top < r2.Bottom && r1.Bottom < r2.Bottom && r1.Bottom < r2.Top;
            var topAndBottomOfR1IsUpToR2 = r1.Top > r2.Top && r1.Top > r2.Bottom && r1.Bottom > r2.Bottom && r1.Bottom > r2.Top;
            var leftAndRightOfR1IsLefterToR2 = r1.Left < r2.Left && r1.Left < r2.Right && r1.Right < r2.Left && r1.Right < r2.Right;
            var leftAndRightOfR1IsRighterToR2 = r1.Left > r2.Left && r1.Left > r2.Right && r1.Right > r2.Left && r1.Right > r2.Right;
            return !(topAndBottomOfR1IsDownToR2 || topAndBottomOfR1IsUpToR2 || leftAndRightOfR1IsLefterToR2 || leftAndRightOfR1IsRighterToR2);
        }

        //public void GetBitmap()
        //{
        //    var bitmap = new Bitmap(1000, 1000);
        //    var g = Graphics.FromImage(bitmap);
        //    foreach (var rectangle in rectangles)
        //    {
        //        g.DrawRectangle(new Pen(Brushes.Black), rectangle);
        //    }
        //    bitmap.Save("bitmap.bmp");
        //}
        public double GetDistance(Point p1, Point p2)
        {
            var vector = new Point(p1.X - p2.X, p1.Y - p2.Y);
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }
    }


    class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter CCL = new CircularCloudLayouter(new Point(0, 0));
        [Test]
        public void GetDistance_OnPoints_ShouldReturnCorrectResult()
        {
            CCL.GetDistance(new Point(3, 0), new Point(0, 4)).Should().BeApproximately(5, 0.00001);
        }
        [Test]
        public void PutNewRectangle_WhenAddingFirstRectangle_ShouldAddItOnCenter()
        {
            var CCL = new CircularCloudLayouter(new Point(0, 0));
            CCL.PutNewRectangle(new Size(20, 10)).Should().Be(new Rectangle(-10, -5, 20, 10));
        }
        [Test]
        public void IsCrossing_WhenTwoCrossingRectangles_ShouldReturnTrue()
        {
            CCL.IsCrossing(new Rectangle(-10, -10, 20, 20), new Rectangle(0, 0, 20, 20)).Should().BeTrue();
        }
        [Test]
        public void IsCrossing_WhenTwoNotCrossingRectangles_ShouldReturnFalse()
        {
            CCL.IsCrossing(new Rectangle(-100, -100, 10, 10), new Rectangle(100, 100, 10, 10)).Should().BeFalse();
        }
        [Test]
        public void IsCrossing_WhenTwoNotCrossingRectanglesHardCase_ShouldReturnFalse()
        {
            CCL.IsCrossing(new Rectangle(-20, -10, 40, 10), new Rectangle(-10, 1, 20, 10)).Should().BeFalse();
        }
        [Test]
        public void IsCrossing_WhenOneInsideAnother_ShouldReturnTrue()
        {
            CCL.IsCrossing(new Rectangle(-20, -20, 40, 40), new Rectangle(-10, -10, 20, 20));
        }
        [Test]
        public void PutNewRectangle_PutMultiple1x1Rectangles_ShouldBeAlmostCircle()
        {
            var size = new Size(1, 1);
            var CLL = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < 1000; i++)
            {
                rectangles.Add(CLL.PutNewRectangle(size));
            }
            /*В данном случае я знаю детали реализации алгоритма,
             * поэтому могу сделать следующее: очевидно, что площадь полученной фигуры будет 1000 единиц.
             * Если бы это был круг, его радиус бы был равен 17.84, а длина окружности 112. 
             Возьмём 112 последних прямоугольников и измерим максимальное расстояние и минимальное (от центра),
            разделим меньшее на большее. Круглость круга составляет 1, круглость квадрата примерно 0.7, а круглость
            восьмиугольника примерно 0.92. Таким образом будем считать, что как минимум 0.9 нас устроит.
            */
            double min = 1000000d;
            double max = 0d;
            for (int i = 888; i < 1000; i++)
            {
                var distance = CCL.GetDistance(new Point(0, 0), rectangles[i].Location);
                if (distance > max)
                {
                    max = distance;
                }
                if (distance < min)
                {
                    min = distance;
                }
            }
            var roundness = min / max;
            roundness.Should().BeGreaterThan(0.90);
        }
    }
}
