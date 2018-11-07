using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    class TagsCloudVisualization
    {
        static void Main(string[] args)
        {
            var allSizes = new List<Size>();
            var rnd = new Random();
            var height = 1080;
            var width = 1920;
            for (int i = 0; i < 60; i++)
            {
                //var nextHeight = rnd.Next(40, 50);
                var nextHeight = 40;
                //var nextWidth = rnd.Next(nextHeight * 2, nextHeight * 6);
                var nextWidth = 160;
                allSizes.Add(new Size(nextWidth, nextHeight));
            }


            var cilkLayouter = new CircularCloudLayouter(new Point(width / 2 - 100, height / 2));
            foreach (var r in allSizes)
            {
                cilkLayouter.PutNextRectangle(r);
            }

            cilkLayouter.VisualizeInPicture("img.png", new Size(width, height));
        }
    }

    public class CircularCloudLayouter
    {
        private Point center;
        private IEnumerator<Point> spiralEnumerator;
        public List<Rectangle> addedRectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            var x = center.X;
            var y = center.Y;
            if (x < 0 || y < 0)
                throw new ArgumentException();

            var spiral = new ArchimedeanSpiral(1);
            spiralEnumerator = spiral.GetIenumeratorDecart(0.1);
            this.center = center;
        }

        private Point GetNextPoint()
        {
            spiralEnumerator.MoveNext();
            return new Point(spiralEnumerator.Current.X + center.X, spiralEnumerator.Current.Y + center.Y);
        }

        private bool RectanglesAreIntersecting(Rectangle rectangle)
        {
            foreach (var rect in addedRectangles)
            {
                if (rect.IntersectsWith(rectangle))
                {
                    return true;
                }
            }

            return false;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var currentPoint = center;
            var currentRectangle = new Rectangle(center.X, center.Y, rectangleSize.Width, rectangleSize.Height);

            while (RectanglesAreIntersecting(currentRectangle))
            {
                currentPoint = GetNextPoint();
                currentRectangle = new Rectangle
                    (currentPoint.X, currentPoint.Y, rectangleSize.Width, rectangleSize.Height);
            }


            addedRectangles.Add(currentRectangle);
            return currentRectangle;
        }

        public void VisualizeInPicture(string pictureName, Size pictureSize)
        {
            var btm = new Bitmap(pictureSize.Width, pictureSize.Height);
            var obj = Graphics.FromImage(btm);
            foreach (var r in addedRectangles)
            {
                obj.DrawRectangle(new Pen(Color.Brown), r);
            }

            btm.Save(pictureName);
        }
    }

    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private List<Rectangle> putedRectangles;
        private CircularCloudLayouter circularCloudLayouter;

        [SetUp]
        public void SetUp()
        {
            putedRectangles = new List<Rectangle>();
            circularCloudLayouter = new CircularCloudLayouter(new Point(100, 100));
        }

        [TestCase(-1, 1, TestName = "X less than 0")]
        [TestCase(1, -1, TestName = "Y less than 0")]
        public void ctor_ThrowsArgumentExceptionWhen(int centerX, int centerY)
        {
            Action act = () => new CircularCloudLayouter(new Point(centerX, centerY));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ReturnsTwoNonIntersectRectangles()
        {
            var rect1 = circularCloudLayouter.PutNextRectangle(new Size(50, 10));
            var rect2 = circularCloudLayouter.PutNextRectangle(new Size(70, 140));
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [TestCase(4)]
        [TestCase(16)]
        [TestCase(65)]
        [TestCase(100)]
        public void PutNextRectangle_ReturnsManyNonIntersectRectangles(int rectanglesCount)
        {
            for (var i = 0; i < rectanglesCount; i++)
            {
                putedRectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(50, 10)));
            }

            for (var i = 0; i < putedRectangles.Count; i++)
            {
                for (var j = 0; j < putedRectangles.Count; j++)
                {
                    if (i != j)
                    {
                        putedRectangles[i].IntersectsWith(putedRectangles[j]).Should().BeFalse();
                    }

                    
                }
            }
        }

        [Test]
        public void PutNextRectangle_ReturnsDifferentRectangles()
        {
            var rect1 = circularCloudLayouter.PutNextRectangle(new Size(50, 10));
            var rect2 = circularCloudLayouter.PutNextRectangle(new Size(70, 140));
            rect1.Should().NotBe(rect2);
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(40)]
        [TestCase(100)]
        public void PutNextRectangle_ReturnsManyRectanglesInscribedInACircle(int rectanglesCount)
        {
            var center = new Point(100, 100);

            var totalSquare = 0;
            for (var i = 0; i < rectanglesCount; i++)
            {
                var x = 50;
                var y = 10;
                putedRectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(x, y)));
                totalSquare += x * y;
            }

            var r = (int) Math.Sqrt(totalSquare / Math.PI);
            foreach (var rect in putedRectangles)
            {
                var distance = Math.Sqrt(Math.Pow(rect.X - center.X, 2) + Math.Pow(rect.Y - center.Y, 2));
                distance.Should().BeLessThan(r * 1.2);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status.)
            {
                circularCloudLayouter.VisualizeInPicture("log.png", new Size(1920, 1080));
            }
        }
    }
}