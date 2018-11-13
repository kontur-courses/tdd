using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class SizeShould
    {
        [Test]
        public void HaveRandomValueInBound()
        {
            for (var i = 0; i < 15; i++)
            {
                var size = new Size().GenerateRandom(-10, 11, -2, 13);

                size.Width.Should().BeGreaterOrEqualTo(-10).And.BeLessThan(11);
                size.Height.Should().BeGreaterOrEqualTo(-2).And.BeLessThan(13);
            }
        }
    }
}
