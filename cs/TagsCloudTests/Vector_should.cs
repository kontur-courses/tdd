using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    class Vector_should
    {
        [Test]
        public void ReturnTrue_WhenVectorsAreCoDirected()
        {
            var vector1 = new Vector(new Point(0, 0), new Point(1, 0));
            var vector2 = new Vector(new Point(10, 10), new Point(54, 10));

            var result = vector1.IsSameDirection(vector2);

            result.Should().BeTrue();
        }

        [Test]
        public void ReturnFalse_WhenVectorsAreNotCoDirected()
        {
            var vector1 = new Vector(new Point(0, 0), new Point(1, 0));
            var vector2 = new Vector(new Point(-10, 11), new Point(-54, 5));

            var result = vector1.IsSameDirection(vector2);

            result.Should().BeFalse();
        }

        [Test]
        public void ReturnPositiveNumber_WhenVectorsAreCoDirected()
        {
            var vector1 = new Vector(new Point(0, 0), new Point(1, 0));
            var vector2 = new Vector(new Point(10, 10), new Point(54, 10));

            var result = vector1.ScalarMultiply(vector2);

            result.Should().BePositive();
        }

        [Test]
        public void ReturnNegativeNumber_WhenVectorsAreNotCoDirected()
        {
            var vector1 = new Vector(new Point(0, 0), new Point(1, 0));
            var vector2 = new Vector(new Point(-10, 11), new Point(-54, 5));

            var result = vector1.ScalarMultiply(vector2);

            result.Should().BeNegative();
        }

        [Test]
        public void ReturnZero_WhenOrthogonalVectorsAreMultiplied()
        {
            var vector1 = new Vector(new Point(0, 0), new Point(5, 0));
            var vector2 = new Vector(new Point(-1, 1), new Point(-1, 5));

            var result = vector1.ScalarMultiply(vector2);

            result.Should().Be(0);
        }
    }
}
