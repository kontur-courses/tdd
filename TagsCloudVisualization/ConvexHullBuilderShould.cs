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
    public class ConvexHullBuilderShould
    {
        [TestCaseSource(nameof(RotationDirectionTestData))]
        public void GetCorrectRotationDirection(Vector vector, Point point, int expectedResult)
        {
            ConvexHullBuilder.GetRotationDirection(vector, point)
                .Should().Be(expectedResult);
        }

        [Test]
        public void BuildCorrectRectanglePointsSet()
        {
            var rectangles = new List<Rectangle>
            {
                new Rectangle(new Point(0, 0), new Size(30, 20)),
                new Rectangle(new Point(-10, -10), new Size(10, 10)),
                new Rectangle(new Point(10, -10), new Size(40, 5))
            };
            var expectedPointsSet = new List<Point>
            {
                new Point(0, 0), new Point(0, 20), new Point(30, 0),
                new Point(30, 20), new Point(-10, -10), new Point(-10, 0),
                new Point(0, -10), new Point(10, -10), new Point(50, -10),
                new Point(50, -5), new Point(10, -5)
            };

            var actualPointsSet = ConvexHullBuilder.BuildRectanglePointsSet(rectangles);

            actualPointsSet.Should().BeEquivalentTo(expectedPointsSet);

        }

        private static IEnumerable<TestCaseData> RotationDirectionTestData
        {
            get
            {
                yield return new TestCaseData(
                    new Vector(new Point(1,1), new Point(4,3)),
                    new Point(3, 5),
                    1)
                    .SetName("when point is located to the left of the vector");
                yield return new TestCaseData(
                    new Vector(new Point(0, 0), new Point(3, 6)),
                    new Point(4, -2),
                    -1)
                    .SetName("when point is located to the right of the vector");
                yield return new TestCaseData(
                    new Vector(new Point(0, 0), new Point(5, 5)),
                    new Point(3, 3),
                    0)
                    .SetName("when point is located on the vector");
            }
        }
    }
}
