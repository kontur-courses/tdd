using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class RectangleExtensions_Tests
	{
		[TestCase(0, 0, 0, 0, TestName = "Zero rectangle")]
		[TestCase(2, 2, 1, -1, TestName = "Square")]
		[TestCase(5, 2, 2, -1, TestName = "Rectangle with odd width")]
		[TestCase(2, 5, 1, -2, TestName = "Rectangle with odd height")]
		[TestCase(7, 5, 3, -2, TestName = "Rectangle with odd width and height")]
		public void GetCenter_ReturnsCorrectCenter(int rectangleWidth,
													int rectangleHeight,
													int expectedCenterX,
													int expectedCenterY)
		{
			var rectangleSize = new Size(rectangleWidth, rectangleHeight);
			var rectangle = new Rectangle(Point.Empty, rectangleSize);
			var expectedCenter = new Point(expectedCenterX, expectedCenterY);

			var actualCenter = rectangle.GetCenter();
			actualCenter.Should().Be(expectedCenter);
		}
	}
}