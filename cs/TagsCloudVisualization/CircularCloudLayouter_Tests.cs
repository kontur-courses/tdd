using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter layouter;
        private List<Size> sizes;
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(300, 300));
            sizes = new List<Size>() 
            {
                new Size(10, 5), 
                new Size(20, 10), 
                new Size(10, 5), 
                new Size(15, 15)
            };

        }
        
        [Test]
        public void PutNextRectangle_ShouldReturnRectangleWithSameSize_WhenSomeSizesAdded()
        {
            foreach (var size in sizes)
                layouter.PutNextRectangle(size).Size
                    .Should().BeEquivalentTo(size);
        }
        
        [Test]
        public void Layouter_ShouldContainsAllRectangles_WhenSomeSizesAdded()
        {
            FillLayouterWithSomeRectangles(layouter, sizes);

            layouter.Rectangles.Select(rect => rect.Size)
                .Should().BeEquivalentTo(sizes);
        }
        
        [Test]
        public void LayouterRectangles_ShouldNotIntersectEachOther_WhenSomeRectanglesAdded()
        {
            FillLayouterWithSomeRectangles(layouter, sizes);

            foreach (var rectangle in layouter.Rectangles)
            {
                foreach (var otherRectangle in layouter.Rectangles)
                {
                    if (rectangle != otherRectangle)
                        rectangle.IntersectsWith(otherRectangle)
                            .Should().BeFalse();
                }
            }
        }
        
        [Test]
        public void LayouterShape_ShouldBeCloseToCircle_WhenManySizesAdded()
        {
            throw new NotImplementedException();
        }
        
        private void FillLayouterWithSomeRectangles(CircularCloudLayouter layouter,
            List<Size> rectangleSizes)
        {
            foreach (var size in rectangleSizes)
                layouter.PutNextRectangle(size);
        }
    }
}
