using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    class CircularCloudLayouterTests
    {
        [Test]
        public void PutNextRectangle_PutFirstRectangle_Correct()
        {
            var center = new Point(10, 10);
            var layouter = new CircularCloudLayouter(center);
            var rectangleSize = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            rectangle.Location.ShouldBeEquivalentTo(new Point(5, 5));
        }
    }
}
