using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class TagsVisualizatorTests
    {
        private TagsVisualizator Sut { get; set; }

        [SetUp]
        public void SetUp()
        {
            Sut = new TagsVisualizator(new List<Rectangle>());
        }

        [Test]
        public void SaveVisualizationToDirectory_GetImageSizeThrowException_WhenRectangleOutOfBoundaries()
        {
            var path = new PathGenerator().GetNewFilePath();
            var rectangles = new List<Rectangle>();
            var location = new Point(-100, -100);
            var size = new Size(50, 50);
            rectangles.Add(new Rectangle(location, size));
            Sut = new TagsVisualizator(rectangles);

            Action saveImage = () => Sut.GetBitmap();

            saveImage.Should().Throw<ArgumentException>();
        }
    }
}