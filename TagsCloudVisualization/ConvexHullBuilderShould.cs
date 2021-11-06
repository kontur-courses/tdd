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
        public void ShowCorrectRotationDirection(Vector vector, Point point, int expectedResult)
        {
            ConvexHullBuilder.GetRotationDirection(vector, point)
                .Should().Be(expectedResult);
        }

        private static IEnumerable<TestCaseData> RotationDirectionTestData
        {
            get
            {
                yield return new TestCaseData(
                    new Vector(new Point(1,1), new Point(4,3)),
                    new Point(3, 5),
                    1)
                    .SetName("the point is located to the left of the vector");
                yield return new TestCaseData(
                    new Vector(new Point(0, 0), new Point(3, 6)),
                    new Point(4, -2),
                    -1)
                    .SetName("the point is located to the right of the vector");
                yield return new TestCaseData(
                    new Vector(new Point(0, 0), new Point(5, 5)),
                    new Point(3, 3),
                    0)
                    .SetName("the point is located on the vector");
            }
        }
    }
}
