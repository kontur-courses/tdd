using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Console = System.Console;
using Fclp;

namespace TagsCloudVisualization
{
    class TagCloud
    {
        static void Main(string[] args)
        {
            var configuration = ParseArguments(args);

            var tagCloudCenter = new Point(configuration.CenterX, configuration.CenterY);

            var tags = TagsGenerator.GenerateRectanglesSizes(configuration.TagsCount);
            var circularCloudLayout = GetCircularCloudLayout(tags, tagCloudCenter);

            var directoryToSave = Directory.CreateDirectory(configuration.DirectoryToSave);
            var filename = GetFilename(configuration.TagsCount, tagCloudCenter);

            TagCloudVisualizer.Visualize(circularCloudLayout, directoryToSave, filename,
                configuration.ImageWidth, configuration.ImageHeight);

            Console.WriteLine("Visualization has been saved to " + Path.Combine(directoryToSave.FullName, filename));

            Console.ReadKey();
        }

        private static TagCloudConfiguration ParseArguments(string[] args)
        {
            var parser = new FluentCommandLineParser<TagCloudConfiguration>();

            parser.Setup(arg => arg.TagsCount)
                .As('c', "tagsCount")
                .Required();

            parser.Setup(arg => arg.DirectoryToSave)
                .As('d', "directory")
                .Required();

            parser.Setup(arg => arg.CenterX)
                .As('x', "centerX")
                .SetDefault(0);
            parser.Setup(arg => arg.CenterY)
                .As('y', "centerY")
                .SetDefault(0);

            parser.Setup(arg => arg.ImageWidth)
                .As('w', "ImageWidth")
                .SetDefault(2048);

            parser.Setup(arg => arg.ImageHeight)
                .As('h', "ImageHeight")
                .SetDefault(1024);

            parser.Parse(args);

            return parser.Object;
        }

        private static string GetFilename(int tagsCount, Point center)
        {
            var randomPart = Path.GetRandomFileName();

            return $"{randomPart}count{tagsCount}x{center.X}y{center.Y}";
        }

        private static IEnumerable<Rectangle> GetCircularCloudLayout(IEnumerable<Size> tags, Point center)
        {
            var circularLayouter = new CircularCloudLayouter(center);

            return tags.Select(circularLayouter.PutNextRectangle);
        }
    }

    public class TagCloudConfiguration
    {
        public int TagsCount { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public string DirectoryToSave { get; set; }
    }
}