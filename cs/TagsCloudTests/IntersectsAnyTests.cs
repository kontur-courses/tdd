using System.Drawing;
using FluentAssertions;
using TagsCloudCore.CloudLayout.Extensions;

namespace TagsCloudTests;

public class IntersectsAnyTests
{
    [Test]
    public void IntersectsAny_ShouldThrowException_IfExistingRectanglesAreNull_Test()
    {
        var rectangle = GetRectangle(50, 50, 50, 50);

        Action act = () => rectangle.IntersectsAny(null);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Existing rectangles can't be null (Parameter 'rectangles')");
    }

    [TestCaseSource(nameof(IntersectionCases))]
    public void IntersectsAny_ShouldWorkForVariousCases_Test(
        Rectangle existing,
        Rectangle candidate,
        bool isIntersectionExpected)
    {
        var hasIntersection = candidate.IntersectsAny([existing]);

        hasIntersection.Should().Be(isIntersectionExpected);
    }
    
    private static IEnumerable<TestCaseData> IntersectionCases()
    {
        var baseRectangle = GetRectangle(50, 50, 50, 50);

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(70, 70, 30, 30),
            true
        ).SetName("Overlap_CenterInside_ShouldCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(0, 0, 30, 30),
            false
        ).SetName("RectanglesThatFarAwayWithNoOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(100, 50, 20, 20),
            false
        ).SetName("RightSideOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(30, 50, 20, 50),
            false
        ).SetName("LeftSideOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(50, 100, 40, 20),
            false
        ).SetName("BottomSideOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(50, 30, 20, 20),
            false
        ).SetName("TopSideOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(100, 100, 1, 1),
            false
        ).SetName("TouchingCorner_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(75, 75, 2, 2),
            true
        ).SetName("Inside_EntirelyContained_ShouldCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            GetRectangle(40, 40, 150, 150),
            true
        ).SetName("Contains_BaseRectangleInsideCandidate_ShouldCountAsOverlap_Test");
    }

    private static Rectangle GetRectangle(int upperLeftCornerX, int upperLeftCornerY, int width, int height)
    {
        return new Rectangle(upperLeftCornerX, upperLeftCornerY, width, height);
    }
}