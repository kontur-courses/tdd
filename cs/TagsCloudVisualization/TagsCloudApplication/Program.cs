using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using TagsCloudVisualization;
using TagsCloudVisualization.Themes;

namespace TagsCloudApplication
{
    internal class Program
    {
        private const string GeneratingMessage = "Generating Tag Cloud";
        private const string SavedMessage = "Saved Tag Cloud to {0}";
        private const string WrongInputMessage = "Error, Input format is not correct.";
        private const string IntRegex = @"([-+]?[0-9]+)";
        private const string FloatRegex = @"([-+]?[0-9]*\.?[0-9]+)";
        private const string ThemeRegex = @"(red|green|indigo)";

        private const string HelpMessage = "Tag Cloud Creator Tool\n"
                                           + "Input format: \'x y radius increment angle c minS maxS w h t filename\'\n"
                                           + "Where x, y - int, coordinates of the cloud center,\n"
                                           + "radius, increment, angle - float, spiral parameters,\n"
                                           + "c - int, rectangles count,\n"
                                           + "minS, maxS - int, rectangle size interval,\n"
                                           + "w, h - int, render image size,\n"
                                           + "t - theme, red, green or indigo\n"
                                           + "filename - string, output file name.";

        private static readonly string FileNameRegex = $"([^{new string(Path.GetInvalidFileNameChars())}]+)";

        private static readonly string InputRegex =
            $"{IntRegex} {IntRegex} {FloatRegex} {FloatRegex} {FloatRegex} {IntRegex} {IntRegex} {IntRegex} " +
            $"{IntRegex} {IntRegex} {ThemeRegex} {FileNameRegex}";

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please enter tag cloud parameters, type \"help\" for help, type \"exit\" to exit");
                var input = Console.ReadLine();

                if (input == "exit")
                    break;

                if (input == "help")
                {
                    Console.WriteLine(HelpMessage);
                    continue;
                }

                var match = Regex.Match(input, InputRegex);
                if (match.Success)
                {
                    Console.WriteLine(GeneratingMessage);
                    var groups = match.Groups;
                    var center = new Point(int.Parse(groups[1].Value), int.Parse(groups[2].Value));
                    var spiral = new ArchimedesSpiral(center, float.Parse(groups[3].Value),
                        float.Parse(groups[4].Value), float.Parse(groups[5].Value));
                    var count = int.Parse(groups[6].Value);
                    var minSize = int.Parse(groups[7].Value);
                    var maxSize = int.Parse(groups[8].Value);
                    var width = int.Parse(groups[9].Value);
                    var height = int.Parse(groups[10].Value);
                    var theme = groups[11].Value == "red"
                        ? new RedTheme()
                        : groups[11].Value == "green"
                            ? new GreenTheme()
                            : (Theme) new IndigoTheme();
                    var filename = groups[12].Value;

                    var layouter = new CircularCloudLayouter(spiral);
                    var rectangles = Utils.GenerateRandomRectangles(layouter, count, minSize, maxSize, new Random());
                    var path = $"{AppDomain.CurrentDomain.BaseDirectory}{filename}";
                    CloudVisualizator.Visualize(theme, rectangles, width, height).Save(path);
                    Console.WriteLine(SavedMessage, path);
                }
                else
                    Console.WriteLine(WrongInputMessage);
            }
        }
    }
}