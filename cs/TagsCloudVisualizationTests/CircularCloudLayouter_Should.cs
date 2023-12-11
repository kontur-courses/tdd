﻿using System.Drawing;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    class CircularCloudLayouter_Should
    {
        private static CircularCloudLayouter? layouter;

        private string imagePath =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FailedLayout.png";

        [TearDown]
        public void TagCloudVisualizerCircularCloudLayouterTearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed && layouter is not null)
            {
                var failImage = Drawer.GetImage(new Size(1920, 1080), layouter.Rectangles);
                failImage.Save(imagePath);
                Console.WriteLine($"Tag cloud visualization saved to file <{imagePath}>");
            }

            layouter = null;
        }

        [TestCaseSource(typeof(TestDataCircularCloudLayouter),
            nameof(TestDataCircularCloudLayouter.ZeroOrLessHeightOrWidth_Size))]
        public void Throw_WhenPutNewRectangle_WidthOrHeightLessEqualsZero(Size size)
        {
            var action = new Action(() => new CircularCloudLayouter(new Point()).PutNextRectangle(size));
            action.Should().Throw<ArgumentException>()
                .Which.Message.Should().Contain("zero or negative height or width");
        }

        [Test]
        public void RectanglesEmpty_AfterCreation()
        {
            layouter = new CircularCloudLayouter(new Point());
            layouter.Rectangles.Should().BeEmpty();
        }

        [TestCaseSource(typeof(TestDataArchimedeanSpiral), nameof(TestDataArchimedeanSpiral.Different_CenterPoints))]
        public void Add_FirstRectangle_ToCenter(Point center)
        {
            layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(new Size(10, 2));
            layouter.Rectangles.Should().HaveCount(1)
                .And.BeEquivalentTo(new Rectangle(
                    new Point(center.X - 10 / 2, center.Y - 2 / 2), new Size(10, 2)));
        }

        [TestCaseSource(typeof(TestDataArchimedeanSpiral), nameof(TestDataArchimedeanSpiral.Different_CenterPoints))]
        public void AddSeveralRectangles_Correctly(Point centerPoint)
        {
            var amount = 25;
            layouter = CreateLayouter_With_SeveralRectangles(amount, centerPoint);
            layouter.Rectangles.Should().HaveCount(amount);
        }

        [TestCaseSource(typeof(TestDataArchimedeanSpiral), nameof(TestDataArchimedeanSpiral.Different_CenterPoints))]
        public void AddSeveralRectangles_DoNotIntersect(Point centerPoint)
        {
            layouter = CreateLayouter_With_SeveralRectangles(25, centerPoint);
            var rectangles = layouter.Rectangles;
            for (var i = 1; i < rectangles.Count; i++)
                rectangles.Skip(i).All(x => !rectangles[i - 1].IntersectsWith(x)).Should().Be(true);
        }

        [Test]
        public void DensityTest()
        {
            var centerPoint = new Point(960, 540);
            layouter = CreateLayouter_With_SeveralRectangles(4000, centerPoint);
            var rectanglesSquare = 0;
            var radius = 0;
            foreach (var rectangle in layouter.Rectangles)
            {
                rectanglesSquare += rectangle.Width * rectangle.Height;

                var x = Math.Max(
                    Math.Abs(centerPoint.X - rectangle.X),
                    rectangle.X + rectangle.Width - centerPoint.X
                    );
                var y = Math.Max(
                    Math.Abs(centerPoint.Y - rectangle.Y),
                    rectangle.Y + rectangle.Height - centerPoint.Y
                );
                radius = Math.Max(radius, (int)Math.Sqrt(x * x + y * y));
            }

            var circleSquare = Math.PI * radius * radius;
            (rectanglesSquare / circleSquare).Should().BeGreaterOrEqualTo(0.75);
        }

        private static CircularCloudLayouter CreateLayouter_With_SeveralRectangles(int amount, Point center)
        {
            var newLayouter = new CircularCloudLayouter(center);

            for (var i = amount; i > 0; i--)
            {
                var width = i % 40 + 10;
                newLayouter.PutNextRectangle(new Size(width, width / 5));
            }

            return newLayouter;
        }
    }
}
