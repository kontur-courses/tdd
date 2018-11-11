using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Rectangle_Should
    {
        private static Rectangle[][] _overlappingRectangles =
        {
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),
                new Rectangle(0, 0, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),
                new Rectangle(50, 50, 10, 10),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),
                new Rectangle(50, 0, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),
                new Rectangle(-50, 0, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),   
                new Rectangle(0, 50, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),   
                new Rectangle(0, -50, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),   
                new Rectangle(0, 50, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),   
                new Rectangle(-50, 50, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),   
                new Rectangle(-50, -50, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),   
                new Rectangle(50, -50, 100, 100),
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 10, 10),   
                new Rectangle(-100, -100, 200, 200),
            }
        };

        private static Rectangle[][] _notOverlappingRectangles =
        {
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),
                new Rectangle(100, 100, 100, 100)   
            },
            
            new Rectangle[] {
                new Rectangle(0, 0, 100, 100),
                new Rectangle(1000, 1000, 100, 100)   
            }
        };
        
        [Test, TestCaseSource(nameof(_overlappingRectangles))]
        public void OverlapsWith_ShouldBeTrue_When(Rectangle[] rects)
        {
            rects[0].OverlapsWith(rects[1]).Should().BeTrue();
            rects[1].OverlapsWith(rects[0]).Should().BeTrue();
        }
        
        [Test, TestCaseSource(nameof(_notOverlappingRectangles))]
        public void OverlapsWith_ShouldBeFalse_When(Rectangle[] rects)
        {
            rects[0].OverlapsWith(rects[1]).Should().BeFalse();
            rects[1].OverlapsWith(rects[0]).Should().BeFalse();
        }

        [Test]
        public void GetOuterRect_ShouldBeCorrect_WhenRectsHasPositiveCoordinates()
        {
            var rects = new Rectangle[]
            {
                new Rectangle(100, 100, 100, 100), 
                new Rectangle(150, 150, 100, 100), 
            };
            
            var expectedOuterRect = new Rectangle(100, 100, 150, 150);
            var actualOuterRect = Rectangle.GetOuterRect(rects);

            actualOuterRect.Should().BeEquivalentTo(expectedOuterRect);
        }

        [Test]
        public void GetOuterRect_ShouldBeCorrect_WhenRectsHasNegativeCoordinates()
        {
            var rects = new Rectangle[]
            {
                new Rectangle(-200, -200, 100, 100), 
                new Rectangle(-150, -150, 100, 100), 
            };
            
            var expectedOuterRect = new Rectangle(-200, -200, 150, 150);
            var actualOuterRect = Rectangle.GetOuterRect(rects);

            actualOuterRect.Should().BeEquivalentTo(expectedOuterRect);
        }

        [Test]
        public void GetOuterRect_ShouldBeCorrect_WhenRectsHasNegativeAndPositiveCoordinates()
        {
            var rects = new Rectangle[]
            {
                new Rectangle(-100, -100, 100, 100), 
                new Rectangle(0, 0, 100, 100), 
            };
            
            var expectedOuterRect = new Rectangle(-100, -100, 200, 200);
            var actualOuterRect = Rectangle.GetOuterRect(rects);

            actualOuterRect.Should().BeEquivalentTo(expectedOuterRect);
        }

        [Test]
        public void BottomRightPoint_ShouldBeCorrect()
        {
            var rect = new Rectangle(-100, -100, 100, 100);
            rect.BottmRightPoint.Should().BeEquivalentTo(new Point(0, 0));
        }

        [Test]
        public void CenterPoint_ShouldBeCorrect()
        {
            var rect = new Rectangle(-100, -100, 100, 100);
            rect.CenterPoint.Should().BeEquivalentTo(new Point(-50, -50));
        }
    }
}