using System;
using System.IO;
using System.Linq;

namespace TagCloud.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("Input was null. For help write \"help\" in input");
                Console.ReadLine();
                return;
            }

            if (args[0].ToLower() == "help")
            {
                WriteHelp();
                Console.ReadLine();
                return;
            }

            var pathToWords = args[0];
            var pathToPicture = args[1];
            var settings = DrawSettings.RectanglesWithNumeration;
            if (args[2] != null)
                settings = (DrawSettings)int.Parse(args[2]);

            var spiral = new Spiral();
            var cloud = new CircularCloudLayouter(spiral);
            var visualizer = new CloudVisualizer
            {
                Settings = settings
            };

            var sizes = WordReader.ReadWords(pathToWords);
            var rectangles = sizes
                .Select(s => cloud.PutNextRectangle(s))
                .ToArray();

            var picture = visualizer.CreatePictureWithRectangles(rectangles);
            picture.Save(Directory.GetCurrentDirectory() + $"{pathToPicture}.png");
        }

        static void WriteHelp()
        {
            Console.WriteLine("Input should be in format: \n" +
                              "[pathToWords] [pathToPicture] [drawSettings = RectanglesWithNumeration] \n" +
                              "Paths starting from exe directory.\n " +
                              "For example:\n" +
                              "Exe in C:.../Test/TagCloud.Utility.exe\n" +
                              "Input: words.txt result\n" +
                              "In result: words should be in ../Test/words.txt," +
                              " picture will be saved in ../Test/result.png");
        }
    }
}