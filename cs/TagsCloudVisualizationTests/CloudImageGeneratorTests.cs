using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    internal class CloudImageGeneratorTests
    {
        private Point testPoint = new Point(1000, 1000);

        [Test]
        public void Should_Throw_WhenTryingToCreateWithNoRectangles()
        {
            var layouter = new CircularCloudLayouter(testPoint);
            FluentActions.Invoking(
                () => CloudImageGenerator.CreateImage(layouter, new Size(testPoint)))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void Should_SaveToFile()
        {
            var layouter = new CircularCloudLayouter(testPoint);
            layouter.PutNextRectangle(new Size(testPoint));
            var path = CloudImageGenerator.CreateImage(layouter, new Size(testPoint));
            File.Exists(path).Should().BeTrue();
            File.Delete(path);
        }
    }
}
