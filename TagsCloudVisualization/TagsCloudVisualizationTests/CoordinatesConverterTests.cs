using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CoordinatesConverterTests
{
	[TestCaseSource(nameof(PointsSource))]
	public void ToPolar_Then_ToCartesian_ShouldReturn_SameCoordinates(int x, int y)
	{
		var point = new Point(x, y);
		var (radius, angle) = CoordinatesConverter.ToPolar(point);
		var point2 = CoordinatesConverter.ToCartesian(radius, angle);

		point2.Should().Be(point);
	}

	[TestCaseSource(nameof(PolarPointsSource))]
	public Point ToCartesian_ShouldReturnCorrectCartesianCoordinates(double radius, double angle)
	{
		return CoordinatesConverter.ToCartesian(radius, angle);
	}

	[TestCaseSource(nameof(CartesianPointsSource))]
	public (double radius, double angle) ToPolar_ShouldReturnCorrectPolarCoordinates(Point cartesian)
	{
		var (radius, angle) = CoordinatesConverter.ToPolar(cartesian);
		return (Math.Round(radius, 2), angle);
	}

	public static IEnumerable<TestCaseData> PointsSource()
	{
		yield return new TestCaseData(0, 0);
		yield return new TestCaseData(1, 1);
		yield return new TestCaseData(-1, -1);
		yield return new TestCaseData(1, -1);
		yield return new TestCaseData(-1, 1);
		yield return new TestCaseData(5, 2);
		yield return new TestCaseData(3, 5);
	}

	public static IEnumerable<TestCaseData> PolarPointsSource()
	{
		yield return new TestCaseData(5.6, 45 * Math.PI / 180).Returns(new Point(4, 4));
		yield return new TestCaseData(5.66, 3 * 45 * Math.PI / 180).Returns(new Point(-4, 4));
		yield return new TestCaseData(5.66, -3 * 45 * Math.PI / 180).Returns(new Point(-4, -4));
		yield return new TestCaseData(5.66, -45 * Math.PI / 180).Returns(new Point(4, -4));
	}

	public static IEnumerable<TestCaseData> CartesianPointsSource()
	{
		yield return new TestCaseData(new Point(4, 4)).Returns((5.66, 45 * Math.PI / 180));
		yield return new TestCaseData(new Point(-4, 4)).Returns((5.66, 3 * 45 * Math.PI / 180));
		yield return new TestCaseData(new Point(-4, -4)).Returns((5.66, -3 * 45 * Math.PI / 180));
		yield return new TestCaseData(new Point(4, -4)).Returns((5.66, -45 * Math.PI / 180));
	}
}