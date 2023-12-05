using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.UnitTests
{
    [TestFixture]
    public class ArchimedeanSpiral_Should
    {
        [TestCaseSource(typeof(TestDataArchimedeanSpiral), nameof(TestDataArchimedeanSpiral.Different_CenterPoints))]
        public void ReturnCenterPoint_WhenFirstTime_GetNextPoint(Point point)
        {
            var spiral = new ArchimedeanSpiral(point);
            spiral.GetNextPoint().Should().BeEquivalentTo(point);
        }

        [TestCaseSource(typeof(TestDataArchimedeanSpiral),
            nameof(TestDataArchimedeanSpiral.DifferentIterationsAdded_ExpectedPoints))]
        public void ReturnsCorrectPoint_When(int iterations, Point expectedPoint)
        {
            var spiral = new ArchimedeanSpiral(new Point());
            for (var i = 0; i < iterations; i++)
                spiral.GetNextPoint();

            spiral.GetNextPoint().Should().BeEquivalentTo(expectedPoint);
        }
    }
}
