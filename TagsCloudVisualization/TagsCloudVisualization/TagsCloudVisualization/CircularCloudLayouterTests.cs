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
		private CircularCloudLayouter cloud;
		private Point center;

		[SetUp]
		public void SetUp()
		{
			center = new Point(500, 400);
			cloud = new CircularCloudLayouter(center);
		}

		[TestCase(1, TestName = "one")]
		[TestCase(42, TestName = "fourty two")]
		[TestCase(100, TestName = "one hundred")]
		public void PutNextRectangle_ShouldAddNumberOfRectangles(int number)
		{
			for (var i = 0; i < number; i++)
			{
				var size = new Size(10 + i, 5 + i);
				cloud.PutNextRectangle(size);
			}
			var rectangles = cloud.GetRectangles();

			rectangles.Should().HaveCount(number);
		}

		[Test]
		public void PutNextRectangle_WithCorrectSize_ReturnRectangleWithThisSize()
		{
			var size = new Size(100, 50);
			var rectangle = cloud.PutNextRectangle(size);

			rectangle.Size.ShouldBeEquivalentTo(size);
		}

		[TestCase(100, 200, TestName = "height more than width")]
		[TestCase(300, 100, TestName = "width more than height")]
		public void PutNextRectangle_NextRectangleShouldNotIntersectWithPrevious(int width, int height)
		{
			var size = new Size(width, height);
			var nextRectangle = cloud.PutNextRectangle(size);
			var previousRectangle = cloud.PutNextRectangle(size);

			nextRectangle.IntersectsWith(previousRectangle).Should().BeFalse();
		}

		public void NextRectangleShouldNotIntersectWithExisting(int number)
		{
			var rectangles = new List<Rectangle>
			{
				cloud.PutNextRectangle(new Size(100, 50)),
				cloud.PutNextRectangle(new Size(150, 100))
			};
			var actualRectangle = cloud.PutNextRectangle(new Size(50, 100));

			rectangles.Any(r => r.IntersectsWith(actualRectangle)).Should().BeFalse();
		}


		[Test]
		public void PutNextRectangle_WithNegativeSize_ShouldThrowException()
		{
			var size = new Size(-100, -100);
			Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(size));
		}
	}
}