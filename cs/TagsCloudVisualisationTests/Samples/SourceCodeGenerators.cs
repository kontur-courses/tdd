using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualisation;
using TagsCloudVisualisation.Extensions;
using TagsCloudVisualisation.Visualisation.TextVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests.Samples
{
    [Explicit]
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class SourceCodeGenerators : LayouterTestBase
    {
        private static readonly Font fontPrototype = SystemFonts.CaptionFont;
        private WordsLayouterAsset asset;

        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0));
            asset = new WordsLayouterAsset(Layouter, 2);
        }

        [Test]
        public void SourceCodeTermsFrequencies()
        {
            var baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var sourceDirectory = GoParentUntil(baseDirectory,
                di => di.EnumerateFiles("*.csproj", SearchOption.TopDirectoryOnly).Any()).Parent;

            var sourceCodeFiles = sourceDirectory!.EnumerateFiles("*.cs", SearchOption.AllDirectories)
                .Select(ReadFile)
                .ToArray();
            PrintTestingMessage($"Successfully read {sourceCodeFiles.Length} .cs files");

            var tcg = new TextCloudGenerator(new SourceCodeTextAnalyzer(), asset, true,
                fontPrototype, () => new SolidBrush(TestingHelpers.RandomColor));
            foreach (var scf in sourceCodeFiles)
                tcg.RegisterText(scf);

            tcg.GenerateCloud(Enumerable.Range(1, 10).Select(i => i * 2).ToArray());
        }

        private string ReadFile(FileInfo fileInfo)
        {
            using var streamReader = new StreamReader(fileInfo.OpenRead());
            return streamReader.ReadToEnd();
        }

        private DirectoryInfo GoParentUntil(DirectoryInfo baseDirectory, Func<DirectoryInfo, bool> predicate)
        {
            while (!predicate.Invoke(baseDirectory))
                baseDirectory = baseDirectory.Parent ?? throw new InvalidOperationException($"No directories left");
            return baseDirectory;
        }

        protected override Image RenderResultImage(IList<Rectangle> resultRectangles)
        {
            asset.DrawQueuedWords();
            return asset.GetImage().FillBackground(Color.FromArgb(46, 46, 46));
        }
    }
}