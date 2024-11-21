using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagsCloudVisualization.CloudClasses;
using TagsCloudVisualization.Visualisation;

namespace TagsCloudVisualization.Tests
{
    public class CircularCloudLayouterTests
    {
        private const int WIDTH = 1000;

        private const int HEIGHT = 1000;

        private CircularCloudLayouter _layouter;

        private Point _center;

        private IVisualiser _visualiser;

        private TagCloudImageGenerator _tagCloudImageGenerator;

        private Size _errorImageSize;

        [SetUp]
        public void Setup()
        {
            _center = new Point(WIDTH / 2, HEIGHT / 2);

            var mover = new SpiralRayMover(_center);

            _layouter = new CircularCloudLayouter(mover);

            _visualiser = new Visualiser(Color.Black, 1);
            
            _tagCloudImageGenerator = new TagCloudImageGenerator(_visualiser);

            _errorImageSize = new Size(WIDTH, HEIGHT);
        }

       [TearDown]
        public void Teardown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                using var errorBitmap = _tagCloudImageGenerator.CreateNewBitmap(_errorImageSize, _layouter.Rectangles);

                var fileName = $"{TestContext.CurrentContext.Test.MethodName + Guid.NewGuid()}.png";
                BitmapSaver.SaveToFail(errorBitmap, fileName);
            }

            _visualiser.Dispose();
            _tagCloudImageGenerator.Dispose();
        }

        [Test]
        public void CircularCloudLayouter_WhenCreated_RectanglesShoudBeEmpty()
        {
            _layouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        public void CircularCloudLayouter_WhenCreated_FirstPointEqualsCenter()
        {
            var firstPoint = _layouter.RayMover.MoveRay().First();

            firstPoint.Should().BeEquivalentTo(_center);
        }

        [Test]
        public void CircularCloudLayouter_WhenAddFirstRectangle_ContainOneAndSameRectangle()
        {
            var rectangle = _layouter.PutNextRectangle(new Size(20, 10));

            _layouter.Rectangles
                .Select(r => r).Should().HaveCount(1).And.Contain(rectangle);
        }

        [Test]
        public void CircularCloudLayouter_WhenAddRectangle_ReturnsRectangleWithSameSize()
        {
            var givenSize = new Size(20, 20);

            var rectangle = _layouter.PutNextRectangle(givenSize);

            rectangle.Size.Should().BeEquivalentTo(givenSize);
        }

        [TestCase(0, 0)]
        [TestCase(-2, 2)]
        [TestCase(2, -2)]
        [TestCase(-2, -2)]
        public void CircularCloudLayouter_WhenWrongSize_ThrowArgumentException(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => _layouter.PutNextRectangle(new Size(width, height)),
                "Not valid size should be positive");
        }

        [Test]
        public void CircularCloudLayouter_WhenAddFew_ShouldHaveSameCount()
        {
            var sizesList = GenerateSizes(5);
            AddRectanglesToLayouter(sizesList);

            _layouter.Rectangles.Should().HaveCount(5);
        }

        [TestCase(1, 2, 3)]
        public void CircularCloudLayouter_ShouldIncreaseRectangleCountCorrectly(int countBefore, int add, int countAfter)
        {
            var countBeforeSizes = GenerateSizes(countBefore);
            AddRectanglesToLayouter(countBeforeSizes);

            var addSizes = GenerateSizes(add);
            AddRectanglesToLayouter(addSizes);

            _layouter.Rectangles.Should().HaveCount(countAfter);
        }

        [TestCase(2)]
        [TestCase(30)]
        public void CircularCloudLayouter_WhenAddFew_RectangleNotIntersectsWithOtherRectangles(int count)
        {
            var listSizes = GenerateSizes(count);
            AddRectanglesToLayouter(listSizes);

            foreach (var rectangle in _layouter.Rectangles)
            {
                _layouter.Rectangles
                    .Any(r => r.IntersectsWith(rectangle) && rectangle != r)
                    .Should().BeFalse();
            }
        }

        private List<Size> GenerateSizes(int count)
        {
            return SizeBuilder.Configure()
                .SetCount(count)
                .SetWidth(60, 80)
                .SetHeight(60, 80)
                .Generate()
                .ToList();
        }

        private void AddRectanglesToLayouter(List<Size> sizes)
        {
            sizes.ForEach(size => _layouter.PutNextRectangle(size));
        }
    }
}