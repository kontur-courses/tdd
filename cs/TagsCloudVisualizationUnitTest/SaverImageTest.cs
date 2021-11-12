using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationUnitTest
{
    class SaverImageTest
    {
        [TestCase("", TestName = "Filename is empty")]
        [TestCase(" ", TestName = "Filename is white Space")]
        public void Save_ShouldBeThrowWhen(string filename)
        {
            var saver = new SaverImage(filename);

            using Image image = new Bitmap(100, 100);

            Action act = () => saver.Execute(image);

            act.Should().Throw<Exception>();
        }

        [Test]
        public void Save_ShouldBeThrowWhen_ImageIsNull()
        {
            var saver = new SaverImage("test");

            Action act = () => saver.Execute(null);

            act.Should().Throw<Exception>();
        }

        [Test]
        public void SavedImage_ShouldBe_Exist()
        {
            const string filename = "test";

            var saver = new SaverImage(filename);

            using Image image = new Bitmap(100, 100);

            saver.Execute(image);

            File.Exists(filename).Should().BeTrue();

            File.Delete(filename);
        }
    }
}