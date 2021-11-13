using System;
using System.Drawing;
using System.IO;
using System.Linq;
using JetBrains.ReSharper.TestRunner.Abstractions.Extensions;
using TagsCloud.Visualization;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.Drawer;
using TagsCloud.Visualization.FontService;
using TagsCloud.Visualization.LayoutContainer;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.PointGenerator;
using TagsCloud.Visualization.WordsFilter;
using TagsCloud.Visualization.WordsParser;
using TagsCloud.Visualization.WordsSizeService;

namespace TagsCloud.Words
{
    public class Program
    {
        private const int MaxFontSize = 2000;
        private static readonly Point Center = Point.Empty;

        public static void Main(string[] args)
        {
            var words = ParseWordsFromFile(GetInputFilename());

            var maxCount = words.Max(x => x.Count);
            var minCount = words.Min(x => x.Count);

            // TODO replace with di container
            var fontService = new FontService(MaxFontSize, minCount, maxCount);
            var wordsSizeService = new WordsSizeService();
            var archimedesSpiralPointGenerator = new ArchimedesSpiralPointGenerator(Center);
            var layouter = new CircularCloudLayouter(Center, archimedesSpiralPointGenerator);
            var wordsBuilder = new WordsContainerBuilder(layouter, fontService, wordsSizeService);
            var visitor = new RandomColorDrawerVisitor();
            var drawer = new Drawer(visitor);

            words.ForEach(x => wordsBuilder.Add(x));

            var container = wordsBuilder.Build();
            using var image = drawer.Draw(container);
            var path = GetDirectoryForSavingExamples() + "\\words4.png";
            image.Save(path);
        }

        private static Word[] ParseWordsFromFile(string path)
        {
            var text = File.ReadAllText(path);
            return new WordsParser(new WordsFilter())
                .CountWordsFrequency(text)
                .OrderByDescending(x => x.Value)
                .Select(x => new Word {Content = x.Key, Count = x.Value})
                .ToArray();
        }

        private static string GetDirectoryForSavingExamples()
        {
            var solutionPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
            var path = Path.Combine(solutionPath, "Examples");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        private static string GetInputFilename()
        {
            var solutionPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
            var path = Path.Combine(solutionPath, "Examples");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + "\\refactoring.txt";
        }
    }
}