using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
	[TestFixture]
	public class CircularCloudLayouterTests
	{
		private CircularCloudLayouter layouter;

		[SetUp]
		public void SetUp()
		{
			layouter = new CircularCloudLayouter(Point.Empty);
		}

		[TestCaseSource(nameof(CentersSource))]
		public void Constructor_WithCorrectCenter_ShouldNotTrow(Point center)
		{
			var action = () => new CircularCloudLayouter(center);

			action.Should().NotThrow();
		}
		

		[TestCase(10, 10, TestName = "Square")]
		[TestCase(100, 10, TestName = "Horizontal oriented rectangle")]
		[TestCase(10, 100, TestName = "Vertical oriented rectangle")]
		public void PutNextRectangle_FirstRectangle_ShouldBeInCenter(int width, int height)
		{
			var rectangleSize = new Size(width, height);
			var actual = layouter.PutNextRectangle(rectangleSize);
			var expected = RectangleCreator.GetRectangle(Point.Empty, rectangleSize);

			actual.Should().BeEquivalentTo(expected);
		}

		[TestCase(0, 0, TestName = "Zero size")]
		[TestCase(0, 10, TestName = "Zero width")]
		[TestCase(10, 0, TestName = "Zero height")]
		[TestCase(-10, -10, TestName = "Negative size")]
		[TestCase(-10, 10, TestName = "Negative width")]
		[TestCase(10, -10, TestName = "Negative height")]
		
		public void PutNextRectangle_IncorrectRectangleSize_ShouldThrowArgumentException(int width, int height)
		{
			var rectangleSize = new Size(width, height);
			var action = () => layouter.PutNextRectangle(rectangleSize);
			
			action.Should().NotThrow<ArgumentException>();
		}

		[TestCaseSource(nameof(RectangleSizesSource))]
		public void PutNextRectangle_ShouldNotContainIntersection(List<Size> sizes)
		{
			var placedRectangles = sizes.Select(size => layouter.PutNextRectangle(size)).ToList();

			//Изначально тут были for, но решарпер предложил заменить на linq, не уверен что это легче читается
			var intersectionsCount = placedRectangles
				.Select((rectangle, i) =>
					placedRectangles
						.Where((_, j) => i != j)
						.Count(rectangle.IntersectsWith))
				.Sum();

			intersectionsCount.Should().Be(0);
		}

		public static IEnumerable<Point> CentersSource()
		{
			yield return Point.Empty;
			yield return new Point(1, 1);
			yield return new Point(-1, 1);
			yield return new Point(1, -1);
			yield return new Point(-1, -1);
		}

		public static IEnumerable<List<Size>> RectangleSizesSource()
		{
			var smallRectangleSize = new Size(15, 5);
			var mediumRectangleSize = new Size(50, 20);
			var bigRectangleSize = new Size(200, 70);

			yield return new List<Size>()
			{
				smallRectangleSize,
				mediumRectangleSize,
				bigRectangleSize,
			};

			yield return new List<Size>()
			{
				bigRectangleSize,
				bigRectangleSize,
				mediumRectangleSize,
				mediumRectangleSize,
				smallRectangleSize,
				smallRectangleSize,
				mediumRectangleSize,
				mediumRectangleSize,
				bigRectangleSize,
				bigRectangleSize
			};

			yield return new List<Size>()
			{
				smallRectangleSize,
				mediumRectangleSize,
				bigRectangleSize,
				mediumRectangleSize,
				bigRectangleSize,
				smallRectangleSize,
				bigRectangleSize
			};
		}
	}
}