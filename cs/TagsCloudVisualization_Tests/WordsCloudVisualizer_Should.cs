using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class WordsCloudVisualizer_Should
    {
        [Test]
        public void DrawRectangles_BeCorrectSize()
        {
            var layout = new WordsCloudLayouter(new Point(0, 0));
            var visualizer = new WordsCloudVisualizer();
            var words = new List<Word>();
            words.Add(layout.PutNextWord("letter", new Font(FontFamily.GenericSansSerif, 14)));
            words.Add(layout.PutNextWord("letter", new Font(FontFamily.GenericSansSerif, 14)));
            visualizer.DrawCloud(words, layout.Radius).Width.Should().Be(layout.Radius * 2);
            visualizer.DrawCloud(words, layout.Radius).Height.Should().Be(layout.Radius * 2);
        }
    }
}