using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Curves;
using TagsCloudVisualization.Savers;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _layouter;
        [SetUp]
        public void SetUp()
        {
            ICurve curve = new ArchimedeanSpiral();
            _layouter = new CircularCloudLayouter(curve);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                CircularCloudDrawer visualisator = new CircularCloudDrawer(_layouter);
                Bitmap image = visualisator.CreateImage();
                IBitmapSaver saver = new HardDriveSaver();
                saver.Save(image, TestContext.CurrentContext.Test.Name);
            }
        }

        private void PutAllRectangles(IReadOnlyList<Size> rectangles)
        {
            foreach (Size rectangleSize in rectangles)
                _layouter.PutNextRectangle(rectangleSize);
        }

        private Size GetBoundsSize()
        {
            int leftBound = _layouter.Rectangles.Min(rectangle => rectangle.Left);
            int rightBound = _layouter.Rectangles.Max(rectangle => rectangle.Right);
            int bottomBound = _layouter.Rectangles.Max(rectangle => rectangle.Bottom);
            int topBound = _layouter.Rectangles.Min(rectangle => rectangle.Top);
            
            int width = rightBound - leftBound;
            int height = bottomBound - topBound;

            return new Size(width, height);
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
            PutAllRectangles(sizesOfRectangles);
            
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
        
        private static IEnumerable<TestCaseData> RectanglesWithMaxArea
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
                    }, 75000).SetName("5 rectangles with size (100x100) and max area = 75000");
                yield return new TestCaseData(
                    new List<Size>()
                    {
                        new Size(500, 500),
                        new Size(400, 400),
                        new Size(300, 300),
                        new Size(200, 200),
                        new Size(100, 100),
                    }, 750000).SetName("5 rectangles with size (100*i x 100*i) and max area = 750000");
            }
        }

        [TestCaseSource(nameof(RectanglesWithMaxArea))]
        public void PutNextRectangle_Area_ShouldBeLessThan(IReadOnlyList<Size> sizesOfRectangles, double maxArea)
        {
            PutAllRectangles(sizesOfRectangles);
            Size boundsSize = GetBoundsSize();
            double area = boundsSize.Height * boundsSize.Width;
            Console.WriteLine(area);
            area.Should().BeLessOrEqualTo(maxArea);
        }
    }
}