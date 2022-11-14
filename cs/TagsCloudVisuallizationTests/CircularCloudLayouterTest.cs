using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisuallizationTests
{
    [TestFixture]
    public class CircularCloudLayouterTest
    { 
        [TestCase(0, 0)]
        [TestCase(-1, -1)]
        public void PutNextRectangle_EmptySize_ThrowArgumentException(int sizeX, int sizeY)
        {
             CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
             Action action = () => layouter.PutNextRectangle(Size.Empty, null);
             action.Should().Throw<ArgumentException>().WithMessage("Size must not be equal to 0");
        }

        [TestCase(1, 1)]
        [TestCase(5, 5)]
        [TestCase(10, 10)]
        [TestCase(50, 50)]
        [TestCase(25, 25)]
        public void PutNextRectangle_ShouldNotIntersects(int sizeX, int sizeY)
        {
            var bitmap = new Bitmap(600, 600);
            var layouter = new CircularCloudLayouter(new Point(bitmap.Width / 2, bitmap.Height / 2));
            var visualisator = new RectangleVisualisator(layouter, bitmap);
            visualisator.AddRandomRectangles(10);
            
            Rectangle exampleRectangle = layouter.PutNextRectangle(new Size(sizeX, sizeY), visualisator.Rectangles);
            bool isIntersects = false;
            foreach (var rectangle in visualisator.Rectangles)
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