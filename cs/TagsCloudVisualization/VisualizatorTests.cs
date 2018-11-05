using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class VisualizatorTests
    {
        #region InvalidSizes

        public static IEnumerable<Size> OnInvalidSizes()
        {
            yield return new Size(0, 0);
            yield return new Size(0, 1);
            yield return new Size(1, 0);
            yield return new Size(-1, 1);
            yield return new Size(1, -1);
            yield return new Size(-1, -1);
        }

        #endregion

        [TestCaseSource(nameof(OnInvalidSizes))]
        [Test]
        public void Constructor_ThrowsException_OnInvalidImageSize(Size size)
        {
            Action action = () => new Visualizator(size, new List<Rectangle>());
            action
                .Should()
                .Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_ThrowsException_OnNullRectangles()
        {
            Action action = () => new Visualizator(
                new Size(100, 100),
                null);
            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        public void Generate_ReturnsBitmap_WithGivenSize()
        {
            var size = new Size(1024, 1024);
            var visualizator = new Visualizator(size, new List<Rectangle>());

            var result = visualizator.Generate();

            result.Size
                .Equals(size)
                .Should()
                .BeTrue();
        }
    }
}