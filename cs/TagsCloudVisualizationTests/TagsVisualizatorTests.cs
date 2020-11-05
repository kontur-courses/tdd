using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            Sut = new TagsVisualizator();
        }

        [Test]
        public void SaveVisualization_UpdateImageBordersThrowException_WhenRectangleOutOfBoundaries()
        {
            var path = Sut.GenerateNewFilePath();
            var rectangles = new List<Rectangle>();
            var location = new Point(-100, -100);
            var size = new Size(50, 50);
            rectangles.Add(new Rectangle(location, size));

            Action saveImage = () => Sut.SaveVisualization(rectangles, path);

            saveImage.Should().Throw<ArgumentException>();
        }

        [TestCase(@"C:\image.gif", TestName = "Wrong image format")]
        [TestCase(@"S:Nonexistent\Dir\image.jpg", TestName = "Directory dont exist")]
        public void TagsVisualizator_ThrowException_When(string path)
        {
            Action saveImage = () => Sut.SaveVisualization(new List<Rectangle>(), path);

            saveImage.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GenerateNewFilePath_ReturnExistingDirectory_WhenCalled()
        {
            var path = Sut.GenerateNewFilePath().Split('\\');
            path[^1] = "";
            var directoryPath = String.Join('\\', path);

            Directory.Exists(directoryPath).Should().BeTrue();
        }
    }
}