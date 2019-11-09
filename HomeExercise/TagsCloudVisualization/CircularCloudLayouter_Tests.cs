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
			for (var i = 0; i < 20; i++)
				_circularCloudLayouter.PutNextRectangle(rectangleSize);
			var expectedResult = new bool[190];

			var filledAreas = _circularCloudLayouter.Rectangles;
			var actualResult = filledAreas
				.SelectMany((area, i) => filledAreas
					.Skip(i + 1)
					.Select(area.IntersectsWith));
			actualResult.Should().BeEquivalentTo(expectedResult);
		}

		[Test]
		public void CloudShouldBeDense()
		{
			const double minimumDensityCoefficient = 0.5;
			var rectangleSize = new Size(20, 20);
			for (var i = 0; i < 100; i++)
				_circularCloudLayouter.PutNextRectangle(rectangleSize);
			var maximumArea = CalculateMaximumArea();
			
			var actualFilledArea = _circularCloudLayouter.Rectangles.Sum(rect => rect.Width * rect.Height);
			var actualDensityCoefficient = (double) actualFilledArea / maximumArea;
			actualDensityCoefficient.Should().BeGreaterOrEqualTo(minimumDensityCoefficient);
		}

		private long CalculateMaximumArea()
		{
			var rectangles = _circularCloudLayouter.Rectangles;
			int maxX = int.MinValue, maxY = int.MinValue;
			int minX = int.MaxValue, minY = int.MaxValue;

			foreach (var rectangle in rectangles)
			{
				maxX = rectangle.Right > maxX ? rectangle.Right : maxX;
				maxY = rectangle.Top > maxY ? rectangle.Top : maxY;
				minY = rectangle.Bottom < minY ? rectangle.Bottom : minY;
				minX = rectangle.Left < minX ? rectangle.Left : minX;
			}

			var width = maxX - minX;
			var height = maxY - minY;
			return width * height;
		}
	}
}