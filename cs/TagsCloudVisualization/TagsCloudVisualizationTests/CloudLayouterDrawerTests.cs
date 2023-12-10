using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace TagsCloudVisualization.TagsCloudVisualizationTests
{
    [TestFixture]
    public class CloudLayouterDrawerTests
    {
        private CloudLayouterDrawer drawer;
        private string fileName;
        private string TestFilePath;


        [SetUp]
        public void SetUp()
        {
            drawer = new CloudLayouterDrawer(10);
            fileName = "testcreation.png";
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            TestFilePath = Path.Combine(projectDirectory, "images", fileName);
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }
        [Test]
        public void CloudLayouterDrawer_Initialize_Params()
        {
            Assert.AreEqual(10,drawer.margin);
        }

        [Test]
        public void CloudLayouterDrawer_IsCreate_Imgae()
        {
            var rectanglesCollection = new Rectangle[] { new Rectangle(0, 0, 5, 10) };
            drawer.DrawCloud(fileName,rectanglesCollection);
            Assert.IsTrue(File.Exists(TestFilePath), $"File at path '{TestFilePath}' should have been created.");
        }
    }
}
