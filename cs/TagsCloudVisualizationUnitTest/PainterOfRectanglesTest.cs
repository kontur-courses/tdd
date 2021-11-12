using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationUnitTest
{
    class PainterOfRectanglesTest
    {
        [Test]
        public void CreateImage_ShouldBeThrow_When_SizeCloud_Greatest_SizeImage()
        {
            var painter = new PainterOfRectangles(new Size(10, 10));
            var centrePoint = new Point(10, 10);
            var spiral = new ArchimedesSpiral(centrePoint);
            var circularCloudLayouter = new CircularCloudLayouter(centrePoint, spiral);
            var rectangles = new List<Rectangle>();
            var saver = new SaverImage("test");

            for (int i = 0; i < 100; i++)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(100, 100)));
            }

            Action act = () => painter.CreateImage(rectangles, saver);

            act.Should().Throw<Exception>();
        }

        [Test]
        public void CreateImage_ShouldBeThrow_When_CommandIsNull()
        {
            var painter = new PainterOfRectangles(new Size(10, 10));
            var centrePoint = new Point(10, 10);
            var spiral = new ArchimedesSpiral(centrePoint);
            var circularCloudLayouter = new CircularCloudLayouter(centrePoint, spiral);
            var rectangles = new List<Rectangle>();
            var saver = new SaverImage("test");

            for (int i = 0; i < 100; i++)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(100, 100)));
            }

            Action act = () => painter.CreateImage(rectangles, saver);

            act.Should().Throw<Exception>();
        }
    }
}