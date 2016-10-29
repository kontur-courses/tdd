using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Fclp;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineParser = new FluentCommandLineParser<RunOptions>();
            commandLineParser
                .Setup(options => options.PathToWords)
                .As('w')
                .WithDescription("path to file with words for cloud");
            commandLineParser   
                .Setup(options => options.PathToFinalImage)
                .As('i')
                .WithDescription("path to final image");
            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader($"{AppDomain.CurrentDomain.FriendlyName} [-i image] [-w words]")
                .Callback(text => Console.WriteLine(text));
            if (commandLineParser.Parse(args).HelpCalled)
                return;
            if (commandLineParser.Object.PathToFinalImage == null || commandLineParser.Object.PathToWords == null)
            {
                Console.WriteLine("you need to specify all parameters. for help use: -h");
                return;
            }
            string[] lines;
            var fileName = commandLineParser.Object.PathToWords;
            try
            {
                lines = File.ReadAllLines(fileName).ToArray();
            }
            catch (IOException)
            {
                Console.WriteLine($"failed to read file {fileName}");
                return;
            }
            var width = 2000;
            var height = 2000;
            var cloud = new CircularCloudLayouter(new Point(width / 2, height / 2));
            var averageLengthString = (double)lines.Sum(s => s.Length)/lines.Length;
            var maxFontSize = ((double)height / lines.Length + width / averageLengthString) / 7;
            var countLines = lines.Length;
            var blocks = new List<Tag>();
            for (var i = 0; i < countLines; i++)
            {
                var coefficientFontSize = 2*((double) (countLines - i)/countLines);
                var emSize = maxFontSize * coefficientFontSize;
                var font = new Font(FontFamily.GenericSerif, (float)emSize, FontStyle.Regular);
                var size = TextRenderer.MeasureText(lines[i], font);
                size = new Size(size.Width + lines[i].Length, size.Height);
                var rectangle = cloud.PutNextRectangle(size);
                blocks.Add(new Tag(lines[i], rectangle, font));
            }
            var visualizer = new TagsCloudVisualizer(new SolidBrush(Color.DarkSlateBlue));
            using (var image = visualizer.GetImageCloud(blocks, width, height, Color.Bisque))
            {
                fileName = commandLineParser.Object.PathToFinalImage;
                try
                {
                    image.Save(fileName);
                }
                catch (Exception)
                {
                    Console.WriteLine($"error saving image {fileName}");
                }
            }
        }

        private class RunOptions
        { 
            public string PathToWords { get; set; }

            public string PathToFinalImage { get; set; }
        }
    }
}
