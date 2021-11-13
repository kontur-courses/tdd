using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class ImageSaver_Tests
    {
        private ImageSaver saver;
        private Bitmap image;

        [SetUp]
        public void SetUp()
        {
            saver = new ImageSaver();
            image = new Bitmap(100, 100);
        }


        [TestCase(@"/\<>*:?""|", TestName = "WhenFileNameContainsInvalidCharacters")]
        [TestCase(null, TestName = "WhenFileNameIsNull")]
        [TestCase("    ", TestName = "WhenFileNameConsistsOfWhiteSpacesOnly")]
        public void SaveImage_ThrowsArgumentException(string fileName)
        {
            Action action = () => saver.SaveImage(image, fileName, ImageFormat.Bmp);

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void SaveImage_FileExists_WhenValidData()
        {
            var fileName = "Image";

            saver.SaveImage(image, fileName, ImageFormat.Bmp);

            File.Exists(fileName).Should().BeTrue();
        }
    }
}