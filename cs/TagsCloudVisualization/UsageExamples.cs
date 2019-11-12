using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class UsageExamples
    {
        private static readonly Size ImageSize = new Size(1200, 1200);
        private const string FontFamilyName = "Arial";

        private static void GenerateTagCloud(IEnumerable<Tuple<string, Font>> wordsWithFonts, string filename)
        {
            var cloudLayouter = new SpiralCloudLayouter(
                new Point(ImageSize.Width / 2, ImageSize.Height / 2),
                new ArchimedeanSpiral(1));
            var cloudDrawer = new CircularCloudDrawer(ImageSize, cloudLayouter);
            foreach (var (word, font) in wordsWithFonts)
                cloudDrawer.DrawWord(word, font);
            cloudDrawer.Save(Environment.CurrentDirectory + $"\\Examples\\{filename}.png");
        }

        private static IEnumerable<Tuple<string, Font>> GetWordsWithFonts(
            int count,
            Func<int, string> wordGenerator,
            Func<int, int> fontSizeGenerator)
        {
            for (var i = 1; i <= count; i++)
            {
                yield return new Tuple<string, Font>(
                    wordGenerator(i), new Font(FontFamilyName, fontSizeGenerator(i)));
            }
        }

        public static void GenerateFirstTagCloud()
        {
            GenerateTagCloud(
                GetWordsWithFonts(25, a => $"word{a.ToString()}", a => a * 3), 
                "first");
        }

        public static void GenerateSecondTagCloud()
        {
            GenerateTagCloud(
                GetWordsWithFonts(50, a => a.ToString(), a => a * 2), 
                "second");
        }

        public static void GenerateThirdTagCloud()
        {
            GenerateTagCloud(
                GetWordsWithFonts(75, a => "25", a => 30),
                "third");
        }

        public static void GenerateFourthTagCloud()
        {
            GenerateTagCloud(
                GetWordsWithFonts(70, a => Math.Pow(5, a).ToString(), a => 10),
                "fourth");
        }
    }
}