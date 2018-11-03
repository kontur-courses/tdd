using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class Visualizator_Should
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
    }
}