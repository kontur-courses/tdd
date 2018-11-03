using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_Should
    {
        private Point center;
        private CircularCloudLayouter layout;
        private CircularCloudVisualizer visualizer;
        private Size defaultSize;

        [SetUp]
        public void SetUp()
        {
            center = new Point(500, 500);
            var layoutSize = new Size(1000, 1000);
            layout = new CircularCloudLayouter(center, layoutSize);
            visualizer = new CircularCloudVisualizer(layout);
            defaultSize = new Size(100, 50);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
            {
                var directory = TestContext.CurrentContext.TestDirectory;
                var filename = TestContext.CurrentContext.Test.Name;
                var path = $"{directory}\\{filename}.png";
                var bitmap = visualizer.DrawRectangles(layout.Rectangles);
                bitmap.Save(path);
                TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }


        [Test]
        public void CircularCloudLayouter_CreateEmptyLayout_EmptyLayout()
        {
            layout.Center.Should().Be(center);
            layout.Rectangles.Count.Should().Be(0);
        }

        [Test]
        public void PutNextRectangle_PutSingleRectangle_SingleRectangleInCenter()
        {
            var rectangle = layout.PutNextRectangle(defaultSize);
            rectangle.ShouldBeEquivalentTo(new Rectangle(new Point(450, 475), defaultSize));
        }

        [Test]
        public void PutNextRectangle_PutOneRectangle_AddToListOfRectangles()
        {
            layout.PutNextRectangle(defaultSize);
            layout.Rectangles.Count.Should().Be(1);
        }

        [Test]
        public void PutNextRectangle_PutTwoRectangles_RectanglesDoNotIntersect()
        {
            var firstRectangle = layout.PutNextRectangle(defaultSize);
            var secondRectangle = layout.PutNextRectangle(new Size(80, 40));
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_PutMultipleRectangles_RectanglesDoNotIntersect()
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 1000; i++)
            {
                var randomSize = new Size(random.Next(200), random.Next(100));
                var newRectangle = layout.PutNextRectangle(randomSize);
                rectangles.ForEach(rect => rect.IntersectsWith(newRectangle).Should().BeFalse());
                rectangles.Add(newRectangle);
            }
        }

    }
}
