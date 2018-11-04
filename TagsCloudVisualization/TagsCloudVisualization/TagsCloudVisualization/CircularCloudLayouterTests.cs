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
			var rectangles = AddRectanglesToCloud(number);
			rectangles.Should().HaveCount(number);
		}

		[Test]
		public void PutNextRectangle_WithCorrectSize_ReturnRectWithThisSize()
		{
			var size = new Size(100, 50);
			var rectangle = cloud.PutNextRectangle(size);

			rectangle.Size.ShouldBeEquivalentTo(size);
		}

		[TestCase(100, 200, TestName = "height more than width")]
		[TestCase(300, 100, TestName = "width more than height")]
		public void PutNextRectangle_TwoTimes_RectanglesShouldNotIntersect(int width, int height)
		{
			var size = new Size(width, height);
			var theRectangle = cloud.PutNextRectangle(size);
			var otherRectangle = cloud.PutNextRectangle(size);

			theRectangle.IntersectsWith(otherRectangle).Should().BeFalse();
		}

		[TestCase(10, TestName = "10 rectangles")]
		[TestCase(100, TestName = "100 rectangles")]
		[TestCase(500, TestName = "500 rectangles")]
		public void PutNextRectangle_NewRectangleDoesNotIntersectWithExist(int number)
		{
			var rectangles = new List<Rectangle>();
			rectangles.AddRange(AddRectanglesToCloud(number));
			var actualRectangle = cloud.PutNextRectangle(new Size(100, 50));

			IsIntersect(actualRectangle, rectangles).Should().BeFalse();
		}


		[Test]
		public void PutNextRectangle_HaveNegativeSize_ShouldThrowException()
		{
			var size = new Size(-100, -100);
			Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(size));
		}

		[Test]
		public void PutNextRectangle_TwoTimes_ShouldDifferentStartPoints()
		{
			var size = new Size(100, 50);
			var theRectangle = cloud.PutNextRectangle(size);
			var otherRectangle = cloud.PutNextRectangle(size);
			var theStartPoint = new[] {theRectangle.X, theRectangle.Y};
			var otherStartPoint = new[] {otherRectangle.X, otherRectangle.Y};

			theStartPoint.Should().NotBeEquivalentTo(otherStartPoint);
		}

		private bool IsIntersect(Rectangle rectangle, List<Rectangle> rectangles) =>
			rectangles.Any(r => r.IntersectsWith(rectangle));

		private List<Rectangle> AddRectanglesToCloud(int number)
		{
			var rnd = new Random();
			for (var i = 0; i < number; i++)
				cloud.PutNextRectangle(new Size(rnd.Next(10, 30), rnd.Next(4, 12)));

			return cloud.GetExistRectangles();
		}
	}
}