using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommandLine;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(ProcessArguments);
        }

        private static void ProcessArguments(Options options)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            foreach (var values in options.Rectangles.Select(s => s.Split(',')))
            {
                layouter.PutNextRectangle(new Size(int.Parse(values[0]), int.Parse(values[1])));
            }

            new CloudVisualizer(layouter.Rectangles).CreateImage(options.ImagePath);
        }

        private class Options
        {
            [Value(0, MetaName = "Image path", HelpText = "The path where the image will be created")]
            public string ImagePath { get; set; }
            
            [Value(1, MetaName = "Rectangles", HelpText = "Comma-separated coordinates" +
                                                          " of the rectangles' upper-left corners")]
            public IEnumerable<string> Rectangles { get; set; }
        }
    }
}
