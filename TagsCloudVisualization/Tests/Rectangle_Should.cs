using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Rectangle_Should
    {
        [Test]
        public void HaveSize_AfterCreate()
        {
            var rectangle = new Rectangle(1,1,1,1);
            rectangle.Size.ShouldBeEquivalentTo(new Size(1,1));
        }
        [Test]
        public void HavePoint_AfterCreate()
        {
            var rectangle = new Rectangle(1, 1, 1, 1);
            rectangle.X.ShouldBeEquivalentTo(new Point(1, 1).X);
            rectangle.Y.ShouldBeEquivalentTo(new Point(1, 1).Y);
        }


    }
}