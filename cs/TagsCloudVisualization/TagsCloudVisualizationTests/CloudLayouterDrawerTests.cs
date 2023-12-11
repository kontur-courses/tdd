using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.TagsCloudVisualizationTests
{
    [TestFixture]
    public class CloudLayouterDrawerTests
    {
        [SetUp]
        public void SetUp()
        {
            drawer = new CloudLayouterDrawer(10);
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            testFilePath = Path.Combine(projectDirectory, "images", fileName);
            if (File.Exists(testFilePath)) File.Delete(testFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(testFilePath)) File.Delete(testFilePath);
        }

        private CloudLayouterDrawer drawer;
        private const string fileName = "testcreation.png";
        private string testFilePath;

        [Test]
        public void CloudLayouterDrawer_Initialize_Params()
        {
            drawer.Margin.Should().Be(10);
        }

        [Test]
        public void CloudLayouterDrawer_Initialize_Throws_ArgumentException_When_Rectangles_length_Is_Zero()
        {
            var rectangles = new List<Rectangle>();
            Assert.Throws<ArgumentException>(()=>drawer.DrawCloud("output.png",rectangles));
        }

        [TestCase("",TestName = "When_Filename_Is_Empty")]
        [TestCase(null, TestName = "When_Filename_Is_Null")]

        public void CloudLayouterDrawer_Initialize_Throws_ArgumentException(string filename)
        {
            var rectangles = new List<Rectangle>(){new Rectangle(1,1,1,1)};
            Assert.Throws<ArgumentException>(() => drawer.DrawCloud(filename, rectangles));
        }

        [Test]
        public void CloudLayouterDrawer_IsCreate_Image()
        {
            var rectanglesCollection = new[] { new Rectangle(0, 0, 5, 10) };
            drawer.DrawCloud(fileName, rectanglesCollection);
            File.Exists(testFilePath).Should().BeTrue();
        }
    }
}