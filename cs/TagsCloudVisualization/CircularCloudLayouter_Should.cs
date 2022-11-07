using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Point center = new Point(200, 100);
        
        [Test]
        public void PutNextRectangle_ShouldReturnFirstRectangle_ThatContainsCenter()
        {
            var circularCloudLayouter = new CircularCloudLayouter(center);
            rectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(10, 5)));
            rectangles[0].Contains(center).Should().Be(true);
        }
        
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(40)]
        [TestCase(100)]
        public void PutNextRectangle_ShouldNotHas_IntersectRectangles(int count)
        {
            var size = new Size(20, 5);
            var circularCloudLayouter = new CircularCloudLayouter(center);
            rectangles = Enumerable.Range(0, count)
                .Select(item => circularCloudLayouter.PutNextRectangle(size))
                .ToList();
            for (var i = 0; i < count; i++)
            {
                var rect = rectangles[i];
                for (var j = 0; j < count; j++)
                {
                    if (i != j)
                        rect.IntersectsWith(rectangles[j]).Should().Be(false);
                }
            }
        }

        [TearDown]
        public void SaveImage_OnTearDown()
        {
            var filename = $"Failed test {TestContext.CurrentContext.Test.Name} image at {DateTime.Now:dd-MM-yyyy HH_mm_ss}.jpg";
            var bitmap = TagCloudDrawer.Draw(rectangles.ToArray(),
                400, 200, center,
                Color.Black, Color.DarkOrange,
                true, true);
            bitmap.Save(filename);
            var path = AppContext.BaseDirectory;
            TestContext.Error.Write($"Tag cloud visualization saved to file {path + filename}");
        }
    }
}