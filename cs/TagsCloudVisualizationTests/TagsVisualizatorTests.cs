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

            Action saveImage = () => Sut.SaveVisualizationToDirectory(path);

            saveImage.Should().Throw<ArgumentException>();
        }

        [TestCase(@"<html></html>", TestName = "Directory dont exist")]
        [TestCase(@"C:/Dir/image.jpg", TestName = "Not backslash separator")]
        [TestCase(@"C:\image jpg", TestName = "Doesnt have dot separator")]
        [TestCase(@"C:\image.", TestName = "Doesnt have filename extension")]
        public void SaveVisualizationToDirectory_ThrowException_When(string path)
        {
            Action saveImage = () => Sut.SaveVisualizationToDirectory(path);

            saveImage.Should().Throw<ArgumentException>();
        }

        [TestCase(@"C:\image.jpg", TestName = "Relative path")]
        [TestCase(@"..\image.jpg", TestName = "Absolute path")]
        public void SaveVisualizationToDirectory_DoesntThrowException_When(string path)
        {
            Action saveImage = () => Sut.SaveVisualizationToDirectory(path);

            saveImage.Should().NotThrow<ArgumentException>();
        }
    }
}