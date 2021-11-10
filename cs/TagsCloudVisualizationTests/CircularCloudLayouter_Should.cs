using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    class LayoutTestMarkerAttribute : Attribute {}

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private Point layouterCenterPoint;
        private Size[] rectanglesSizes;
        private const string ExceptionsFolder = "Exceptions";

        [TearDown]
        public void OnTearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status == TestStatus.Failed &&
                IsTestMarkedAsLayoutTest(context.Test.Name))
            {
                var layouter = new CircularCloudLayouter(layouterCenterPoint);
                var bitmapWidth = this.rectanglesSizes.Sum(x => x.Width) + Math.Abs(layouterCenterPoint.X);
                var bitmapHeight = this.rectanglesSizes.Sum(x => x.Height) + Math.Abs(layouterCenterPoint.Y);
                var visualizer = new Visualizer(layouter, new Random(123), bitmapWidth, bitmapHeight);
                var image = visualizer.CreateImageFromRectangles(rectanglesSizes);
                var path = Path.Join(Environment.CurrentDirectory, ExceptionsFolder, context.Test.Name + ".jpg");
                Directory.CreateDirectory(ExceptionsFolder);
                File.WriteAllBytes(path, image);

                TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [Test]
        public void NotThrow_InConstructor()
        {
            var startPoint = new Point();
            Action lambda = () => new CircularCloudLayouter(startPoint);
            lambda.Should().NotThrow();
        }

        [Test]
        public void ReturnRectangleWithCenterInStartPosition_OnFirstIteration()
        {
            var startPoint = new Point(100, 200);
            var size = new Size(500, 300);
            var expectedRectangle = new Rectangle(startPoint - size / 2, size);
            var actualRectangle = new CircularCloudLayouter(startPoint).PutNextRectangle(size);
            actualRectangle.Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void ReturnRectanglesAroundCenter_WhenSquaresWithSameSizeGiven()
        {
            var center = new Point();
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(2, 2);
            var startPoint = center - size / 2;
            var horizontalOffset = new Size(size.Width, 0);
            var verticalOffset = new Size(0, size.Height);
            var expected = new List<Rectangle>()
            {
                new Rectangle(startPoint, size),
                new Rectangle(startPoint + horizontalOffset, size),
                new Rectangle(startPoint + horizontalOffset - verticalOffset, size),
                new Rectangle(startPoint + horizontalOffset + verticalOffset, size),
                new Rectangle(startPoint - horizontalOffset, size),
                new Rectangle(startPoint - horizontalOffset + verticalOffset, size),
                new Rectangle(startPoint - horizontalOffset - verticalOffset, size),
                new Rectangle(startPoint - verticalOffset, size),
                new Rectangle(startPoint + verticalOffset, size)
            };

            var actual = Enumerable
                .Range(1, expected.Count)
                .Select(x => layouter.PutNextRectangle(size))
                .ToList();
            actual.Should().BeEquivalentTo(expected,
                config =>
                    config.WithoutStrictOrdering());
        }

        [Test]
        [LayoutTestMarker]
        public void ReturnNotIntersectedRectangles()
        {
            var rectanglesCount = 50;
            var startPoint = new Point(100, 200);
            var sizes = new Size[rectanglesCount];
            var random = new Random(12345);
            for (var i = 0; i < rectanglesCount; i++)
            {
                sizes[i] = new Size(random.Next(1, 100), random.Next(1, 100));
            }

            InitializeLayoutMarkedTest(startPoint, sizes);

            var layouter = new CircularCloudLayouter(startPoint);

            var rectangles = sizes.Select(size => layouter.PutNextRectangle(size)).ToArray();
            for (int i = 0; i < rectanglesCount; i++)
            {
                for (int j = i + 1; j < rectanglesCount; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        [LayoutTestMarker]
        public void ReturnRectanglesWithTightDistribution()
        {
            var startPoint = new Point(100, 200);
            var sizes = new Size[]
            {
                new Size(100, 100),
                new Size(50, 100),
                new Size(100, 50),
                new Size(100, 200),
                new Size(200, 100),
                new Size(150, 150),
                new Size(300, 300),
                new Size(400, 400)
            };
            InitializeLayoutMarkedTest(startPoint, sizes);

            var layouter = new CircularCloudLayouter(startPoint);
            var allowedMaxDistance = 1000;

            var rectangles = sizes.Select(x => layouter.PutNextRectangle(x)).ToList();
            var maxDistance = 0d;
            foreach (var rect1 in rectangles)
            {
                foreach (var rect2 in rectangles)
                {
                    var deltaLocation = rect1.Location - (Size) rect2.Location;
                    maxDistance = Math.Max(maxDistance,
                        Math.Sqrt(deltaLocation.X * deltaLocation.X) + Math.Sqrt(deltaLocation.Y * deltaLocation.Y));
                }
            }

            maxDistance.Should().BeLessThan(allowedMaxDistance);
        }

        //Здесь я взял несколько рабочих тестов и приписал Assert.Fail(), чтобы протестировать пункт 3 из задачи
        [Test]
        [LayoutTestMarker]
        public void FailingTest1()
        {
            var sizes = new Size[]
            {
                new(10, 10),
                new(20, 30)
            };
            var startPoint = new Point(100, 200);

            InitializeLayoutMarkedTest(startPoint, sizes);
            Assert.Fail();
        }

        [Test]
        [LayoutTestMarker]
        public void FailingTest2()
        {
            var rectanglesCount = 50;
            var startPoint = new Point(100, 200);
            var sizes = new Size[rectanglesCount];
            var random = new Random(12345);
            for (var i = 0; i < rectanglesCount; i++)
            {
                sizes[i] = new Size(random.Next(1, 100), random.Next(1, 100));
            }
            InitializeLayoutMarkedTest(startPoint, sizes);
            var layouter = new CircularCloudLayouter(startPoint);

            var rectangles = sizes.Select(size => layouter.PutNextRectangle(size)).ToArray();
            
            for (int i = 0; i < rectanglesCount; i++)
            {
                for (int j = i + 1; j < rectanglesCount; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
            Assert.Fail();
        }

        [Test]
        [LayoutTestMarker]
        public void FailingTest3()
        {
            var startPoint = new Point(100, 200);
            var sizes = new Size[]
            {
                new Size(100, 100),
                new Size(50, 100),
                new Size(100, 50),
                new Size(100, 200),
                new Size(200, 100),
                new Size(150, 150),
                new Size(300, 300),
                new Size(400, 400)
            };
            InitializeLayoutMarkedTest(startPoint, sizes);

            var layouter = new CircularCloudLayouter(startPoint);
            var allowedMaxDistance = 1000;

            var rectangles = sizes.Select(x => layouter.PutNextRectangle(x)).ToList();
            var maxDistance = 0d;
            foreach (var rect1 in rectangles)
            {
                foreach (var rect2 in rectangles)
                {
                    var deltaLocation = rect1.Location - (Size) rect2.Location;
                    maxDistance = Math.Max(maxDistance,
                        Math.Sqrt(deltaLocation.X * deltaLocation.X) + Math.Sqrt(deltaLocation.Y * deltaLocation.Y));
                }
            }

            Assert.Fail();
        }

        private void InitializeLayoutMarkedTest(Point center, Size[] sizes)
        {
            this.layouterCenterPoint = center;
            this.rectanglesSizes = sizes;
        }

        private bool IsTestMarkedAsLayoutTest(string testName)
        {
            var type = typeof(CircularCloudLayouter_Should);
            var method = type.GetMethod(testName);
            var attribute = method?.GetCustomAttribute<LayoutTestMarkerAttribute>();
            return attribute != null;
        }
    }
}