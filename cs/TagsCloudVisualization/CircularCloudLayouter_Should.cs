using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void CircularCloudLayouter_ShouldThrowArgumentException_WhenNegativeX_Y()
        {
            Action act = () =>
            {
                var cloudLayouter = new CircularCloudLayouter(new Point(-1, -1));
            };
            act.Should().Throw<ArgumentException>();
        }
    }
}
