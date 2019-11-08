using System;
using System.Linq;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class CircularCloudLayouter_Tests
	{
		private CircularCloudLayouter _circularCloudLayouter;

		[SetUp]
		public void StartUp() => _circularCloudLayouter = new CircularCloudLayouter();

		[Test]
		public void PutNextRectangle_SavesPutRectangles()
		{
			const int expectedRectanglesCount = 5;

			for (var i = 0; i < expectedRectanglesCount; i++)
				_circularCloudLayouter.PutNextRectangle(new Size());
			var actualRectangles = _circularCloudLayouter.Rectangles;
			actualRectangles.Count.Should().Be(expectedRectanglesCount);
		}

		[TestCase(-1, 0, TestName = "Negative rectangle width")]
		[TestCase(0, -1, TestName = "Negative rectangle height")]
		[TestCase(-1, -1, TestName = "Negative rectangle width and height")]
		public void PutNextRectangle_ThrowsExceptionOnNegativeSizeValues(int width, int height)
		{
			var firstRectangleSize = new Size(width, height);
			Action action = () => _circularCloudLayouter.PutNextRectangle(firstRectangleSize);
			action.Should().Throw<ArgumentException>();
		}

		[TestCase(2, 2, TestName = "Squares")]
		[TestCase(5, 3, TestName = "Horizontal rectangles")]
		[TestCase(3, 5, TestName = "Vertical rectangles")]
		[TestCase(0, 0, TestName = "Zero size")]
		public void RectanglesShouldNotIntersect(int width, int height)
		{
			var rectangleSize = new Size(width, height);
			for (var i = 0; i < 500; i++)
				_circularCloudLayouter.PutNextRectangle(rectangleSize);
			var expectedResult = new bool[124750];

			var filledAreas = _circularCloudLayouter.Rectangles;
			var actualResult = filledAreas
				.SelectMany((area, i) => filledAreas
					.Skip(i + 1)
					.Select(area.IntersectsWith));
			actualResult.Should().BeEquivalentTo(expectedResult);
		}
	}
}