using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class TagsCloudPositioning_should
    {

        [Test]
        public void ReturnRectangleAtCenter_WhenAddFirstRectangle()
        {
            var origin = new Point(0, 0);
            var circularCloudLayouter = new CircularCloudLayouter(origin);
            var size = new Size(100, 100);

            var rectangle = circularCloudLayouter.PutNextRectangle(size);

            rectangle.Center.Should().BeEquivalentTo(origin);
        }

        [Test]
        public void NoOneRectangleShouldIntersectWithOthers_WhenAddMoreThenOne()
        {
            var origin = new Point(0, 0);
            var circularCloudLayouter = new CircularCloudLayouter(origin);
            var size = new Size(200, 100);
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 10; i++)
            {
                var rectangle = circularCloudLayouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
            }

            var result =
                (from rectangle1 in rectangles
                from rectangle2 in rectangles
                select rectangle1.Intersects(rectangle2)).Any(x => x);
            result.Should().BeFalse();
        }
    }
}
