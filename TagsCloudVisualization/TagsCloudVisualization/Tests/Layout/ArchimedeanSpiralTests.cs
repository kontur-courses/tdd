using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Layout;

namespace TagsCloudVisualization.Tests.Layout
{
	[TestFixture]
	public class ArchimedeanSpiralTests
	{
		[Test]
		[Ignore("Not implemented yet")]
		public void GetNextPoint_OnFirstCall_ShouldReturnCenter()
		{
			var center = new Point(100, 100);
			var spiral = new ArchimedeanSpiral(center);

			var point = spiral.GetNextPoint();

			point.Should().Be(center);
		}

		[Test]
		[Ignore("Not implemented yet")]
		public void GetNextPoint_SeveralCalls_ShouldProduceGrowingRadii()
		{
			var center = new Point(0, 0);
			var spiral = new ArchimedeanSpiral(center);
			spiral.GetNextPoint();

			var radii = Enumerable.Range(0, 20)
				.Select(_ => spiral.GetNextPoint().DistanceTo(center))
				.ToList();

			radii.Should().BeInAscendingOrder();
		}
	}
}