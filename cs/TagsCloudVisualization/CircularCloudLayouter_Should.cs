using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(1000, 1000));
        }
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = TestContext.CurrentContext.TestDirectory;
                var fileName = $"{TestContext.CurrentContext.Test.Name} failed on {DateTime.Now}.png";
                var outputFileName = Path.Combine(path, fileName);
                var bitmap = new Bitmap(layouter.Size.Width, layouter.Size.Height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    foreach (var rectangle in layouter.Rectangles)
                    {
                        graphics.DrawRectangle(new Pen(Color.Red), rectangle);
                    }
                    bitmap.Save(outputFileName);
                }
                TestContext.WriteLine($"Tag cloud visualization saved to file {outputFileName}");
            }
        }
        [Test]
        public void HaveSizeTwiceAsMuchCenterPoint_OnInitialization()
        {
            layouter.Size.Should().Be(new Size(2000, 2000));
        }

        [Test]
        public void HaveEmptyRectanglesCollection_OnInitialization()
        {
            layouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        public void PutNextRectangleWithoutException_OnEmptySize()
        {
            Action action = () => layouter.PutNextRectangle(new Size());
            action.Should().NotThrow<Exception>();
        }

        [Test]
        public void CreateFirstRectangle_InCenter()
        {
            var rectangle = layouter.PutNextRectangle(new Size(100, 50));
            rectangle.Location.Should().Be(layouter.Center);
        }

        [Test]
        public void AddSeveralRectangles()
        {
            for (int i = 0; i < 1000; i++)
            {
                layouter.PutNextRectangle(new Size());
            }
            layouter.Rectangles.Count.Should().Be(1000);
        }

        [Test]
        public void AddRectanglesWithoutIntersections()
        {
            for (int i = 0; i < 1000; i++)
            {
                layouter.PutNextRectangle(new Size());
            }

            foreach (var rectangle in layouter.Rectangles)
            {
                layouter.Rectangles
                    .All(e => e.IntersectsWith(rectangle)).Should().BeFalse();
            }
        }
    }
}
