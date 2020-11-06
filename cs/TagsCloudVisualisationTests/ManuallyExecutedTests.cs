using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
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
            asset = new WordsLayouterAsset(Layouter);
        }

        [Test]
        public void VisualiseLoremIpsum()
        {
            DrawWords(GetLoremIpsum());
        }

        private void DrawWords(params string[] words)
        {
            foreach (var word in words)
            {
                var wordToDraw = new WordToDraw(word, SystemFonts.CaptionFont, 
                    new SolidBrush(TestingHelpers.RandomColor));
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