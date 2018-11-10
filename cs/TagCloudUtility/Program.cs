using System;
using System.IO;
using System.Linq;
using TagCloud.CloudLayouter;
using TagCloud.CloudVisualizer;
using TagCloud.Models;
using TagCloud.PointsSequence;
using TagCloud.Utility.Models;

namespace TagCloud.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!IsInputCorrect(args))
                return;

            var pathToWords = args[0];
            var pathToPicture = args[1];
            var settings = DrawSettings.WordsInRectangles;
            if (args.Length == 3)
                settings = (DrawSettings)(int.Parse(args[2]) % 4);

            var spiral = new Spiral();
            var cloud = new CircularCloudLayouter(spiral);
            var visualizer = new CloudVisualizer.CloudVisualizer
            {
                Settings = settings
            };

            var cloudItems = new TagReader()
                .GetTags(pathToWords)
                .Select(tag => new CloudItem(tag.Word, cloud.PutNextRectangle(tag.Size)))
                .ToArray();

            var picture = visualizer.CreatePictureWithItems(cloudItems);
            picture.Save(Path.Combine(Directory.GetCurrentDirectory(), $"{pathToPicture}.png"));
        }

        private static bool IsInputCorrect(string[] input)
        {
            if (input.Length == 0)
            {
                Console.WriteLine("Input was null. For help write \"help\" in input");
                Console.ReadLine();
                return false;
            }

            if (input[0].ToLower() == "help")
            {
                WriteHelp();
                Console.ReadLine();
                return false;
            }

            return true;
        }

        static void WriteHelp()
        {
            Console.WriteLine("Input should be in format: \n" +
                              "[pathToWords] [pathToPicture] [drawSettings = RectanglesWithNumeration] \n" +
                              "Paths starting from exe directory.\n " +
                              "For example:\n" +
                              "Exe in C:.../Test/TagCloud.Utility.exe\n" +
                              "Input: words.txt result\n" +
                              "In result:\n" +
                              "Words should be in ../Test/words.txt\n" +
                              "Picture will be saved in ../Test/result.png \n" +
                              "\n" +
                              "Draw Settings:\n" +
                              "OnlyWords == 0\n" +
                              "WordsInRectangles == 1\n" +
                              "OnlyRectangles == 2\n" +
                              "RectanglesWithNumeration == 3 \n" +
                              "\n" +
                              "All words will be in 3 tags groups : Big,Average,Small");
        }
    }
}