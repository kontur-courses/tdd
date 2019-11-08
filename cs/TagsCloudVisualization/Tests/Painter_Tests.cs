using System;
using System.Drawing;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Painter_tests
    {
        [Test]
        public void PainterCtor_ValidSize_ShouldNotThrowException()
        {
            Action act = () => new Painter(new Size(100, 100));
            act.Should().NotThrow();
        }
    }
}