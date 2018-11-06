using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

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

		[TearDown]
		public void TearDown()
		{
			if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
			{
				var rectangles = cloud.GetRectangles();
				var vizualizer = new RectangleTagsCloudVisualizer(center.X * 3, center.Y * 3);
				var picture = vizualizer.GetPicture(rectangles);
				var path = $"{TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.FullName}";
				picture.Save($"{path}.png");
				TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
			}
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
		public void NextRectangleShouldNotIntersectWithPrevious(int width, int height)
		{
			var size = new Size(width, height);
			var nextRectangle = cloud.PutNextRectangle(size);
			var previousRectangle = cloud.PutNextRectangle(size);

			nextRectangle.IntersectsWith(previousRectangle).Should().BeFalse();
		}

		[Test]
		public void PutNextRectangle_WithNegativeSize_ShouldThrowException()
		{
			var size = new Size(-100, -100);
			Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(size));
		}

		[TestCase(50)]
		[TestCase(150)]
		[TestCase(400)]
		public void PutNextRectangles_MustBePlacedTightly(int number)
		{
			for (var i = 0; i < number; i++)
				cloud.PutNextRectangle(new Size(i + 1, i + 1));

			var rectangles = cloud.GetRectangles();
			var min = new Point(rectangles.Min(r => r.Left), rectangles.Min(r => r.Top));
			var max = new Point(rectangles.Max(r => r.Right), rectangles.Min(r => r.Bottom));
			double rectanglesArea = rectangles.Sum(r => r.Width * r.Height);
			double wholeArea = (max.X - min.X) * (max.Y - min.Y);

			(rectanglesArea / wholeArea).Should().BeGreaterThan(0.5);
		}
	}
}