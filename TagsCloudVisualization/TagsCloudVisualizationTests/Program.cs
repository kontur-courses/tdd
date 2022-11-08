using System.Drawing;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
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
			layouter = new CircularCloudLayouter(new Point(50, 50));
		}

		[Test]
		public void Constructor_ShouldBeNotNull_AfterCreation()
		{
			layouter.Should().NotBeNull();
		}
	}
}