using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _layouter;
        [SetUp]
        public void SetUp()
        {
            _layouter = new CircularCloudLayouter(new Point(500, 500));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                CircularCloudLayouterVisualisator visualisator =
                    new CircularCloudLayouterVisualisator(_layouter);
                Bitmap image = visualisator.CreateImage();
                visualisator.Save(image, TestContext.CurrentContext.Test.Name);
            }
        }

        private static IEnumerable<TestCaseData> SizesOfRectangles
        {
            get
            {
                yield return new TestCaseData(
                    new List<Size>()
                    {
                        new Size(100, 100),
                        new Size(100, 100),
                        new Size(100, 100),
                        new Size(100, 100),
                        new Size(100, 100),
                    });
                yield return new TestCaseData(
                    new List<Size>()
                    {
                        new Size(100, 100),
                        new Size(200, 100),
                        new Size(100, 200),
                        new Size(1000, 1000),
                        new Size(100, 100),
                    });
            }
        }

        [TestCaseSource(nameof(SizesOfRectangles))]
        public void PutNextRectangle_ShouldNotMakeIntersections(IReadOnlyList<Size> sizesOfRectangles)
        {
            foreach (Size rectangleSize in sizesOfRectangles)
            {
                _layouter.PutNextRectangle(rectangleSize);
                foreach (Rectangle rectangle1 in _layouter.Rectangles)
                {
                    foreach (Rectangle rectangle2 in _layouter.Rectangles)
                    {
                        if (rectangle1 == rectangle2)
                            continue;
                        rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
                    }
                }
            }
        }
    }
}