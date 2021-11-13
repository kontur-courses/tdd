using NUnit.Framework;
using System.Drawing;

namespace TagsCloudVisualizer
{
    class PointExtensions_Tests
    {
        [TestCase(new int[] { 0, 0 }, 1, 1, ExpectedResult = new int[] { 1, 1 })]
        [TestCase(new int[] { 0, 0 }, 2, 2, ExpectedResult = new int[] { 2, 2 })]
        [TestCase(new int[] { 0, 0 }, -1, -1, ExpectedResult = new int[] { -1, -1 })]
        [TestCase(new int[] { 1, 1 }, 1, 1, ExpectedResult = new int[] { 2, 2 })]
        [TestCase(new int[] { -1, -1 }, 1, 1, ExpectedResult = new int[] { 0, 0 })]
        [TestCase(new int[] { 2, 3 }, 1, 1, ExpectedResult = new int[] { 3, 4 })]
        [TestCase(new int[] { 2, 3 }, 0, 0, ExpectedResult = new int[] { 2, 3 })]
        public int[] Displace_OnPoint_ShouldWorkCorrect(int[] a, int dX, int dY)
        {
            var p = new Point(a[0], a[1]);
            var point = p.Displace(dX, dY);
            return new int[] { point.X, point.Y };
        }
    }
}
