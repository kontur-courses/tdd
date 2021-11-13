using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TagsCloud.Visualization;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.Drawer;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.FontFactory;
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
        public static void Main(string[] args)
        {
            var serviceProvider = CreateServiceProvider(Point.Empty);

            var parsedWords = ParseWordsFromFile(serviceProvider, GetInputFilename());

            var maxCount = parsedWords.Max(x => x.Count);
            var minCount = parsedWords.Min(x => x.Count);

            var wordsBuilder = serviceProvider.GetRequiredService<WordsContainerBuilder>();
            parsedWords.ForEach(x => wordsBuilder.Add(x, minCount, maxCount));

            using var container = wordsBuilder.Build();

            var drawer = serviceProvider.GetRequiredService<IDrawer>();
            using var image = drawer.Draw(container);
            var path = GetDirectoryForSavingExamples() + "\\words4.png";
            image.Save(path);
        }

        private static Word[] ParseWordsFromFile(IServiceProvider serviceProvider, string path)
        {
            var text = File.ReadAllText(path);
            return serviceProvider.GetRequiredService<IWordsParser>()
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
            if (!Directory.Exists(solutionPath))
                Directory.CreateDirectory(solutionPath);
            return solutionPath + "\\refactoring.txt";
        }

        private static IServiceProvider CreateServiceProvider(Point center)
        {
            var collection = new ServiceCollection();

            collection.AddScoped<IWordsFilter, WordsFilter>();
            collection.AddScoped<IWordsParser, WordsParser>();

            collection.AddScoped<FontFactory>();
            collection.AddScoped<IWordsSizeService, WordsSizeService>();
            collection.AddScoped<WordsContainerBuilder>();

            collection.AddScoped<IPointGenerator>(_ => new ArchimedesSpiralPointGenerator(center));
            collection.AddScoped<ICloudLayouter>(provider
                => new CircularCloudLayouter(center, provider.GetService<IPointGenerator>()));

            collection.AddScoped<IContainerVisitor, RandomColorDrawerVisitor>();
            collection.AddScoped<IDrawer, Drawer>();

            return collection.BuildServiceProvider();
        }
    }
}