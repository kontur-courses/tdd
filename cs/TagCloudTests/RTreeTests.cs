using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests
{
    public class RTreeTests
    {
        [Test]
        public void RTree_ShouldBeEmpty_AfterCreation()
        {
            var tree = new RTree();

            tree.GetRectanglesForNode().Should().BeEmpty();
        }

        [Test]
        public void Insert_ShouldAddRectangleToNode_AfterAdd()
        {
            var tree = new RTree();
            var addedRectangle = new Rectangle(new Point(2, 2), new Size(3, 4));

            tree.Insert(addedRectangle);

            tree.GetRectanglesForNode().Should().HaveCount(1).And.Contain(addedRectangle);
        }

        [Test]
        public void RTreeNodeBound_ShouldBeEqualFirstRectangle_AfterAdd()
        {
            var tree = new RTree();
            var addedRectangle = new Rectangle(new Point(2, 2), new Size(3, 4));
            var expectedBound = addedRectangle;


            tree.Insert(addedRectangle);

            tree.NodeBound.Should().Be(expectedBound);

        }

        [Test]
        public void Insert_ShouldUpdateNodeBounds_AfterAddRectangle()
        {
            var tree = new RTree();
            var addedRectangle1 = new Rectangle(new Point(2, 2), new Size(3, 4));
            var addedRectangle2 = new Rectangle(new Point(5, 7), new Size(4, 2));
            var expectedBound = new Rectangle(addedRectangle1.Location, new Size(7, 7));


            tree.Insert(addedRectangle1);
            tree.Insert(addedRectangle2);

            tree.NodeBound.Should().Be(expectedBound);

        }

       
        private RTree InsertManyRectangles(int rectanglesCount, RTree tree, Rectangle first)
        {
            var prev = first;
            var deltaCoor = Math.Max(first.Width, first.Height);
            for (var _ = 0; _ < rectanglesCount; _++)
            {
                var newR = new Rectangle(new Point(prev.X + deltaCoor, prev.Y + deltaCoor), prev.Size);
                tree.Insert(newR);
                prev = newR;
            }

            return tree;
        }
    }
}
