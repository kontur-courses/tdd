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
            var result = false;

            for (var i = 0; i < 10; i++)
            {
                var rectangle = circularCloudLayouter.PutNextRectangle(size);
                if (result)
                    break;
                result = rectangle.IsIntersectsWithAnyRect(rectangles);
                rectangles.Add(rectangle);
            }

            result.Should().BeFalse();
        }
    }
}
