using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
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
            var rectangles = options.Rectangles.Select(s => s.Split(',')).Select(x =>
                new Size(int.Parse(x[0]), int.Parse(x[1])));
            var layouter = new CircularCloudLayouter(new Point());
            var visualizer = new CloudVisualizer();
            new RectangleVisualizer(rectangles, layouter, visualizer).CreateCloudImage(options.ImagePath);
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
