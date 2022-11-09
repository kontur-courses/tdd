using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
	[TestFixture]
	public class CoordinatesConverterTests
	{
		[TestCase(100, 100)]
		[TestCase(0, 0)]
		[TestCase(1, 1)]
		[TestCase(256, 512)]
		[TestCase(654, 321)]
		[TestCase(-123, -456)]
		[TestCase(-12, 34)]
		[TestCase(12, -34)]
		public void ToPolar_Then_ToCartesian_ShouldReturn_SameCoordinates(int x, int y)
		{
			var point = new Point(x, y);
			var (radius, angle) = CoordinatesConverter.ToPolar(point);
			var point2 = CoordinatesConverter.ToCartesian(radius, angle);

			point2.Should().BeEquivalentTo(point);
		}
	}
}