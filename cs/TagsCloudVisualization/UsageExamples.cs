using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class UsageExamples
    {
        public static void GenerateTagCloud(Size imageSize, List<Tuple<string, Font>> wordsWithFonts, string filename)
        {
            var cloudLayouter = new CircularCloudLayouter(
                new Point(imageSize.Width / 2, imageSize.Height / 2),
                new ArchimedeanSpiral(1));
            var cloudDrawer = new CircularCloudDrawer(imageSize, cloudLayouter);
            foreach (var pair in wordsWithFonts)
            {
                cloudDrawer.DrawWord(pair.Item1, pair.Item2);
            }

            cloudDrawer.Save(filename);
        }

        public static void GenerateFirstTagCloud()
        {
            var wordsWithFonts = new List<Tuple<string, Font>>();
            for (var i = 1; i <= 25; i++)
            {
                wordsWithFonts.Add(new Tuple<string, Font>($"word{i.ToString()}",
                    new Font("Arial", i * 3)));
            }

            GenerateTagCloud(new Size(1200, 1200), wordsWithFonts,
                Environment.CurrentDirectory + @"\Examples\first.png");
        }

        public static void GenerateSecondTagCloud()
        {
            var wordsWithFonts = new List<Tuple<string, Font>>();
            for (var i = 1; i <= 50; i++)
            {
                wordsWithFonts.Add(new Tuple<string, Font>(i.ToString(),
                    new Font("Arial", i * 2)));
            }

            GenerateTagCloud(new Size(1200, 1200), wordsWithFonts,
                Environment.CurrentDirectory + @"\Examples\second.png");
        }

        public static void GenerateThirdTagCloud()
        {
            var wordsWithFonts = new List<Tuple<string, Font>>();
            for (var i = 1; i <= 75; i++)
            {
                wordsWithFonts.Add(new Tuple<string, Font>("25",
                    new Font("Arial", 30)));
            }

            GenerateTagCloud(new Size(1200, 1200), wordsWithFonts,
                Environment.CurrentDirectory + @"\Examples\third.png");
        }
    }
}