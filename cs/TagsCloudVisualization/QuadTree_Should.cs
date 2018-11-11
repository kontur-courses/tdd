using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class QuadTree_Should
    {
        private class TestQuadTreeNode
        {
            public int Id;
            public Rectangle Rectangle;
        }
        
        [Test]
        public void Constructor_ShouldThrowsInvalidArgumentsException_WhenBoundsSizeAreZero()
        {
            Action act = () => new QuadTree<Rectangle>(new Rectangle(0, 0, 0, 0));
            act.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void Constructor_ShouldBeSetCorrectInitialBounds()
        {
            var initialBounds = new Rectangle(0, 0, 500, 500);
            var qt = new QuadTree<Rectangle>(initialBounds);
            qt.Bounds.Should().BeEquivalentTo(initialBounds);
        }
        
        [Test]
        public void Insert_ShouldThrowsInvalidArgumentsException_WhenBoundsSizeAreZero()
        {
            var qt = new QuadTree<Rectangle>(new Rectangle(0, 0, 100, 100));
            var insertRect = new Rectangle(0, 0, 0, 0);
            Action act = () => qt.Insert(insertRect, insertRect);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bounds_ShouldBeResized_WhenInsertRectangleAreOutsideCurrentBounds()
        {
            var initialBounds = new Rectangle(0, 0, 500, 500);
            var qt = new QuadTree<Rectangle>(initialBounds);
            var insertRect = new Rectangle(-500, -500, 100, 100);
            qt.Insert(insertRect, insertRect);
            qt.Bounds.Should().BeEquivalentTo(new Rectangle(-1500, -1500, 2500, 2500));
        }

        [TestFixture]
        public class TreeChangesShouldBeCorrect
        {
            private static Rectangle[] _testRectangles =
            {
                new Rectangle(0, 0, 100, 100),
                new Rectangle(-150, -50, 150, 50),
                new Rectangle(0, -150, 250, 150),
                new Rectangle(-180, 0, 180, 100),
                new Rectangle(-250, -200, 250, 150)
            };

            private static Rectangle[] _searchBounds =
            {
                new Rectangle(-50, -80, 100, 100),
                new Rectangle(-50, -80, 40, 40),
                new Rectangle(-50, -80, 60, 40),
                new Rectangle(0, 0, 100, 100),
                new Rectangle(-500, -500, 1000, 1000),
                new Rectangle(10, 10, 10, 10)
            };
        
            private QuadTree<TestQuadTreeNode> _quadTree;
        
            [SetUp]
            public void SetUp()
            {
                _quadTree = new QuadTree<TestQuadTreeNode>(new Rectangle(-500, -500, 1000, 1000));

                for (int i = 0; i < _testRectangles.Length; i++)
                {
                    var node = new TestQuadTreeNode()
                    {
                        Id = i + 1,
                        Rectangle = _testRectangles[i]
                    };
                        
                    _quadTree.Insert(node, node.Rectangle);
                }
            }
                
            [Test]
            public void InsertedNodesCount_ShouldBeEqualsToTestNodesCount()
            {
                _quadTree.Nodes.Count().Should().Be(_testRectangles.Length);
            }

            [TestCaseSource(nameof(_searchBounds))]
            public void GetNodesInside_ShouldBeReturnNodesInsideBounds_Where(Rectangle bounds)
            {
                var insideNodesIds = _quadTree.GetNodesInside(bounds).ToList().Select(n => n.Id);
                var overlappedNodesIds = SlowGetOverlappedNodes(bounds, _quadTree.Nodes).Select(n => n.Id);
                insideNodesIds.Should().BeEquivalentTo(overlappedNodesIds);
            }

            [TestCaseSource(nameof(_searchBounds))]
            public void HasNodesInside_ShouldBeTrue_Where(Rectangle bounds)
            {
                _quadTree.HasNodesInside(bounds).Should().BeTrue();
            }
            
            [Test]
            public void AfterRemove_GetNodesInside_ShouldBeReturnCorrectNodes()
            {
                var removeNodes = _quadTree.Nodes.ToList();
                
                foreach (var node in removeNodes)
                {
                    _quadTree.Remove(node);
                        
                    foreach (var bounds in _searchBounds)
                    {
                        var insideNodesIds = _quadTree.GetNodesInside(bounds).Select(n => n.Id);
                        var overlappedNodesIds = SlowGetOverlappedNodes(bounds, _quadTree.Nodes).Select(n => n.Id);
                        insideNodesIds.Should().BeEquivalentTo(overlappedNodesIds);
                    }
                }
            }

            [Test]
            public void ReIndex_ShouldBeCorrect_WhenBoundsChanged()
            {
                var insertNode = new TestQuadTreeNode()
                {
                    Id = _quadTree.Nodes.Count() + 1,
                    Rectangle = new Rectangle(-600, -600, 100, 100)
                };
                
                _quadTree.Insert(insertNode, insertNode.Rectangle);
                
                foreach (var bounds in _searchBounds)
                {
                    var insideNodesIds = _quadTree.GetNodesInside(bounds).Select(n => n.Id);
                    var overlappedNodesIds = SlowGetOverlappedNodes(bounds, _quadTree.Nodes).Select(n => n.Id);
                    insideNodesIds.Should().BeEquivalentTo(overlappedNodesIds);
                }
                
                _quadTree.HasNodesInside(new Rectangle(-550, -550, 100, 100)).Should().BeTrue();
            }
            
            private IEnumerable<TestQuadTreeNode> SlowGetOverlappedNodes(Rectangle rect, IEnumerable<TestQuadTreeNode> nodes)
            {
                var result = new List<TestQuadTreeNode>();
                
                foreach (var node in nodes)
                {
                    if (node.Rectangle.OverlapsWith(rect))
                    {
                        result.Add(node);
                    }
                }

                return result;
            }
        }
    }
}