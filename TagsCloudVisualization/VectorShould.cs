using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class VectorShould
    {
        [Test]
        public void InitializeFieldsAfterInstanceCreation()
        {
            var begin = new Point(0, 0);
            var end = new Point(1, 1);

            var vector = new Vector(begin, end);

            vector.Begin.Should().BeEquivalentTo(begin);
            vector.End.Should().BeEquivalentTo(end);
        }
    }
}
