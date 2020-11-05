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
        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void TestingData()
        {
            Layouter.Put(new Size(10, 10), out _)
                .Put(new Size(6, 5), out _)
                .Put(new Size(4, 5), out _)
                .Put(new Size(4, 4), out _)
                .Put(new Size(7, 5), out _)
                .Put(new Size(29, 10), out _)
                .Put(new Size(10, 8), out _)
                .Put(new Size(10, 20), out _)
                .Put(new Size(4, 5), out _)
                .Put(new Size(4, 4), out _)
                .Put(new Size(7, 5), out _)
                .Put(new Size(29, 10), out _)
                .Put(new Size(5, 6), out _);
        }

        [Test]
        public void VisualiseLoremIpsum()
        {
            var image = DrawWords(GetLoremIpsum());
            SaveImage(image, "Words-visualisation.png");
        }

        private void SaveImage(Image image, string filename)
        {
            var fullPath = Path.Combine(TestingHelpers.GetOutputDirectory("manual-saved"), filename);
            image.Save(fullPath);
            TestContext.Out.WriteLine($"Saved to {fullPath}");
        }

        private Image DrawWords(params string[] words)
        {
            var asset = new WordsLayouterAsset(new CircularCloudLayouter(new Point(0, 0)));
            foreach (var word in words)
                asset.DrawWord(new WordToDraw(word, SystemFonts.CaptionFont,
                    new SolidBrush(TestingHelpers.RandomColor)));
            return asset.GetImage();
        }

        private static string[] GetLoremIpsum()
        {
            return File.ReadAllLines(TestingHelpers.GetTestFile("Lorem Ipsum.txt").FullName)
                .SelectMany(x => x.Split(',', '.', ' ', '?', '!'))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();
        }
    }
}