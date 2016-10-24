using System;
using System.IO;
using System.Linq;
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
            try
            {
                var fileName = commandLineParser.Object.PathToWords;
                lines = File.ReadAllLines(fileName).ToArray();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            var width = 2000;
            var height = 2000;
            var picture = new Picture(width, height);
            double averageLengthString = (double)lines.Sum(s => s.Length)/lines.Length;
            var countLines = lines.Length;
            for (var i = 0; i < countLines; i++)
            {
                var coefficientFontSize = 2 * ((double) (countLines - i)/countLines) *
                    (averageLengthString / (averageLengthString + lines[i].Length));
                picture.DrawPhrase(lines[i], coefficientFontSize);
            }
            picture.SaveToFile(commandLineParser.Object.PathToFinalImage);
        }

        private class RunOptions
        { 
            public string PathToWords { get; set; }

            public string PathToFinalImage { get; set; }
        }



}


}
