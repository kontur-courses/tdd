using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;

namespace Tests
{
    [TestFixture]
    public class ArchimedeanSpiralTests
    {
        private ArchimedeanSpiralPlacer archimedeanSpiralPlacer;
        [SetUp]
        public void SetUp()
        {
            archimedeanSpiralPlacer = new ArchimedeanSpiralPlacer(new Point(1, 1), 1, 1, 1);
        }

        static IEnumerable<TestCaseData> ConstructorArgumentException => new[]
        {
            new TestCaseData(new Point(1, 1), 0, 1, 1).SetName("WhenGivenNotPositiveStep"),
            new TestCaseData(new Point(1, 1), 1, 0, 1).SetName("WhenGivenNotPositiveRadius"),
            new TestCaseData(new Point(1, 1), 1, 1, 0).SetName("WhenGivenNotPositiveAngle"),
    };

        [TestCaseSource(nameof(ConstructorArgumentException))]
        public void Constructor_ShouldThrowArgumentException(Point center, double step, double radius, double angle) =>
            Assert.Throws<ArgumentException>(() => new ArchimedeanSpiralPlacer(center, step, radius, angle));

        static IEnumerable<TestCaseData> GetNextRectangleArgumentException => new[]
        {
            new TestCaseData(new Size(0,1)).SetName("WhenGivenNotPositiveWidth"),
            new TestCaseData(new Size(1,0)).SetName("WhenGivenNotPositiveHeigth"),
            new TestCaseData(new Size(0,0)).SetName("WhenGivenNotPositiveHeigthAndWidth"),
    };

        [TestCaseSource(nameof(GetNextRectangleArgumentException))]
        public void GetNextRectangle_ShouldThrowArgumentException(Size rectangleSize) =>
                Assert.Throws<ArgumentException>(() => archimedeanSpiralPlacer.GetNextRectangle(rectangleSize));
    }

}