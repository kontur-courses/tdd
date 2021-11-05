using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Visualization;

namespace TagsCloud.Tests
{
    public class ArchimedesSpiralPointGeneratorTest
    {
        private readonly Point center = new(10, 10);
        private ArchimedesSpiralPointGenerator spiralPointGenerator;

        [SetUp]
        public void InitGenerator()
        {
            spiralPointGenerator = new ArchimedesSpiralPointGenerator(center);
        }

        [Test]
        public void GetNext_OnFirstCall_Should_ReturnCenter()
        {
            var point = spiralPointGenerator.GenerateNextPoint().First();

            point.Should().BeEquivalentTo(center);
        }

        [Test]
        public void GetNext_Should_ReturnPoints_WithSameRadii()
        {
            var points = spiralPointGenerator.GenerateNextPoint().Take(100)
                .ToList();

            var radii = points.Select(x => x.GetDistance(center)).ToList();

            foreach (var (previous, current) in radii.Zip(radii.Skip(1)))
                current.Should().BeInRange(previous, previous + 1);
        }

        [Test]
        public void GetNext_Should_ReturnPoints_WithIncreasingRadius()
        {
            var points = spiralPointGenerator.GenerateNextPoint().Take(100).ToList();

            var radii = points.Select(x => x.GetDistance(center)).ToList();

            foreach (var (previous, current) in radii.Zip(radii.Skip(1)))
                (current - previous).Should().BeGreaterThanOrEqualTo(0);
        }
    }
}