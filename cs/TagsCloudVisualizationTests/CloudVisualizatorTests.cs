using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

namespace TagCloudVisualizationTests
{
    public class CloudVisualizatorTests
    {
        private CloudVisualizator visualizator;

        [SetUp]
        public void SetUp()
        {
            visualizator = new CloudVisualizator(1200, 1200, Color.Blue, Color.CornflowerBlue);
        }

        [Test]
        public void Ctor_ShouldAcceptWidthHeightPenColorBrushColor()
        {
            Assert.DoesNotThrow(() => new CloudVisualizator(10, 10, Color.Black, Color.Black));
        }

        [TestCase(0, 1, TestName = "When Zero Width")]
        [TestCase(1, 0, TestName = "When Zero Height")]
        [TestCase(0, 0, TestName = "When Zero Width And Height")]
        [TestCase(-1, 1, TestName = "When Negative Width")]
        [TestCase(1, -1, TestName = "When Negative Height")]
        [TestCase(-1, -1, TestName = "When Negative Width And Height")]
        public void Ctor_ShouldThrows(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => new CloudVisualizator(width, height, Color.Blue, Color.Aquamarine));
        }

        [TestCaseSource(nameof(CasesForDrawRectangle))]
        public void DrawRectangle_Should(int pixelX, int pixelY, int expectedARGBColor)
        {
            var bitmap = (Bitmap)typeof(CloudVisualizator)
                .GetField(nameof(Bitmap).ToLower(), BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(visualizator);

            visualizator.DrawRectangle(new Rectangle(50, 50, 50, 50));

            bitmap.GetPixel(pixelX, pixelY).ToArgb()
                .Should().Be(expectedARGBColor);
        }

        private static IEnumerable<TestCaseData> CasesForDrawRectangle
        {
            get
            {
                yield return new TestCaseData(51, 51, Color.CornflowerBlue.ToArgb())
                    .SetName("Fill Area");
                yield return new TestCaseData(50, 50, Color.Blue.ToArgb())
                    .SetName("Draw Border");
                yield return new TestCaseData(49, 49, Color.Empty.ToArgb())
                    .SetName("Not Draw Outside");
            }
        }

        [Test]
        public void SaveImage_ShouldSaveWithFilename()
        {
            var rectangle = new Rectangle(50, 50, 200, 200);

            visualizator.DrawRectangle(rectangle);
            visualizator.SaveImage("test.png", ImageFormat.Png);

            Directory.GetFiles(Environment.CurrentDirectory)
                .Should().Contain(Environment.CurrentDirectory + "\\test.png");
        }
    }
}
