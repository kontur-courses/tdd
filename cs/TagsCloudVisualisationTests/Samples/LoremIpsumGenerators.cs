using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualisation;
using TagsCloudVisualisation.Extensions;
using TagsCloudVisualisation.Visualisation.TextVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests.Samples
{
    [Explicit]
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class LoremIpsumGenerators : LayouterTestBase
    {
        private static readonly char[] separators =
            {',', '.', ' ', '!', '?', '\n', '\r', '\t', ';', '{', '}', '(', ')', '-', '\\', '[', ']'};

        private static readonly Font fontPrototype = SystemFonts.CaptionFont;
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
            DrawWords(GetLoremIpsumSplit());
        }

        [Test]
        public void LoremIpsumFrequencies()
        {
            var tcg = new TextCloudGenerator(new SimpleTextAnalyzer(separators), asset, true,
                fontPrototype, () => new SolidBrush(TestingHelpers.RandomColor));
            tcg.RegisterText(GetLoremIpsum());
            tcg.GenerateCloud(Enumerable.Range(1, 10).Select(i => i * 2).ToArray());
        }

        private void DrawWords(params string[] words)
        {
            var randomizer = new Randomizer();
            var fonts = Enumerable.Range(1, 4)
                .Select(i => WordToDraw.MultiplyFontSize(fontPrototype, i * i))
                .ToArray();
            foreach (var word in words)
            {
                var font = fonts[randomizer.Next(0, fonts.Length)];
                var wordToDraw = new WordToDraw(word, font, new SolidBrush(TestingHelpers.RandomColor));
                asset.EnqueueWord(wordToDraw);
            }
        }

        protected override Image RenderResultImage(IList<Rectangle> resultRectangles)
        {
            asset.DrawQueuedWords();
            return asset.GetImage().FillBackground(Color.FromArgb(46, 46, 46));
        }

        private static string[] GetLoremIpsumSplit()
        {
            return File.ReadAllLines(TestingHelpers.GetTestFile("Lorem Ipsum.txt").FullName)
                .SelectMany(x => x.Split(',', '.', ' ', '?', '!'))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();
        }

        private static string GetLoremIpsum() =>
            File.ReadAllText(TestingHelpers.GetTestFile("Lorem Ipsum.txt").FullName);
    }
}