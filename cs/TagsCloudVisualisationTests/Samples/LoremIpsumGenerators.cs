using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualisation;
using TagsCloudVisualisation.Visualisation.TextVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests.Samples
{
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class LoremIpsumGenerators : BaseSampleGenerator
    {
        private static readonly char[] separators = {',', '.', ' ', '!', '?', '\n', '\r', '\t'};
        private static readonly Font fontPrototype = SystemFonts.CaptionFont;
        private const string LoremIpsumFileName = "Lorem Ipsum.txt";

        private TextCloudGenerator textCloudGenerator;

        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0));
            WordsLayouterAsset = new WordsLayouterAsset(Layouter, 1);

            textCloudGenerator = new TextCloudGenerator(
                new SimpleTextAnalyzer(separators),
                WordsLayouterAsset,
                true,
                fontPrototype,
                () => new SolidBrush(TestingHelpers.RandomColor));
        }

        [Test]
        public void LoremIpsumFrequencies()
        {
            textCloudGenerator.RegisterText(GetLoremIpsum());
            textCloudGenerator.GenerateCloud(Enumerable.Range(1, 10).Select(i => i * 2).ToArray());
        }

        private static string GetLoremIpsum() =>
            File.ReadAllText(TestingHelpers.GetTestFile(LoremIpsumFileName).FullName);
    }
}