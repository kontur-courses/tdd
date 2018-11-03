using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        private Point startPoint;
        private ICircularCloudLayouter circularCloudLayouter;
        private ICollection<Rectangle> rectangles;

        [SetUp]
        public void DoBeforeAnyTest()
        {
            startPoint = new Point();
            circularCloudLayouter = new CircularCloudLayouter(startPoint);
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void DoAfterAnyTest()
        {
            if (TestContext.CurrentContext.Result.FailCount != 0)
            {
                var savePath = TestContext.CurrentContext.TestDirectory
                    + $"\\test_failed_{TestContext.CurrentContext.Test.Name}.bmp";

                new Visualizator(new Size(5000, 5000), rectangles)
                    .Generate()
                    .Save(savePath);
            }
        }

        [Test]
        public void PutNextRectangle_ReturnsSameSizeRectangleAtStartPoint_OnFirstRectangle()
        {
            var size = new Size(100, 100);
            var expected = new Rectangle(startPoint, size);

            var result = circularCloudLayouter.PutNextRectangle(size);

            result
                .Equals(expected)
                .Should()
                .BeTrue();
        }

        [Test]
        public void PutNextRectangle_ReturnsNotIntersectedRectangle_OnCorrectSizes()
        {
            var random = new Random();

            for (var i = 0; i < 100; i++)
            {
                var size = new Size(random.Next(1, 100), random.Next(1, 100));
                var rectangle = circularCloudLayouter.PutNextRectangle(size);

                rectangle.IntersectsWith(rectangles)
                    .Should()
                    .BeFalse();

                rectangles.Add(rectangle);
            }
        }

        #region InvalidSizes

        public static IEnumerable<Size> OnInvalidSizes()
        {
            yield return new Size(0, 0);
            yield return new Size(0, 1);
            yield return new Size(1, 0);
            yield return new Size(-1, 1);
            yield return new Size(1, -1);
            yield return new Size(-1, -1);
        }

        #endregion

        [TestCaseSource(nameof(OnInvalidSizes))]
        public void PutNextRectangle_ThrowsException_OnInvalidSize(Size size)
        {
            Action action = () => circularCloudLayouter.PutNextRectangle(size);

            action
                .Should()
                .Throw<ArgumentException>();
        }
    }
}