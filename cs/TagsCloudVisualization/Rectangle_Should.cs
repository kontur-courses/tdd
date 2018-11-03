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
                new Rectangle(new Point(0, 0), new Size(100, 100)),
                new Rectangle(new Point(0, 0), new Size(100, 100)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),
                new Rectangle(new Point(50, 50), new Size(10, 10)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),
                new Rectangle(new Point(50, 0), new Size(100, 100)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),
                new Rectangle(new Point(-50, 0), new Size(100, 100)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),   
                new Rectangle(new Point(0, 50), new Size(100, 100)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),   
                new Rectangle(new Point(0, -50), new Size(100, 100)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),   
                new Rectangle(new Point(50, 50), new Size(100, 100)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),   
                new Rectangle(new Point(-50, 50), new Size(100, 100)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),   
                new Rectangle(new Point(-50, -50), new Size(100, 100)),
            },
            
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),   
                new Rectangle(new Point(50, -50), new Size(100, 100)),
            }
        };

        private static Rectangle[][] _notOverlappingRectangles =
        {
            new Rectangle[] {
                new Rectangle(new Point(0, 0), new Size(100, 100)),
                new Rectangle(new Point(100, 100), new Size(100, 100))   
            }
        };
        
        [Test, TestCaseSource(nameof(_overlappingRectangles))]
        public void IsOverlap_ShouldBeTrue_When(Rectangle[] rects)
        {
            Rectangle.IsOverlap(rects[0], rects[1]).Should().BeTrue();
            Rectangle.IsOverlap(rects[1], rects[0]).Should().BeTrue();
        }
        
        [Test, TestCaseSource(nameof(_notOverlappingRectangles))]
        public void IsOverlap_ShouldBeFalse_When(Rectangle[] rects)
        {
            Rectangle.IsOverlap(rects[0], rects[1]).Should().BeFalse();
            Rectangle.IsOverlap(rects[1], rects[0]).Should().BeFalse();
        }

        [Test]
        public void GetOuterRect_ShouldBeCorrect_WhenRectsHasPositiveCoordinates()
        {
            var rects = new Rectangle[]
            {
                new Rectangle(new Point(100, 100), new Size(100, 100)), 
                new Rectangle(new Point(150, 150), new Size(100, 100)), 
            };
            
            var expectedOuterRect = new Rectangle(new Point(100, 100), new Size(150, 150));
            var actualOuterRect = Rectangle.GetOuterRect(rects);

            actualOuterRect.Should().BeEquivalentTo(expectedOuterRect);
        }

        [Test]
        public void GetOuterRect_ShouldBeCorrect_WhenRectsHasNegativeCoordinates()
        {
            var rects = new Rectangle[]
            {
                new Rectangle(new Point(-200, -200), new Size(100, 100)), 
                new Rectangle(new Point(-150, -150), new Size(100, 100)), 
            };
            
            var expectedOuterRect = new Rectangle(new Point(-200, -200), new Size(150, 150));
            var actualOuterRect = Rectangle.GetOuterRect(rects);

            actualOuterRect.Should().BeEquivalentTo(expectedOuterRect);
        }

        [Test]
        public void GetOuterRect_ShouldBeCorrect_WhenRectsHasNegativeAndPositiveCoordinates()
        {
            var rects = new Rectangle[]
            {
                new Rectangle(new Point(-100, -100), new Size(100, 100)), 
                new Rectangle(new Point(0, 0), new Size(100, 100)), 
            };
            
            var expectedOuterRect = new Rectangle(new Point(-100, -100), new Size(200, 200));
            var actualOuterRect = Rectangle.GetOuterRect(rects);

            actualOuterRect.Should().BeEquivalentTo(expectedOuterRect);
        }
    }
}