using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;

namespace Tests
{
    [TestFixture]
    public class ArchimedeanSpiralTests
    {
        private static IEnumerable<TestCaseData> ConstructorArgumentException => new[]
        {
            new TestCaseData(new Point(1, 1), 0, 1, 1).SetName("WhenGivenNotPositiveStep"),
            new TestCaseData(new Point(1, 1), 1, 0, 1).SetName("WhenGivenNotPositiveRadius"),
            new TestCaseData(new Point(1, 1), 1, 1, 0).SetName("WhenGivenNotPositiveAngle"),
    };

        [TestCaseSource(nameof(ConstructorArgumentException))]
        public void Constructor_ShouldThrowArgumentException(Point center, double step, double radius, double angle) =>
            Assert.Throws<ArgumentException>(() => new ArchimedeanSpiralPointer(center, step, radius, angle));
    }
}