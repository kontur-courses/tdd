using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Fclp;
using TagsCloudVisualization.Visualizer;
using TagsCloudVisualization.WordsAnalyzer;

namespace TagsCloudVisualization
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var cmdParser = CreateCmdParser();
            if (cmdParser.Parse(args).HelpCalled)
            {
                return;
            }
            if (cmdParser.Object.PathToImage == null)
            {
                cmdParser.HelpOption.ShowHelp(cmdParser.Options);
                return;
            }
            try
            {
                Run(cmdParser.Object);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private static void Run(RunOptions options)
        {
            Console.InputEncoding = Encoding.UTF8;
            var weightedWords = new WordsCounter()
                .Сonsider(ReadPhrases(Console.In))
                .OrderByDescending(w => w.Weight)
                .ToList();
            var center = GetCenter(options.Width, options.Height);
            var cloud = new CircularCloudLayouter(center);
            var tagLayouter = new TagLayouter(cloud, options.MinFont, options.MaxFont);
            var tags = tagLayouter.GetTags(weightedWords).ToList();
            var visualizer = new TagsCloudVisualizer(new SolidBrush(Color.DarkSlateBlue), Color.Bisque);
            using (var image = visualizer.GetCloudImage(tags, options.Width, options.Height))
            {
                var fileName = options.PathToImage;
                SaveImage(image, fileName);
            }
        }

        private static Point GetCenter(int width, int height)
        {
            return new Point(width / 2, height / 2);
        }

        public static void SaveImage(Bitmap image, string fileName)
        {  
            try
            {
                image.Save(fileName);
            }
            catch (Exception)
            {
                Console.WriteLine($"error saving image {fileName}");
            }
        }

        public static string[] ReadPhrases(TextReader textReader)
        {
            return textReader.ReadToEnd().Split('\n').Select(s => s.Trim()).ToArray();
        }

        static FluentCommandLineParser<RunOptions> CreateCmdParser()
        {
            var cmdParser = new FluentCommandLineParser<RunOptions>();
            cmdParser
                .Setup(options => options.MinFont)
                .As('l')
                .SetDefault(30)
                .WithDescription("minimum font size");
            cmdParser
                .Setup(options => options.MaxFont)
                .As('r')
                .SetDefault(60)
                .WithDescription("maximum font size");
            cmdParser
                .Setup(options => options.Width)
                .As('w')
                .SetDefault(1500)
                .WithDescription("width of image");
            cmdParser
                .Setup(options => options.Height)
                .As('h')
                .SetDefault(1500)
                .WithDescription("height of image");
            cmdParser
                .Setup(options => options.PathToImage)
                .As('i')
                .WithDescription("path to final image");
            cmdParser
                .SetupHelp("help")
                .WithHeader($"{AppDomain.CurrentDomain.FriendlyName} [-i image] [-w width] [-h height]\n" +
                            " A list of words is passed on standard input. " +
                            "Each phrase should be written on a new line.")
                .Callback(text => Console.WriteLine(text));
            return cmdParser;
        }

        private class RunOptions
        { 
            public int Width { get; set; }
            public int Height { get; set; }
            public int MinFont { get; set; }
            public int MaxFont { get; set; }
            public string PathToImage { get; set; }
        }
    }
}
