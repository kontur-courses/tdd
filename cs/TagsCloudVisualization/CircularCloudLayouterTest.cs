using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTest
    { 
        [Test] 
        public void PutNextRectangle_EmptySize_ThrowArgumentException()
        {
             CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
             Action action = () => layouter.PutNextRectangle(Size.Empty);
             action.Should().Throw<ArgumentException>().WithMessage("Size must not be equal to 0");
        }

        [TestCase(1, 1)]
        [TestCase(5, 5)]
        [TestCase(10, 10)]
        [TestCase(25, 25)]
        [TestCase(50, 50)]
        public void PutNextRectangle_ShouldNotIntersects(int sizeX, int sizeY)
        {
            CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.AddRandomRectangles(10);
            Rectangle exampleRectangle = layouter.PutNextRectangle(new Size(sizeX, sizeY));

            bool isIntersects = false;
            foreach (var rectangle in layouter.Rectangles)
            {
                if(exampleRectangle == rectangle)
                    continue;
                if (rectangle.IntersectsWith(exampleRectangle))
                    isIntersects = true;
            }
            isIntersects.Should().BeFalse();
        }
    }
}