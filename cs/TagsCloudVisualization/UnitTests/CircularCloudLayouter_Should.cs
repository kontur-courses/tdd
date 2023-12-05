using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.UnitTests
{
    class CircularCloudLayouter_Should
    {
        [TestCaseSource(typeof(TestDataCircularCloudLayouter),
            nameof(TestDataCircularCloudLayouter.ZeroOrLessHeightOrWidth_Size))]
        public void Throw_WhenPutNewRectangle_WidthOrHeightLessEqualsZero(Size size)
        {
            var action = new Action(() => new CircularCloudLayouter(new Point()).PutNextRectangle(size));
            action.Should().Throw<ArgumentException>()
                .Which.Message.Should().Contain("zero or negative height or width");
        }

        [Test]
        public void RectanglesEmpty_AfterCreation()
        {
            var layouter = new CircularCloudLayouter(new Point());
            layouter.GetRectangles().Should().BeEmpty();
        }

        [TestCaseSource(typeof(TestDataArchimedeanSpiral), nameof(TestDataArchimedeanSpiral.Different_CenterPoints))]
        public void Add_FirstRectangle_ToCenter(Point center)
        {
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(new Size(10, 2));
            layouter.GetRectangles().Should().HaveCount(1)
                .And.BeEquivalentTo(new Rectangle(center, new Size(10, 2)));
        }

        [Test]
        public void AddSeveralRectangles_Correctly()
        {
            var layouter = new CircularCloudLayouter(new Point());
            for (var i = 1; i < 26; i++)
            {
                layouter.PutNextRectangle(new Size(i * 20, i * 10));
            }

            layouter.GetRectangles().Should().HaveCount(25).And.AllBeOfType(typeof(Rectangle));
        }
        
        [TestCaseSource(typeof(TestDataArchimedeanSpiral), nameof(TestDataArchimedeanSpiral.Different_CenterPoints))]
        public void AddSeveralRectangles_DoNotIntersect(Point point)
        {
            var layouter = new CircularCloudLayouter(point);
            for (var i = 1; i < 26; i++)
            {
                layouter.PutNextRectangle(new Size(i * 20, i * 10));
            }

            var rectangles = layouter.GetRectangles();
            for (var i = 1; i < rectangles.Count; i++)
                rectangles.Skip(i).All(x => !rectangles[i - 1].IntersectsWith(x)).Should().Be(true);
        }
    }
}
