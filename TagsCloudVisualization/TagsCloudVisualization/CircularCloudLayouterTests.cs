using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        [TestCase(0, 0, 10, 6, Description = "Zero center")]
        [TestCase(-5, 4, 10, 6, Description = "Non zero center")]
        [TestCase(2, 2, 7, 3, Description = "Odd width and height")]
        public void PutNextRectangle_FirstRectangle_PlacesIntoCenter(int x, int y, int width, int height)
        {
            var layouter = new CircularCloudLayouter(new Point(x, y));
            Assert.AreEqual(
                new Rectangle(x - width / 2, y - height / 2, width, height),
                layouter.PutNextRectangle(new Size(width, height)));
        }

        [TestCase(new [] {5, 5, 5, 5} , new[] { -5, -5, 5, 5 }, ExpectedResult = false)]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { 2, 2, 5, 5 }, ExpectedResult = true, Description = "Internal rectangle")]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { -2, -2, 5, 5 }, ExpectedResult = true)]
        [TestCase(new[] { -2, -2, 5, 5 }, new[] { 0, 0, 10, 10 }, ExpectedResult = true)]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { 0, 20, 10, 10 }, ExpectedResult = false, Description = "Y-Axis is parallel")]
        [TestCase(new[] { 0, 0, 10, 10 }, new[] { -10, 20, 10, 10 }, ExpectedResult = false)]
        public bool CheckCollision(int[] rect, int[] other)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            return layouter.IsCollision(
                new Rectangle(rect[0], rect[1], rect[2], rect[3]),
                new Rectangle(other[0], other[1], other[2], other[3]));
        }
    }
}
