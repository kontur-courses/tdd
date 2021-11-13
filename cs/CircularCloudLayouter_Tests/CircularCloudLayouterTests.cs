using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CloudLayouter;
using CloudLayouterVisualizer;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using static FluentAssertions.FluentActions;

namespace CircularCloudLayouter_Tests
{
    public class CircularCloudLayouterTests
    {
        private static readonly Point Center = new(500, 500);
        private static readonly Size MinRectangle = new(20, 10);
        private static readonly Size MaxRectangle = new(100, 50);

        private static readonly RandomRectangleGenerator RectangleGenerator =
            new(MinRectangle, MaxRectangle);

        private static readonly Func<int, int, CircularCloudLayouter> CreateLayouter = 
            (x, y) => new CircularCloudLayouter(new Point(x, y));

        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;

        [TearDown]
        public void DrawError()
        {
            var context = TestContext.CurrentContext;

            if (!context.Test.Properties["Category"].Contains("CanBeDrawn") ||
                context.Result.Outcome != ResultState.Failure) return;
            
            var picureLocation = "../../../errors/" + context.Test.Name + ".png";
            using (var visualizer = new Visualizer(
                new Size(Center) * 2, new Pen(Color.Red)))
            {
                visualizer.DrawRectangles(rectangles);
                visualizer.SaveImage(picureLocation);
            }

            var fullpath = new FileInfo(picureLocation).FullName;
            Console.WriteLine($"Tag cloud visualization saved to file {fullpath}");
        }

        [SetUp]
        [Category("CanBeDrawn")]
        public void CreateMaximumLayouter()
        {
            layouter = new CircularCloudLayouter(Center);
            rectangles = new List<Rectangle>();
        }
        

        [TestCase(-5, -5, TestName = "Center in third quadrant")]
        [TestCase(-5, 5, TestName = "Center in second quadrant")]
        [TestCase(5, -5, TestName = "Center in fourth quadrant")]
        [TestCase(0, -5, TestName = "Center on border of third and fourth quadrants")]
        [TestCase(-5, 0, TestName = "Center on border of second and third quadrants")]
        public void Constructor_CenterInAnyQuadrantExceptFirst_ThrowsArgumentException(int x, int y)
        {
            Invoking(() => CreateLayouter(x, y))
                .Should().Throw<ArgumentException>();
        }

        [TestCase(5, 5, TestName = "Canvas is 4x4 and rectangle is 5x5")]
        [TestCase(1, 1, TestName = "Canvas is zero and rectangle is 1x1")]
        [TestCase(5, 1, TestName = "Canvas is 4x0 and rectangle 5x1")]
        [TestCase(1, 5, TestName = "Canvas is 0x4 and rectangle 1x5")]
        public void PutNextRectangle_rectangleSizeBiggerThanCanvas_ThrowsArgumentException(int width, int height)
        {
            Invoking(() => CreateLayouter((width - 1) / 2, (height - 1) / 2)
                    .PutNextRectangle(new Size(width, height)))
                .Should().Throw<Exception>();
        }
        
        [Test]
        public void PutNextRectangle_CanNotPlaceAnyMoreRectangles_ThrowsException()
        {
            var layouter = new CircularCloudLayouter(new Point(5, 5));
            layouter.PutNextRectangle(new Size(9, 9));
            
            Invoking(() => layouter
                    .PutNextRectangle(new Size(2, 2)))
                .Should().Throw<Exception>()
                .WithMessage("Rectangle was placed out side of canvas");
        }
        
        [TestCase(-4, -2, TestName = "width and height are negative numbers")]
        [TestCase(-2, 2, TestName = "width is negative and height is positive")]
        [TestCase(2, -2, TestName = "width is positive and height is negative")]
        [TestCase(0, -5, TestName = "width is zero and height is negative")]
        [TestCase(-7, 0, TestName = "width is negative and height is zero")]
        public void PutNextRectangle_rectangleSizeIsNegative_ThrowsArgumentException(int width, int height)
        {
            Invoking(() => new CircularCloudLayouter(Center)
                    .PutNextRectangle(new Size(width, height)))
                .Should().Throw<ArgumentException>()
                .WithMessage("Given rectangle size is negative");
        }

        [Test]
        public void PutNextRectangle_FirstCall_returnsRectangleInCenter()
        {
            var rectangleSize = new Size(400, 500);
            var position = Center - rectangleSize / 2;

            layouter.PutNextRectangle(rectangleSize).Should().Be(new Rectangle(position, rectangleSize));
        }
        
        [Test, Category("CanBeDrawn")]
        public void PutNextRectangle_100Rectangles_ShouldNotIntersect()
        {
            using (new AssertionScope())
            {
                for (var i = 0; i < 100; i++)
                { 
                    var rectangle = layouter
                        .PutNextRectangle(GetRandomRectangle());

                
                    rectangle.IntersectsWith(rectangles).Should()
                        .BeFalse("Error on iteration: {0}, rectangles should not intersect", i);
                
                    rectangles.Add(rectangle);
                }
            }
        }

        [Test, Category("CanBeDrawn")]
        public void PutNextRectangle_100Rectangles_ShouldBeCompact()
        {
            using (new AssertionScope())
            {
                for (var i = 0; i < 100; i++)
                {
                    var rectangle = layouter.PutNextRectangle(GetRandomRectangle());
                
                    if(i != 0)
                        rectangles
                            .Min(r => GetDistanceBetweenRectangles(r, rectangle))
                            .Should().BeInRange(0, 10, 
                            "Error found at iteration {0}, distance between rectangles is expected to be less than 10", i);
                
                    rectangles.Add(rectangle);
                } 
            }
        }

        private int GetDistanceBetweenRectangles(Rectangle one, Rectangle two)
        {
            var righter = one.X >= two.X ? one : two;
            var lefter = one.X < two.X ? one : two;
            var upper = one.Y <= two.Y ? one : two;
            var lower = one.Y > two.Y ? one : two;

            if (lefter.Left <= righter.Left && righter.Left < lefter.Right)
                return lower.Top - upper.Bottom;
            if (upper.Top <= lower.Top && lower.Top < upper.Bottom)
                return righter.Left - lefter.Right;

            if (righter == upper)
                return (int)new Point(upper.Left, upper.Bottom)
                    .GetDistanceTo(new Point(lower.Right, lower.Top));

            return (int)new Point(upper.Right, upper.Bottom)
                .GetDistanceTo(new Point(lower.Left, lower.Top));
        }
        
        [Test, Timeout(1500)]
        public void PutNextRectangle_10000Calls_ElapsedTimeIsLessThanOneAndAHalfSecond()
        {
            var cloudLayouter = new CircularCloudLayouter(
                new Point(int.MaxValue/2, int.MaxValue/2));

            for (int i = 0; i < 10000; i++)
            {
                cloudLayouter.PutNextRectangle(new Size(10, 10));
            }
        }

        private Size GetRandomRectangle()
        {
            return RectangleGenerator.GetRandomRectangle();
        }
    }
}
