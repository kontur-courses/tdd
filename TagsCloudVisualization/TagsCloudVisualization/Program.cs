using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;

        public HashSet<Rectangle> Rectangles { get; private set; } = new HashSet<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectSize)
        {

            CheckArguments(rectSize);

            var result = new Rectangle(ChooseRectLocation(rectSize), rectSize);
            Rectangles.Add(result);
            return result;
        }

        private Point ChooseRectLocation(Size rectSize)
        {
            var angle = 0.0;
            var rectCenter = Center;
            while (RectanglesIntersect(rectCenter, rectSize))
            {
                angle += Math.PI / 8;
                var x = (int)(angle * Math.Cos(angle) / (2*Math.PI));
                var y = (int)(angle * Math.Sin(angle) / (2*Math.PI));
                rectCenter = new Point(x, y);
            }
            return countLocation(rectCenter, rectSize);
        }

        private bool RectanglesIntersect(Point rectCenter, Size rectSize)
        {
            var rectangle = new Rectangle(countLocation(rectCenter, rectSize),rectSize);
            foreach (var rect in Rectangles)
                if (rectangle.IntersectsWith(rect))
                    return true;
            return false;
        }

        private Point countLocation(Point rectCenter, Size rectSize)
        {
            var resultX = rectCenter.X - rectSize.Width / 2;
            var resultY = rectCenter.Y - rectSize.Height / 2;
            return new Point(resultX, resultY);
        }

        private void CheckArguments(Size rectSize)
        {
            if (rectSize.Height <= 0 || rectSize.Width <= 0)
                throw new ArgumentException(String.Format("Wrong rectangle size : width={0}, height={1}",
                    rectSize.Width, rectSize.Height));
        }
    }

    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [Test]
        public void Cloud_HaveCenter_ItWasCreatedWith()
        {
            var cloud = new CircularCloudLayouter(new Point(-5, 6));
            cloud.Center.Should().Be(new Point(-5, 6));
        }

        [Test]
        public void Cloud_HaveNoRectangle_BeforeAnyWasPut()
        {
            var cloud = new CircularCloudLayouter(new Point());
            cloud.Rectangles.Should().BeEmpty();
        }

        [TestCase(-1, 1, TestName = "Negative width")]
        [TestCase(1, -1, TestName = "Negative height")]
        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        public void Constructor_ThrowsException_OnWrongArguments(int width, int height)
        {
            var cloud = new CircularCloudLayouter(new Point());
            Action act = () => cloud.PutNextRectangle(new Size(width, height));
            act.ShouldThrow<ArgumentException>();
        }

        [TestCase(4, 2, TestName = "With even size")]
        [TestCase(1, 3, TestName = "With odd size")]
        public void PutNextRectangle_ReturnsCenterRectangle_OneFirstPut(int width, int height)
        {
            var center = new Point(1, -4);
            var cloud = new CircularCloudLayouter(center);
            var rectangleSize = new Size(width, height);
            var expectedLocation = new Point(center.X - width / 2, center.Y - height / 2);
            cloud.PutNextRectangle(rectangleSize).Location.Should().Be(expectedLocation);
        }

        [Test]
        public void TwoPuttedRectangles_HaveDifferentLocations()
        {
            var center = new Point(0,0);
            var cloud = new CircularCloudLayouter(center);
            var rect1 = cloud.PutNextRectangle(new Size(4, 5));
            var rect2 = cloud.PutNextRectangle(new Size(8, 3));
            rect1.Location.Should().NotBe(rect2.Location);
        }

        [Test]
        public void TwoPuttedRectangles_DoNotIntersect()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var rect1 = cloud.PutNextRectangle(new Size(4, 5));
            var rect2 = cloud.PutNextRectangle(new Size(8, 3));
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsNotIntersectingRectangles_On1000SameSizes()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var intersects = false;
            for (var i = 0; i < 100; i++)
            {
                var rectangle = cloud.PutNextRectangle(new Size(2, 3));
                foreach(var rect in cloud.Rectangles)
                    if (!rect.Equals(rectangle))
                        intersects |= rectangle.IntersectsWith(rect);
            }
            intersects.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsNotIntersectingRectangles_On1000DifferentSizes()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var intersects = false;
            for (var i = 0; i < 100; i++)
            {
                var rectangle = cloud.PutNextRectangle(new Size(100-i, 100-i));
                foreach (var rect in cloud.Rectangles)
                    if (!rect.Equals(rectangle))
                        intersects |= rectangle.IntersectsWith(rect);
            }
            intersects.Should().BeFalse();
        }

        [Test, Timeout(1000)]
        public void PutNextRectangle_WorksFast_On100IterationsWithSameSize()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            for (var i = 0; i < 100; i++)
                cloud.PutNextRectangle(new Size(1, 1));
        }

        [Test, Timeout(1000)]
        public void PutNextRectangle_WorksFast_On100IterationsWithDecreasingSize()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            for (var i = 0; i < 100; i++)
                cloud.PutNextRectangle(new Size(100-i, 100-i));
        }
        
        [Test, Timeout(1000)]
        public void PutNextRectangle_WorksFast_On100IterationsWithIncreasingSize()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            for (var i = 0; i < 100; i++)
                cloud.PutNextRectangle(new Size(i+1, i+1));
        }

        [Test, Timeout(1000)]
        public void Constructor_WorksFast_On10000Iterations()
        {
            CircularCloudLayouter cloud;
            var center = new Point(0, 0);
            for (var i = 0; i < 10000; i++)
                cloud = new CircularCloudLayouter(center);
        }

        [Test]
        public void PutNextRectangle_PutsRectanglesCloselyToCenter()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var maxX = 0;
            var maxY = 0;
            var minX = 0;
            var minY = 0;
            var sumWidth = 0;
            var sumHeight = 0;
            for (var i = 0; i < 100; i++)
            {
                var rectangle = cloud.PutNextRectangle(new Size(100 - i, 100 - i));
                if (rectangle.Bottom > maxY)
                    maxY = rectangle.Bottom;
                if (rectangle.Top < minY)
                    minY = rectangle.Top;
                if (rectangle.Right > maxX)
                    maxX = rectangle.Right;
                if (rectangle.Left > minX)
                    minY = rectangle.Left;
                sumWidth += 100 - i;
                sumHeight += 100 - i;
            }
            (maxY < sumHeight && minY > -sumHeight && maxX < sumWidth && minX > -sumWidth).Should().BeTrue();
        }

        [Test]
        public void CircularCloudLayouters_HaveDifferentRectanglesCount()
        {
            var center = new Point(0, 0);
            var cloud1 = new CircularCloudLayouter(center);
            var cloud2 = new CircularCloudLayouter(center);
            cloud1.PutNextRectangle(new Size(1, 2));
            cloud2.Rectangles.Count.Should().NotBe(cloud1.Rectangles.Count);
        }

        [Test]
        public void CircularCloudLayouters_HaveDifferentCenters()
        {
            var center1 = new Point(0, 0);
            var center2 = new Point(1,1);
            var cloud1 = new CircularCloudLayouter(center1);
            var cloud2 = new CircularCloudLayouter(center2);
            cloud2.Center.Should().NotBe(cloud1.Center);
        }
    }
}
