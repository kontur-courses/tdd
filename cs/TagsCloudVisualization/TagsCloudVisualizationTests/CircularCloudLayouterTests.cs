using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [SetUp]
        public void SetUp()
        {
            center = new Point();
            distribution = new SpiralDistribution(center);
            tagsCloud = new CircularCloudLayouter(center, distribution);
            drawer = new CloudLayouterDrawer(10);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var fileName = $"{TestContext.CurrentContext.Test.FullName}.png";
                drawer.DrawCloud(fileName, tagsCloud.WordPositions);
                Console.WriteLine($"Tag cloud visualization saved to file /images/{fileName}");
            }
        }

        private Point center;
        private CircularCloudLayouter tagsCloud;
        private SpiralDistribution distribution;
        private CloudLayouterDrawer drawer;

        [Test]
        public void CircularCloudLayouter_Initialize_Params()
        {
            tagsCloud.WordPositions.Count.Should().Be(0);
            tagsCloud.Center.Should().Be(center);
            tagsCloud.Distribution.Should().Be(distribution);
        }

        [Test]
        public void CircularCloudLayouter_Initialize_Throws_ArgumentException_When_Distribution_Have_Different_Center()
        {
            var center = new Point(1, 5);
            var centerDistribution = new Point(2, 4);
            var distribution = new SpiralDistribution(centerDistribution);
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(center, distribution));
        }

        [TestCaseSource(nameof(PutNextRectangleIncorrectArguments))]
        public void PutNextRectangle_ThrowsArgumentException_WhenIncorrectArguments(Size rectangleSize,
            CircularCloudLayouter tagsCloud)
        {
            Assert.Throws<ArgumentException>(() => tagsCloud.PutNextRectangle(rectangleSize));
        }

        [Test]
        public void PutNextRectangle_Should_Place_First_On_Center()
        {
            tagsCloud.PutNextRectangle(new Size(3, 1));
            tagsCloud.WordPositions[0].Location.Should().Be(tagsCloud.Center);
        }

        [Test]
        public void CircularCloudLayouter_Should_Has_No_Intersections_When_1000_Rectangles()
        {
            getRandomTagsCloud().WordPositions.Any(tag1 =>
                    tagsCloud.WordPositions.Any(tag2 => tag1.IntersectsWith(tag2) && tag1 != tag2))
                .Should().BeFalse();
        }


        [Test]
        public void CircularCloudLayouter_Should_Be_Close_To_Circle()
        {
            var randomTagsCloud = getRandomTagsCloud();
            randomTagsCloud.WordPositions.All(tag =>
            {
                var distanceToCenter =
                    Math.Sqrt(Math.Pow(tag.X - tagsCloud.Center.X, 2) + Math.Pow(tag.Y - tagsCloud.Center.Y, 2));
                return distanceToCenter <= GetCircilarCloudLayouterRadius(randomTagsCloud);
            }).Should().BeTrue();
        }


        private static IEnumerable<TestCaseData> PutNextRectangleIncorrectArguments()
        {
            var center = new Point();
            var distribution = new SpiralDistribution(center);
            var tagsCloud = new CircularCloudLayouter(center, distribution);

            yield return new TestCaseData(new Size(-1, 1), tagsCloud)
                .SetName("PutNextReactangle_Throws_ArgumentException_When_Width_Is_Negative");

            yield return new TestCaseData(new Size(1, -1), tagsCloud)
                .SetName("PutNextReactangle_Throws_ArgumentException_When_Height_Is_Negative");

            yield return new TestCaseData(new Size(0, 1), tagsCloud)
                .SetName("PutNextReactangle_Throws_ArgumentException_When_Width_Is_Zero");

            yield return new TestCaseData(new Size(1, 0), tagsCloud)
                .SetName("PutNextReactangle_Throws_ArgumentException_When_Height_Is_Zero");
        }


        private CircularCloudLayouter getRandomTagsCloud()
        {
            var randomTagCloud = new CircularCloudLayouter(center, distribution);
            var random = new Random();
            for (var i = 0; i < 1000; i++)
            {
                var size = new Size(random.Next(1, 100), random.Next(1, 100));
                randomTagCloud.PutNextRectangle(size);
            }

            return randomTagCloud;
        }

        public static double GetCircilarCloudLayouterRadius(CircularCloudLayouter layouter)
        {
            return layouter.WordPositions.Max(tag => Math.Sqrt(Math.Pow(tag.X, 2) + Math.Pow(tag.Y, 2)));
        }
    }
}