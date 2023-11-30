using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Creates_WithoutException()
        {
            var createCloudLayouter = () => new CircularCloudLayouter(new Point(1,1));
            createCloudLayouter.Should().NotThrow();
        }
    }
}