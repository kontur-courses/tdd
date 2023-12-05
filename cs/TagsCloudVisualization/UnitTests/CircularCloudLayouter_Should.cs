using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.UnitTests
{
    class CircularCloudLayouter_Should
    {
        [Test]
        public void Create_Without_Exceptions()
        {
            var action = new Action(() => new CircularCloudLayouter(new Point()));
            action.Should().NotThrow();
        }

        [TestCaseSource(typeof(TestDataCircularCloudLayouter), nameof(TestDataCircularCloudLayouter.ZeroOrLessHeightOrWidth_Size))]
        public void Throw_WhenPutNewRectangle_WidthOrHeightLessEqualsZero(Size size)
        {
            var action = new Action(() => new CircularCloudLayouter(new Point()).PutNextRectangle(size));
            action.Should().Throw<ArgumentException>()
                .Which.Message.Should().Contain("zero or negative height or width");
        }
    }
}
