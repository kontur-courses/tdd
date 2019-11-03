using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    class RectangleExtension_should
    {
        [Test]
        public void GetCenter_WhenRectangleIsPoint_ReturnThisPoint()
        {
            var point = new Point(5,5);
            var rectangle = new Rectangle(point, new Size(0, 0));
            rectangle.GetCenter().Should().Be(point);
        }

        [Test]
        public void GetCenter_WhenRectangle_ReturnCenter()
        {
            var point = new Point(4, 4);
            var rectangle = new Rectangle(point, new Size(10, 10));
            rectangle.GetCenter().Should().Be(new Point(9, 9));
        }

        [Test]
        public void DoSomething_WhenSomething()
        {
            var point = new Point(4, 4);
            var rectangle = new Rectangle(point, new Size(0, 10));
            rectangle.GetCenter().Should().Be(new Point(4, 9));
        }
    }
}
