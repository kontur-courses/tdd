using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0) throw new ArgumentException("center x must be a positive number");
            if (center.Y < 0) throw new ArgumentException("center y must be a positive number");
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0) throw new ArgumentException("width must be a positive number");
            if (rectangleSize.Height < 0) throw new ArgumentException("height must be a positive number");
            var rectangle = new Rectangle(new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2), rectangleSize);

            var phi = 0.0;
            while (IntersectionWithOtherRectangles(rectangle))
            {
                phi += 0.1;
                rectangle.X = center.X + (int)Math.Floor(phi * Math.Cos(phi));
                rectangle.Y = center.Y + (int)Math.Floor(phi * Math.Sin(phi));
            }

            rectangles.Add(rectangle);

            return rectangle;
        }

        private bool IntersectionWithOtherRectangles(Rectangle rect) => rectangles.Exists(rect.IntersectsWith);

        public void Generate()
        {
            var bmp = new Bitmap(center.X * 2 + 100, center.Y * 2 + 100);
            var graphics = Graphics.FromImage(bmp);
            rectangles.ToList().ForEach(r => graphics.DrawRectangle(new Pen(Color.Brown, 6f), r));
            bmp.Save("./example.jpg");
        }
    }

    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private Point center = new Point(500, 500);
        private CircularCloudLayouter circularCloudLayouter;

        [TestCase(-1, 1, "center x must be a positive number*", TestName = "x less than zero")]
        [TestCase(1, -1, "center y must be a positive number*", TestName = "y less than zero")]
        public void Constructor_Throws_Argument_Exception_When
            (int centerX, int centerY, string expectedExceptionMessage)
        {
            Action ctorInvocation = () => new CircularCloudLayouter(new Point(centerX, centerY));
            ctorInvocation.Should().Throw<ArgumentException>().WithMessage(expectedExceptionMessage);
        }

        [TestCase(-1, 1, "width must be a positive number*", TestName = "width less than zero")]
        [TestCase(1, -1, "height must be a positive number*", TestName = "height less than zero")]
        public void PutNextRectangle_Throws_Argument_Exception_When
            (int width, int height, string expectedExceptionMessage)
        {
            var circularCloudLayouter = new CircularCloudLayouter(center);

            Action ctorInvocation = () => circularCloudLayouter.PutNextRectangle(new Size(width, height));
            ctorInvocation.Should().Throw<ArgumentException>().WithMessage(expectedExceptionMessage);
        }

        [SetUp]
        public void SetUp() => circularCloudLayouter = new CircularCloudLayouter(center);

        [Test]
        public void PutNextRectangle_One_Rectangle_Should_place_to_center()
        {
            circularCloudLayouter.PutNextRectangle(new Size(100, 20)).Should().Be(new Rectangle(450, 490, 100, 20));
        }

        [Test]
        public void PutNextRectangle_Rectangles_Should_Not_Intersect()
        {
            var rects = new List<Rectangle>();
            var count = 10;
            while (count-- > 0)
            {
                var newRectangle = circularCloudLayouter.PutNextRectangle(new Size(10, 20));
                rects.All(r => !r.IntersectsWith(newRectangle)).Should().BeTrue();
                rects.Add(newRectangle);
            }
        }

        [Test]
        public void Сloud_Should_has_shape_of_circle()
        {
            var rects = new List<Rectangle>();
            var count = 10;
            while (count-- > 0)
            {
                rects.Add(circularCloudLayouter.PutNextRectangle(new Size(10, 20)));
            }

            var maxRast = 0.0;
            foreach (var rect in rects)
            {
                GetVertexCoordinates(rect).ToList().ForEach(p => maxRast = Math.Max(maxRast,
                    GetDistanceFromPointToCenter(p)));
            }

            maxRast.Should().BeLessThan(20.0 * 2 + 3.0);

        }

        private Point[] GetVertexCoordinates(Rectangle rectangle) => new[]
        {
            new Point(rectangle.Left, rectangle.Top),
            new Point(rectangle.Right, rectangle.Top),
            new Point(rectangle.Left, rectangle.Bottom),
            new Point(rectangle.Right, rectangle.Bottom)
        };

        private double GetDistanceFromPointToCenter(Point p) => Math.Sqrt(Math.Pow(p.X - center.X, 2) + Math.Pow(p.Y - center.Y, 2));
    }
}
