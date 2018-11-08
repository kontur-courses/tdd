using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class WordsCloudLayouter_Should
    {
        private Point center;
        private WordsCloudLayouter cloudLayouter;
        private Font defaultFont;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            cloudLayouter = new WordsCloudLayouter(center);
            defaultFont = new Font(FontFamily.GenericSansSerif, 14);
        }

        [Test]
        public void Radius_BeZeroAfterCreation() => cloudLayouter.Radius.Should().Be(0);


        [Test]
        public void PutNextWord_ThrowArgumentException_OnEmptyString() =>
            cloudLayouter
                .Invoking(obj => obj.PutNextWord("", defaultFont))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("*?grater than zero");
    }
}