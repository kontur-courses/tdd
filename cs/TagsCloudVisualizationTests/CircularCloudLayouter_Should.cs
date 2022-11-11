
using System.Drawing;
using TagsCloudVisualization;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter _ccl;
        private List<Rectangle> _rectangles;

        [SetUp]
        public void SetUp()
        {
            Point center = new Point(1500, 1500);
            _rectangles = new List<Rectangle>();
            _ccl = new CircularCloudLayouter(center, 0.1, 1, 0);
        }
        
        [TearDown]
        public void WhenTestFailed_DrawActual()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var counter = 1;
                
                while (true)
                {
                    var path = "FailedTests\\" + TestContext.CurrentContext.Test.Name;
                    Directory.CreateDirectory(path);
                    var filename =  path + "\\Attempt" + counter + ".png";
                    if (!File.Exists(filename))
                    {
                        LayoutDrawer layoutDrawer = new LayoutDrawer(3000, 3000, Color.Black, 2);
                        layoutDrawer.DrawLayout(_rectangles, filename);
                        break;
                    }

                    counter++;
                }
            }

            _rectangles = new List<Rectangle>();
        }

        [Test]
        public void OnOneRectangle_ReturnsRectangleWithHisCenterAsLocation()
        {
            Size size = new Size(50, 100);
            var actual = _ccl.PutNextRectangle(size);
            actual.Should().Be(new Rectangle(new Point(1475, 1450), size));
            _rectangles.Add(actual);
        }
        
        [TestCase(-10, 20)]
        [TestCase(20, -10)]
        [TestCase(0, 0)]
        [TestCase(-10, -20)]
        public void OnNonPositiveWidthAndHeight_ThrowsArgumentException(int width, int height)
        {
            Size size = new Size(width, height);
            Action act = () => _ccl.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>().WithMessage("Width and Height must be positive!");
        }

        [Test]
        public void OnFewRectangleSizes_ReturnsRectanglesWithRightLocation()
        {
            _rectangles = new List<Rectangle>();
            Size s1, s2, s3, s4;
            s1 = new Size(20, 30);
            s2 = new Size(20, 30);
            s3 = new Size(50, 20);
            s4 = new Size(10, 20);
            _rectangles.Add(_ccl.PutNextRectangle(s1));
            _rectangles.Add(_ccl.PutNextRectangle(s2));
            _rectangles.Add(_ccl.PutNextRectangle(s3));
            _rectangles.Add(_ccl.PutNextRectangle(s4));

            var expected = new List<Rectangle>();
            expected.Add(new Rectangle(new Point(1490, 1485), s1));
            expected.Add(new Rectangle(new Point(1470, 1493), s2));
            expected.Add(new Rectangle(new Point(1460, 1465), s3));
            expected.Add(new Rectangle(new Point(1511, 1480), s4));
            
            _rectangles.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenNoPlaceForRectangle_ThrowsException()
        {
            _ccl.PutNextRectangle(new Size(1_000_000_000, 1_000_000_000));
            Action act = () => _ccl.PutNextRectangle(new Size(100, 100));
            act.Should().Throw<ArgumentException>();
        }   
    }
}

