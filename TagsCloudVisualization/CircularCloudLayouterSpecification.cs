using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	[TestFixture]
	class CircularCloudLayouterSpecification
	{
		private CircularCloudLayouter layouter;
		private Point center;

		[SetUp]
		public void SetUp()
		{
			center = new Point(100, 1000);
			layouter = new CircularCloudLayouter(center);
		}

		[Test]
		public void ShouldPlaceLeftTopVertexAtCenter_WhenFirstRectangle()
		{
			layouter.PutNextRectangle(new Size(1, 2)).ShouldBeEquivalentTo(new Rectangle(center.X, center.Y, 1, 2));
		}

		[Test]
		public void ShouldPlaceLeftBottomVertexAtCenter_WhenSecondRectangle()
		{
			layouter.PutNextRectangle(new Size(1, 2));
			layouter.PutNextRectangle(new Size(3, 4)).ShouldBeEquivalentTo(new Rectangle(center.X, center.Y - 4, 3, 4));
		}

		[Test]
		public void ShouldPlaceRightBottomVertexAtCenter_WhenThirdRectangle()
		{
			layouter.PutNextRectangle(new Size(1, 2));
			layouter.PutNextRectangle(new Size(3, 4));
			layouter.PutNextRectangle(new Size(5, 6)).ShouldBeEquivalentTo(new Rectangle(center.X - 5, center.Y - 6, 5, 6));
		}

		[Test]
		public void ShouldPlaceRightTopVertexAtCenter_WhenFourthRectangle()
		{
			layouter.PutNextRectangle(new Size(1, 2));
			layouter.PutNextRectangle(new Size(3, 4));
			layouter.PutNextRectangle(new Size(5, 6));
			layouter.PutNextRectangle(new Size(7, 8)).ShouldBeEquivalentTo(new Rectangle(center.X - 7, center.Y, 7, 8));
		}

		[Test]
		public void ShouldPlaceRectangle_WhenFifthRectangle()
		{
			layouter.PutNextRectangle(new Size(1, 2));
			layouter.PutNextRectangle(new Size(3, 4));
			layouter.PutNextRectangle(new Size(5, 6));
			layouter.PutNextRectangle(new Size(7, 8));
			layouter.PutNextRectangle(new Size(9, 10)).Should().NotBe(new Rectangle());
		}

		//NOTE: Перед написанием последующих тестов стоит сделать рефакторинг реализации, чтобы вместо константных прямоугольников возвращались автопостроенные вокруг центра.

		[Test]
		public void ShouldPlaceManyRectangles()
		{
			var rectangles = new List<Rectangle>();
			const int count = 100;
			for (var i = 0; i < count; i++)
			{
				var nextRectangle = layouter.PutNextRectangle(new Size(i, i));
				nextRectangle.Should().NotBe(new Rectangle(), $"{i + 1} is less that {count}");
				rectangles.All(r => !RectangleExtensions.IsIntersectedWith(r, nextRectangle)).Should().BeTrue();

				rectangles.Add(nextRectangle);
			}
		}

		[Test]
		public void ShouldPreferCloserToCenterPoints_WhenCloserByX()
		{
			layouter.PutNextRectangle(new Size(10, 10));
			layouter.PutNextRectangle(new Size(10, 10));
			layouter.PutNextRectangle(new Size(5, 10));
			layouter.PutNextRectangle(new Size(10, 10));

			var points1 = layouter.PutNextRectangle(new Size(1, 1)).GetPoints();
			points1.Should().Contain(new Point(center.X - 5, center.Y));
		}

		[Test]
		public void ShouldPreferCloserToCenterPoints_WhenCloserByY()
		{
			layouter.PutNextRectangle(new Size(10, 10));
			layouter.PutNextRectangle(new Size(10, 10));
			layouter.PutNextRectangle(new Size(10, 5));
			layouter.PutNextRectangle(new Size(10, 10));

			var points1 = layouter.PutNextRectangle(new Size(1, 1)).GetPoints();
			points1.Should().Contain(new Point(center.X, center.Y - 5));
		}

		[Test, Ignore("not ready")]
		public void ShouldPreferCloserToCenterPointsAnticlockwise()
		{
			layouter.PutNextRectangle(new Size(10, 10));
			layouter.PutNextRectangle(new Size(10, 10));
			layouter.PutNextRectangle(new Size(10, 10));
			layouter.PutNextRectangle(new Size(10, 10));

			layouter.PutNextRectangle(new Size(1, 1)).GetPoints()
				.Should().Contain(new Point(center.X + 10, center.Y));
			layouter.PutNextRectangle(new Size(1, 1)).GetPoints()
				.Should().Contain(new Point(center.X + 10, center.Y));

			layouter.PutNextRectangle(new Size(1, 1)).GetPoints()
				.Should().Contain(new Point(center.X, center.Y - 10));
			layouter.PutNextRectangle(new Size(1, 1)).GetPoints()
				.Should().Contain(new Point(center.X, center.Y - 10));

			layouter.PutNextRectangle(new Size(1, 1)).GetPoints()
				.Should().Contain(new Point(center.X - 10, center.Y));
			layouter.PutNextRectangle(new Size(1, 1)).GetPoints()
				.Should().Contain(new Point(center.X - 10, center.Y));

			layouter.PutNextRectangle(new Size(1, 1)).GetPoints()
				.Should().Contain(new Point(center.X, center.Y - 10));
			layouter.PutNextRectangle(new Size(1, 1)).GetPoints()
				.Should().Contain(new Point(center.X, center.Y - 10));
		}
	}
}