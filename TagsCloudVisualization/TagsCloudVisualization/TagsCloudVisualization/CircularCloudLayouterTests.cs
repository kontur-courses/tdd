using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
	[TestFixture]
	class CircularCloudLayouterTests
	{
		private CircularCloudLayouter layouter;
		private Point center;

		[SetUp]
		public void SetUp()
		{
			center = new Point(500, 400);
			layouter = new CircularCloudLayouter(center);
		}

		[Test]
		public void PutNextRectangle_RectanglesShouldCountOne()
		{
			var size = new Size(100, 50);

			layouter.PutNextRectangle(size);

			layouter.rectangles.Should().HaveCount(1);
		}

		[Test]
		public void PutNextRectangle_WithCorrectSize_ReturnRectWithThisSize()
		{
			var size = new Size(100, 50);

			layouter.PutNextRectangle(size);

			layouter.rectangles.FirstOrDefault().Size.ShouldBeEquivalentTo(size);
		}

		[Test]
		public void TwoRectangles_ShouldIntersect()
		{
			var size = new Size(100, 50);

			var rectangle1 = new Rectangle(center, size);
			var rectangle2 = new Rectangle(center, size);

			rectangle1.IntersectsWith(rectangle2).Should().BeTrue();
		}

		[Test]
		public void PutNextRectangle_Twice_ShouldNotIntersect()
		{
			var size = new Size(100, 50);

			layouter.PutNextRectangle(size);
			layouter.PutNextRectangle(size);

			IsAnyIntersect(layouter.rectangles).Should().BeFalse();
		}

		[Test]
		public void PutNextRectangle_NewRectangleDoesNotIntersectWithExist()
		{
			var existRectangles = new List<Rectangle>();
			var size = new Size(100, 50);
			for (int i = 0; i < 10; i++)
				existRectangles.Add(layouter.PutNextRectangle(size));

			var actualRectangle = layouter.PutNextRectangle(size);

			IsIntersect(existRectangles, actualRectangle).Should().BeFalse();
		}


		[Test]
		public void PutNextRectangle_HaveNegativeSize_ShouldThrowException()
		{
			var size = new Size(-100, -100);

			Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(size));
		}

		[Test]
		public void PutNextRectangle_Twice_ShouldDifferentStartPoints()
		{
			var size = new Size(100, 50);

			var rectangle1 = layouter.PutNextRectangle(size);
			var rectangle2 = layouter.PutNextRectangle(size);
			var actualPoint1 = new[] { rectangle1.X, rectangle1.Y };
			var actualPoint2 = new[] { rectangle2.X, rectangle2.Y };

			actualPoint1.Should().NotBeEquivalentTo(actualPoint2);
		}

		bool IsAnyIntersect(List<Rectangle> rectangles)
		{
			for (var i = 0; i < rectangles.Count; i++)
				for (var j = i + 1; j < rectangles.Count; j++)
					if (rectangles[i].IntersectsWith(rectangles[j]))
						return true;

			return false;
		}
		bool IsIntersect(List<Rectangle> rectangles, Rectangle rect) =>
			rectangles.Count(r => r.IntersectsWith(rect)) > 0;
	}
}