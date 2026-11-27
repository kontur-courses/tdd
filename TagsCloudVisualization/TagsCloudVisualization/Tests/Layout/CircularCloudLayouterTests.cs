using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Layout;

namespace TagsCloudVisualization.Tests.Layout
{
	[TestFixture]
	public class CircularCloudLayouterTests
	{
		[Test]
		[Ignore("Not implemented yet")]
		public void PutNextRectangle_FirstRectangle_ShouldBeCenteredAtCloudCenter()
		{
			var center = new Point(400, 300);
			var size = new Size(80, 40);
			var layouter = new CircularCloudLayouter(center);

			var rectangle = layouter.PutNextRectangle(size);

			rectangle.Size.Should().Be(size);
			rectangle.Center().Should().Be(center);
		}

		[Test]
		[Ignore("Not implemented yet")]
		public void PutNextRectangle_TwoRectangles_ShouldNotIntersect()
		{
			var layouter = new CircularCloudLayouter(new Point(400, 300));

			var first = layouter.PutNextRectangle(new Size(80, 40));
			var second = layouter.PutNextRectangle(new Size(80, 40));

			second.IntersectsWith(first).Should().BeFalse();
		}

		[Test]
		[Ignore("Not implemented yet")]
		public void PutNextRectangle_ManyRectangles_ShouldNotIntersectEachOther()
		{
			var layouter = new CircularCloudLayouter(new Point(400, 300));
			var sizes = Enumerable.Range(0, 50)
				.Select(i => new Size(20 + i % 30, 15 + i % 20))
				.ToArray();

			var rectangles = sizes.Select(s => layouter.PutNextRectangle(s)).ToList();

			foreach (var r in rectangles)
			{
				r.IntersectsWithAny(rectangles.Where(o => o != r)).Should().BeFalse();
			}
		}

		[Test]
		[Ignore("Not implemented yet")]
		public void PutNextRectangle_ManyRectangles_ShouldFormCircleAroundCenter()
		{
			var center = new Point(400, 300);
			var layouter = new CircularCloudLayouter(center);

			var rectangles = Enumerable.Range(0, 100)
				.Select(_ => layouter.PutNextRectangle(new Size(30, 20)))
				.ToList();
			var totalArea = rectangles.Sum(r => (double)r.Width * r.Height);
			var idealRadius = Math.Sqrt(totalArea / Math.PI);
			var actualMaxRadius = rectangles.Max(r => r.Center().DistanceTo(center));

			actualMaxRadius.Should().BeLessThan(idealRadius * 2.0);
		}

		[Test]
		[Ignore("Not implemented yet")]
		public void PutNextRectangle_ManyRectangles_ShouldBeDense()
		{
			var center = new Point(400, 300);
			var layouter = new CircularCloudLayouter(center);

			var rectangles = Enumerable.Range(0, 100)
				.Select(_ => layouter.PutNextRectangle(new Size(30, 20)))
				.ToList();
			var totalArea = rectangles.Sum(r => (double)r.Width * r.Height);
			var maxRadius = rectangles.Max(r => r.Center().DistanceTo(center));
			var enclosingArea = Math.PI * maxRadius * maxRadius;

			(totalArea / enclosingArea).Should().BeGreaterThan(0.4);
		}
	}
}