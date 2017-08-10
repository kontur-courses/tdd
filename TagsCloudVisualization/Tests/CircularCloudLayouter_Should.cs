using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void BeEmpty_OnCreate()
        {
            var layout = new CircularCloudLayouter(new Point(0,0));
            layout.GetCloud().Should().BeEmpty();
        }

        

    }
}