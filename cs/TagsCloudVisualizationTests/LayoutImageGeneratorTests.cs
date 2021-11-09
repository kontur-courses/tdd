using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    internal class LayoutImageGeneratorTests
    {
        [Test]
        public void Should_Throw_WhenTryingToCreateWithNoRectangles()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            FluentActions.Invoking(
                () => LayoutImageGenerator.CreateImage(layouter))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void Should_SaveToFile()
        {
            var path = LayoutImageGenerator.GenerateImage(new Point(500, 500), 100);
            File.Exists(path).Should().BeTrue();
        }
    }
}
