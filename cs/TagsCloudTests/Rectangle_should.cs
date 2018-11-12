using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    [TestFixture]
    class Rectangle_should
    {
        [Test]
        public void ReturnFalse_WhenRectanglesAreNotIntersect()
        {
            var size = new Size(10, 10);
            var center = new Point(0, 0);
            var rect1 = new Rectangle(center, center, size);
            var rect2 = new Rectangle(center, new Point(20, 0), size);

            var result = rect1.Intersects(rect2);

            result.Should().BeFalse();
        }

        [Test]
        public void ReturnTrue_WhenRectanglesAreIntersect()
        {
            var size = new Size(10, 10);
            var center = new Point(0, 0);
            var rect1 = new Rectangle(center, new Point(0, 0), size);
            var rect2 = new Rectangle(center, new Point(5, 0), size);

            var result = rect1.Intersects(rect2);

            result.Should().BeTrue();
        }
    }
}
