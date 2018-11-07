using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TagsCloudVisualization;

namespace TagClouVisualizationTest
{
    [TestFixture]
    class CircularCloudLayouterTest
    {
        static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(new List<Size>()
            {
                new Size(4, 1), new Size(1, 1),
            }).SetName("TwoRectangles");
            yield return new TestCaseData(new List<Size>()
            {
                new Size(2, 5), new Size(1, 3), new Size(1, 7), new Size(3, 9),
                new Size(2, 6), new Size(1, 4), new Size(1, 2), new Size(1, 8),

            }).SetName("VerticalRectangles");
            yield return new TestCaseData(new List<Size>()
            {
                new Size(2, 5), new Size(1, 3), new Size(1, 7), new Size(3, 9),
                new Size(2, 6), new Size(1, 4), new Size(1, 2), new Size(1, 8),

            }).SetName("BigHorizontalRectangles");
            yield return new TestCaseData(new List<Size>()
            {
                new Size(1, 1), new Size(3, 3), new Size(5, 5), new Size(9, 9),

            }).SetName("Square");
        }

        [Test, TestCaseSource("GetTestCases")]
        public void PutNextRectangle_ShouldNotCrossRectangles(List<Size> sizes)
        {
            var center = new Point(0, 0);
            var testCircularCloudLayouter = new CircularCloudLayouter(center);

            var rectangles = new List<Rectangle>();
            foreach (var size in sizes)
                rectangles.Add(testCircularCloudLayouter.PutNextRectangle(size));
            AreIntersects(rectangles).Should().BeFalse();

        }     

        private bool AreIntersects(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return true;
            return false;
        }
    }
}
