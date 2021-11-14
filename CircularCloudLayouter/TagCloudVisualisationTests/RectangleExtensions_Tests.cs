using NUnit.Framework;
using System;
using System.Drawing;

namespace TagCloudVisualisation
{
    [TestFixture]
    public class RectangleExtensions_Tests
    {
        [TestCase(3, 4, ExpectedResult = 5)]
        [TestCase(6, 8, ExpectedResult = 10)]
        [TestCase(4, 3, ExpectedResult = 5)]
        [TestCase(3, 4, ExpectedResult = 5)]
        [TestCase(5, 12, ExpectedResult = 13)]
        [TestCase(8, 15, ExpectedResult = 17)]
        [TestCase(7, 24, ExpectedResult = 25)]
        public int GetDiagonalLength_OnRectangles_ShouldReturnCorrectResult(int x, int y)
        {
            var rect = new Rectangle(new Point(0, 0), new Size(x, y));
            return (int)Math.Round(rect.GetDiagonalLength());
        }
    }
}
