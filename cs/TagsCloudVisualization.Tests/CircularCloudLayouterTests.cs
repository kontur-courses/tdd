using System.Drawing;
using AwesomeAssertions;
using FakeItEasy;

namespace TagsCloudVisualization.Tests;

using TagsCloudVisualization;

public class CircularCloudLayouterTests
{
    private readonly Point defaultCenter = new Point(10, 10);
    private readonly Size defaultSize = new Size(4, 2);

    [TestCase(6, 10, 3, 3)] // четные стороны
    [TestCase(5, 7, 0, 0)] // нечетные стороны
    public void CircularCloudLayouter_PutsFirstRectangleInCenter(
        int width, int height, int centerX, int centerY)
    {
        var provider = A.Fake<ICenterPointProvider>();
        A.CallTo(() => provider.GetNextCenterPosition()).Returns(new Point(centerX, centerY));
        var layouter = new CircularCloudLayouter(provider);
        var expectedRect = new Rectangle(centerX - width / 2, centerY - height / 2, width, height);

        var actualRect = layouter.PutNextRectangle(new Size(width, height));

        actualRect.Should().Be(expectedRect);
    }

    [TestCase(-10, 10)]
    [TestCase(10, -10)]
    [TestCase(10, 0)]
    [TestCase(0, 10)]
    [TestCase(0, 0)]
    public void CircularCloudLayouter_ThrowsArgumentException_IfSideIsNotPositive(
        int width, int height)
    {
        var provider = A.Fake<ICenterPointProvider>();
        var layouter = new CircularCloudLayouter(provider);

        var action = () => layouter.PutNextRectangle(new Size(width, height));

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void CircularCloudLayouter_CallsProviderForFirstRectangle()
    {
        var provider = A.Fake<ICenterPointProvider>();
        var layouter = new CircularCloudLayouter(provider);

        layouter.PutNextRectangle(defaultSize);

        A.CallTo(() => provider.GetNextCenterPosition()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void CircularCloudLayouter_PutsSecondRectangleAtProviderPoint_WhenNoIntersection()
    {
        var provider = A.Fake<ICenterPointProvider>();
        var firstCenter = new Point(5, 5);
        var size = new Size(4, 4);
        var farawayPoint = new Point(100, 100); // берем далекую точку, чтобы не было пересечения 
        A.CallTo(() => provider.GetNextCenterPosition())
            .ReturnsNextFromSequence(
                firstCenter,
                farawayPoint
            );
        var layouter = new CircularCloudLayouter(provider);

        _ = layouter.PutNextRectangle(size);
        var secondRectangle = layouter.PutNextRectangle(size);

        secondRectangle.GetCenter().Should().Be(farawayPoint);
        A.CallTo(() => provider.GetNextCenterPosition()).MustHaveHappenedTwiceExactly();
    }

    [Test]
    public void CircularCloudLayouter_PutsSecondRectangleAtDifferentFromProviderPoint_WhenIntersection()
    {
        var provider = A.Fake<ICenterPointProvider>();
        var conflictingPoint = new Point(1, 1); // точка пересечения (одна из)
        var unconflictingPoint = new Point(1000, 1000); // точка не в пересечении
        var bigSize = new Size(100, 100);

        // два раза возвращается точка в пересечении, а во всех следующих вызовах - не из пересечения
        // сделано, чтобы предусмотреть варианты реализаций CircularCloudLayouter, в которых при пересечении берется
        // следующая точка из provider и в которых не берется. без этого в первом варианте будет вечный цикл
        
        A.CallTo(() => provider.GetNextCenterPosition())
            .Returns(unconflictingPoint);
        A.CallTo(() => provider.GetNextCenterPosition())
            .Returns(conflictingPoint).Twice();
        
        var layouter = new CircularCloudLayouter(provider);

        var firstRect = layouter.PutNextRectangle(bigSize);
        var secondRect = layouter.PutNextRectangle(bigSize);

        secondRect.GetCenter().Should().NotBe(conflictingPoint);
        firstRect.IntersectsWith(secondRect).Should().BeFalse();
    }
}