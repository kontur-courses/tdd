using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization.Base;

namespace TagsCloudVisualizationTests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class PolarMathTests
{
    [TestCase(0, ExpectedResult = 0, TestName = "DegreesEqualZero")]
    [TestCase(30, ExpectedResult = Math.PI / 6, TestName = "DegreesEqual30")]
    [TestCase(90, ExpectedResult = Math.PI / 2, TestName = "DegreesEqual90")]
    [TestCase(120, ExpectedResult = 2 * Math.PI / 3, TestName = "DegreesEqual120")]
    [TestCase(180, ExpectedResult = Math.PI, TestName = "DegreesEqual180")]
    [TestCase(270, ExpectedResult =  3 * Math.PI / 2, TestName = "DegreesEqual270")]
    [TestCase(315, ExpectedResult =  7 * Math.PI / 4, TestName = "DegreesEqual315")]
    [TestCase(360, ExpectedResult = 2 * Math.PI, TestName = "DegreesEqual360")]
    public double ConvertToRadians_ShouldReturnExpectedResult(double degrees) => 
        PolarMath.ConvertToRadians(degrees);
    
    [TestCase(0, ExpectedResult = 0, TestName = "RadiansEqualZero")]
    [TestCase(Math.PI / 6, ExpectedResult = 30, TestName = "RadiansEqualOneSixthPi")]
    [TestCase(Math.PI / 2, ExpectedResult = 90, TestName = "RadiansEqualHalfPi")]
    [TestCase(2 * Math.PI / 3, ExpectedResult = 120, TestName = "RadiansEqualTwoThirdPi")]
    [TestCase(Math.PI, ExpectedResult = 180, TestName = "RadiansEqualPi")]
    [TestCase(3 * Math.PI / 2, ExpectedResult = 270, TestName = "RadiansEqualThreeSecondPi")]
    [TestCase(7 * Math.PI / 4, ExpectedResult = 315, TestName = "RadiansEqualSevenFourthPi")]
    [TestCase(2 * Math.PI, ExpectedResult = 360, TestName = "RadiansEqualTwoPi")]
    public double ConvertToDegrees_ShouldReturnExpectedResult(double radians) => 
        Math.Round(PolarMath.ConvertToDegrees(radians));

    [TestCase(0, ExpectedResult = 0, TestName = "RadiusEqualZero")]
    [TestCase(5, ExpectedResult = 5 * 5 * Math.PI, TestName = "RadiusEqualGreaterThanZero")]
    public double GetSquareOfCircle_ShouldReturnExpectedResult(double radius) => 
        PolarMath.GetSquareOfCircle(radius);
    
    [TestCase(0, ExpectedResult = 1, TestName = "DegreesEqualZero")]
    [TestCase(145, ExpectedResult = 2, TestName = "DegreesInSecondSector")]
    [TestCase(375, ExpectedResult = 1, TestName = "DegreesInNextCircle")]
    public double GetSectorOfCircleFromDegrees_ShouldReturnExpectedResult(int degrees) =>
        PolarMath.GetSectorOfCircleFromDegrees(degrees);
    
    [TestCase(0, ExpectedResult = 1, TestName = "RadiansEqualZero")]
    [TestCase(3 * Math.PI / 4, ExpectedResult = 2, TestName = "RadiansInSecondSector")]
    [TestCase(9 * Math.PI / 4, ExpectedResult = 1, TestName = "RadiansInNextCircle")]
    public double GetSectorOfCircleFromRadians_ShouldReturnExpectedResult(double radians) =>
        PolarMath.GetSectorOfCircleFromRadians(radians);
    
    [TestCase( 0.5 * Math.PI, ExpectedResult = 0.25, TestName = "RadiusEqualHalfOfPi")]
    [TestCase(Math.PI, ExpectedResult = 0.5, TestName = "RadiusEqualPi")]
    [TestCase(1.5 * Math.PI, ExpectedResult = 0.75, TestName = "RadiusEqualOneAndHalfPi")]
    [TestCase(2 * Math.PI, ExpectedResult = 1, TestName = "RadiusEqualTwoPi")]
    public double GetOffsetPerRadianForArchimedeanSpiral_ShouldReturnExpectedResult(double radius) => 
        PolarMath.GetOffsetPerRadianForArchimedeanSpiral(radius);
    
    [TestCaseSource(nameof(_convertToCartesianCoordinateSystemTestCases))]
    public Point ConvertToCartesianCoordinateSystem_ShouldReturnExpectedResult(double polarAngle) => 
        PolarMath.ConvertToCartesianCoordinateSystem(10, polarAngle);

    private static object[] _convertToCartesianCoordinateSystemTestCases =
    {
        new TestCaseData(Math.PI / 2).Returns(new Point(0, 10)).SetName("PolarAngleEqualHalfOfPi"),
        new TestCaseData(Math.PI).Returns(new Point(-10, 0)).SetName("PolarAngleEqualPi"),
        new TestCaseData(0.25 * Math.PI).Returns(new Point(7, 7)).SetName("PolarAngleEqualQuarterOfPi"),
        new TestCaseData(1.5 * Math.PI).Returns(new Point(0, -10)).SetName("PolarAngleEqualHalfOfPi"),
    };
    
    [TestCaseSource(nameof(_convertToPolarCoordinateSystemTestCases))]
    public (double polarRadius, double polarAngle) ConvertToPolarCoordinateSystem_ShouldReturnExpectedResult(Point point) => 
        PolarMath.ConvertToPolarCoordinateSystem(point);
    
    private static object[] _convertToPolarCoordinateSystemTestCases =
    {
        new TestCaseData(new Point(0, 0)).Returns((0, 0)),
        new TestCaseData(new Point(1, 1)).Returns((Math.Sqrt(2), Math.PI / 4)),
        new TestCaseData(new Point(-1, -1)).Returns((Math.Sqrt(2), 5 * Math.PI / 4)),
        new TestCaseData(new Point(0, 2)).Returns((2, Math.PI / 2)),
        new TestCaseData(new Point(-2, 0)).Returns((2, Math.PI))
    };
}