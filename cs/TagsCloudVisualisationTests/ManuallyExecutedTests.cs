using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualisation;
using TagsCloudVisualisation.Visualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests
{
    [Explicit]
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class ManuallyExecutedTests : LayouterTestBase
    {
        private WordsLayouterAsset asset;

        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0));
            asset = new WordsLayouterAsset(Layouter, 2);
        }

        [Test]
        public void VisualiseLoremIpsum()
        {
            DrawWords(GetLoremIpsum());
        }

        private void DrawWords(params string[] words)
        {
            var randomizer = new Randomizer();
            var fonts = Enumerable.Range(1, 4)
                .Select(i => WordToDraw.MultiplyFontSize(SystemFonts.CaptionFont, i * i))
                .ToArray();
            foreach (var word in words)
            {
                var font = fonts[randomizer.Next(0, fonts.Length)];
                var wordToDraw = new WordToDraw(word, font, new SolidBrush(TestingHelpers.RandomColor));
                asset.DrawWord(wordToDraw);
            }
        }

        protected override Image RenderResultImage(IList<Rectangle> resultRectangles) => asset.GetImage();

        private static string[] GetLoremIpsum()
        {
            return File.ReadAllLines(TestingHelpers.GetTestFile("Lorem Ipsum.txt").FullName)
                .SelectMany(x => x.Split(',', '.', ' ', '?', '!'))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();
        }
    }
}