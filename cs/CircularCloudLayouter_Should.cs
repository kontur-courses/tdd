using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NUnit.Framework;



namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_Should
    {
        [Test]
        public void AddingOneRectangle_ShouldBeInCenter()
        {
            var cloudCenter = new Point(40, 40);
            var rectangleSize = new Size(20, 10);
            var expectedRectangle = new Rectangle(10, 15, 20, 10);


            var cloudLayouter = new CircularCloudLayouter(cloudCenter);
            var actualRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            //actualRectangle.
        }
    }
}
