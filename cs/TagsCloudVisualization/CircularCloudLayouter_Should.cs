using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void ThrowException_WhenPutNotPositiveSize()
        {
            var layouter = new CircularCloudLayouter(new Point(7, 8));

            Action act = () => layouter.PutNextRectangle(new Size(-1, 0));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PlaceFirstFourRectanglesAroundCenter()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
            };
            
            var rect = layouter.PutNextRectangle(rectangles[0]);
            rect.Location.Should().Be(center);

            rect = layouter.PutNextRectangle(rectangles[1]);
            rect.X.Should().Be(center.X);
            rect.Bottom.Should().Be(center.Y);

            rect = layouter.PutNextRectangle(rectangles[2]);
            rect.Right.Should().Be(center.X);
            rect.Bottom.Should().Be(center.Y);
            
            rect = layouter.PutNextRectangle(rectangles[3]);
            rect.Right.Should().Be(center.X);
            rect.Y.Should().Be(center.Y);
        }

        [Test]
        public void NotHaveIntersections_WithFourRectangles()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
            };
            var result = rectangles.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result
                .Where(r => result.Where(rr => rr != r).Any(r.IntersectsWith)).Should().BeEmpty();
        }
        
        [Test]
        public void NotHaveIntersections_WithALotRectangles()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
                new Size(7, 5),
                new Size(3, 10),
                new Size(15, 1),
                new Size(31, 10),
                new Size(11, 26),
                new Size(12, 25),
                new Size(13, 24),
                new Size(14, 23),
                new Size(15, 22),
                new Size(16, 21),
                new Size(17, 20),
                new Size(18, 19),
            };
            var result = rectangles.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result
                .Where(r => result.Where(rr => rr != r).Any(r.IntersectsWith)).Should().BeEmpty();
        }
        
        [Test]
        public void NotChangeSize_WithFourRectangles()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
            };
            var result = rectangles.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result.Select(r => r.Size).Should().BeEquivalentTo(rectangles, options => options.WithStrictOrdering());
        }
        
        [Test]
        public void NotChangeSize_WithALotRectangles()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
                new Size(7, 5),
                new Size(3, 10),
                new Size(15, 1),
                new Size(31, 10),
                new Size(40, 12),
                new Size(11, 26),
                new Size(12, 25),
                new Size(13, 24),
                new Size(14, 23),
                new Size(15, 22),
                new Size(16, 21),
                new Size(17, 20),
                new Size(18, 19),
            };
            var result = rectangles.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result.Select(r => r.Size).Should().BeEquivalentTo(rectangles, options => options.WithStrictOrdering());
        }
    }
}