using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ApprovalTests;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Test_CircularCloudLayouter
    {
        private ConcurrentDictionary<string, CircularCloudLayouter> dictionaryCloudLayouter =
            new ConcurrentDictionary<string, CircularCloudLayouter>();

        [SetUp]
        public void AddInDictionary()
        {
            dictionaryCloudLayouter[TestContext.CurrentContext.Test.Name] =
                new CircularCloudLayouter(new Point(450, 450));
        }

        [TearDown]
        public void SaveImage()
        {
            var testContext = TestContext.CurrentContext;
            if (testContext.Result.Outcome.Status != TestStatus.Failed)
                return;

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestBug");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, $"{testContext.Test.Name}.png");
            var cloudLayouter = dictionaryCloudLayouter[testContext.Test.Name];
            cloudLayouter.Visualization().Save(path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        [TestCase(0, 0)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -1)]
        public void PutNextRectangle_OnNegativeSize_ShouldThrowArgExcept(int width, int height)
        {
            var cloudLayouter = new CircularCloudLayouter();
            Action action = () => cloudLayouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnSomeRectangleWithRightSize()
        {
            var cloudLayouter = new CircularCloudLayouter();
            cloudLayouter.PutNextRectangle(new Size(7, 5)).Size.Should().Be(new Size(7, 5));
        }

        [Test]
        public void PutNextRectangle_AnyTwoRectanglesNotHaveIntersect()
        {
            var cloudLayouter = new CircularCloudLayouter();
            var random = TestContext.CurrentContext.Random;
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 50; i++)
                rectangles.Add(cloudLayouter.PutNextRectangle(new Size(random.Next(1, 100), random.Next(1, 100))));

            for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

        [Test]
        public void CircularCloudLayout_AnyTwoRectanglesNotHaveIntersectAfterCentreings()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(450, 450));
            var random = TestContext.CurrentContext.Random;
            for (var i = 0; i < 50; i++)
                cloudLayouter.PutNextRectangle(new Size(random.Next(1, 100), random.Next(1, 100)));

            var rectanglesHash = cloudLayouter.Centreings();
            var rectangles = new List<Rectangle>(rectanglesHash);

            for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

    }
}