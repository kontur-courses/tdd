using System;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualisation.Extensions;
using TagsCloudVisualisation.Visualisation.TextVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests.Samples
{
    [Explicit]
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public abstract class BaseSampleGenerator : LayouterTestBase
    {
        private const string ReadmeFileName = "README.md";
        private const string ReadmeResourcesFolderName = "readme-resources";
        private Image renderedImage;

        protected WordsLayouterAsset WordsLayouterAsset;
        protected Color BackgroundColor;

        public override void SetUp()
        {
            renderedImage = null;
            BackgroundColor = Color.FromArgb(46, 46, 46);
            base.SetUp();
        }

        public override void TearDown()
        {
            base.TearDown();
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                using (new Measurement($"Save to {ReadmeResourcesFolderName}", TestContext.Progress))
                    SaveReadmeImage(RenderResultImage());
            }
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            using (new Measurement("Create README.md", TestContext.Progress))
                CreateReadme();
        }

        protected sealed override Image RenderResultImage()
        {
            if (renderedImage == null)
            {
                WordsLayouterAsset.DrawQueuedWords();
                renderedImage = WordsLayouterAsset.GetImage().FillBackground(BackgroundColor);
            }

            return renderedImage;
        }

        private static void CreateReadme()
        {
            var readmeBaseDirectory = GetReadmeBaseDirectory();
            var markdownContent = string.Join(Environment.NewLine + Environment.NewLine,
                GetReadmeResourcesDirectory()
                    .EnumerateFiles("*.png")
                    .Select(x => MarkdownElements.Image($@"{ReadmeResourcesFolderName}/{x.Name}"))
                    .OrderBy(x => x));

            var readmeFile = readmeBaseDirectory.GetFiles(ReadmeFileName).Single();
            using var writer = new StreamWriter(readmeFile.Open(FileMode.Create));
            writer.Write(markdownContent);
        }

        private static void SaveReadmeImage(Image image) => image.Save(Path.Combine(
            GetReadmeResourcesDirectory().FullName,
            $"{TestContext.CurrentContext.Test.Name}.png"));

        private static DirectoryInfo GetReadmeResourcesDirectory() => GetReadmeBaseDirectory()
            .CreateSubdirectory(ReadmeResourcesFolderName);

        private static DirectoryInfo GetReadmeBaseDirectory() =>
            new DirectoryInfo(TestContext.CurrentContext.WorkDirectory)
                .GoParentUntil(di => di.EnumerateFiles(ReadmeFileName, SearchOption.TopDirectoryOnly).Any());
    }
}