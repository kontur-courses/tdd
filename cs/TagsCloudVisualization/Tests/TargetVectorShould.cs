using FluentAssertions;
using NUnit.Framework;
using System.Drawing;



namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class TargetVectorShould
    {
        TargetVector vector;
        Point target;
        Point location;

        [SetUp]
        public void SetUp()
        {
            target = new Point(2, 3);
            location = new Point(5, 7);
            vector = new TargetVector(target, location);
        }

        [Test]
        public void PartialDeltaReturnMinimalOffset()
        {
            foreach (var delta in vector.GetPartialDelta())
            {
                delta.X.Should().BeInRange(-1, 1);
                delta.Y.Should().BeInRange(-1, 1);
            }
        }

        [Test]
        public void PartialDeltaAllDeltaMoveToTraget()
        {
            var dx = 0;
            var dy = 0;
            foreach (var delta in vector.GetPartialDelta())
            {
                dx += delta.X;
                dy += delta.Y;
            }
            location.Offset(dx, dy);
            location.Should().Be(target);
        }

    }
}
