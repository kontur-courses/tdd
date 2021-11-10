using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class PolygonShould
    {
        [Test]
        public void Normalize_DoesntChangeNormalPolygon()
        {
            var polygon = new Polygon(new[] { new PointF(0, 0), new PointF(0, 1),
                new PointF(1, 1), new PointF(1, 0) });
            var expected = new[] { new PointF(0, 0), new PointF(0, 1),
                new PointF(1, 1), new PointF(1, 0) };
            polygon.Normalize();
            polygon.Vertexes.Should().Equal(expected);
        }
        
        [Test]
        public void Normalize_RemovesDoubles()
        {
            var polygon = new Polygon(new[] { new PointF(0, 0), new PointF(0, 0),
                new PointF(0, 1), new PointF(1, 1), new PointF(1, 0), new PointF(1, 0) });
            var expected = new[] { new PointF(0, 0), new PointF(0, 1),
                new PointF(1, 1), new PointF(1, 0) };
            polygon.Normalize();
            polygon.Vertexes.Should().Equal(expected);
        }

        [Test]
        public void Normalize_RemovesUnnecessaryVertexes()
        {
            var polygon = new Polygon(new[] { new PointF(0, 0), new PointF(0, 0.5f), new PointF(0, 0.75f),
                new PointF(0, 1), new PointF(1, 1), new PointF(1, 0.5f), new PointF(1, 0) });
            var expected = new[] { new PointF(0, 0), new PointF(0, 1),
                new PointF(1, 1), new PointF(1, 0) };
            polygon.Normalize();
            polygon.Vertexes.Should().Equal(expected);
        }
        
        [Test]
        public void Normalize_RemovesUnnecessaryVertexesOnLastSegment()
        {
            var polygon = new Polygon(new[] { new PointF(0, 0), new PointF(0, 0.5f),
                new PointF(0, 1), new PointF(1, 1), new PointF(1, 0), new PointF(0.5f, 0) });
            var expected = new[] { new PointF(0, 0), new PointF(0, 1),
                new PointF(1, 1), new PointF(1, 0) };
            polygon.Normalize();
            polygon.Vertexes.Should().Equal(expected);
        }
    }
}