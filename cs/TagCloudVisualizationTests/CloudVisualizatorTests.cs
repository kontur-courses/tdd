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
        private CloudVisualizator sut;

        [SetUp]
        public void SetUp()
        {
            sut = new CloudVisualizator(1200, 1200, Color.Blue, Color.CornflowerBlue);
        }

        [Test]
        public void Ctor_ShouldAcceptWidthHeightPenColorBrushColor()
        {
            Assert.DoesNotThrow(() => new CloudVisualizator(10, 10, Color.Black, Color.Black));
        }

        [TestCase(0, 1, TestName = "WhenZeroWidth")]
        [TestCase(1, 0, TestName = "WhenZeroHeight")]
        [TestCase(0, 0, TestName = "WhenZeroWidthAndHeight")]
        [TestCase(-1, 1, TestName = "WhenNegativeWidth")]
        [TestCase(1, -1, TestName = "WhenNegativeHeight")]
        [TestCase(-1, -1, TestName = "WhenNegativeWidthAndHeight")]
        public void Ctor_ShouldThrows(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => new CloudVisualizator(width, height, Color.Blue, Color.Aquamarine));
        }

        [TestCaseSource(nameof(CasesForDrawRectangle))]
        public void DrawRectangle_Should(int pixelX, int pixelY, int expectedARGBColor)
        {
            sut.DrawRectangle(new Rectangle(50, 50, 50, 50));
            var field = (Bitmap) typeof(CloudVisualizator)
                .GetField(nameof(Bitmap).ToLower(), BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(sut);
            field.GetPixel(pixelX, pixelY).ToArgb()
                .Should().Be(expectedARGBColor);
        }

        private static IEnumerable<TestCaseData> CasesForDrawRectangle
        {
            get
            {
                yield return new TestCaseData(51, 51, Color.CornflowerBlue.ToArgb())
                    .SetName("FillArea");
                yield return new TestCaseData(50, 50, Color.Blue.ToArgb())
                    .SetName("DrawBorder");
                yield return new TestCaseData(49, 49, Color.Empty.ToArgb())
                    .SetName("NotDrawOutside");
            }
        }

        [Test]
        public void SaveImage_ShouldSaveWithFilename()
        {
            sut.DrawRectangle(new Rectangle(50, 50 ,200 , 200));
            sut.SaveImage("test.png", ImageFormat.Png);
            
            Directory.GetFiles(Directory.GetCurrentDirectory())
                .Should().Contain(Directory.GetCurrentDirectory() + "\\test.png");
        }
    }
}
