using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class UsageExamples
    {
        public static void GenerateTagCloud(Size imageSize, Dictionary<string, Font> wordsWithFonts, string filename)
        {
            var cloudLayouter = new CircularCloudLayouter(
                new Point(imageSize.Width / 2, imageSize.Height / 2),
                new ArchimedeanSpiral(1));
            var cloudDrawer = new CircularCloudDrawer(imageSize, cloudLayouter);
            foreach (var pair in wordsWithFonts)
            {
                cloudDrawer.DrawWord(pair.Key, pair.Value);
            }

            cloudDrawer.Save(filename);
        }

        public static void GenerateFirstTagCloud()
        {
            var wordsWithFonts = new Dictionary<string, Font>()
            {
                ["TDD"] = new Font("Calibri", 40),
                ["Code"] = new Font("Calibri", 15),
                ["Shpora"] = new Font("Calibri", 30),
                ["Cloud"] = new Font("Calibri", 50),
                ["Homework"] = new Font("Calibri", 15),
                ["Tests"] = new Font("Calibri", 10),
                ["Tags"] = new Font("Calibri", 25),
                ["Kontur"] = new Font("Calibri", 10),
                ["Experience"] = new Font("Calibri", 10),
                ["Lessons"] = new Font("Calibri", 30),
                ["Practice"] = new Font("Calibri", 30),
                ["Theory"] = new Font("Calibri", 25)
            };
            GenerateTagCloud(new Size(1000, 1000), wordsWithFonts,
                Environment.CurrentDirectory + @"\Examples\first.png");
        }

        public static void GenerateSecondTagCloud()
        {
            var wordsWithFonts = new Dictionary<string, Font>();
            for (var i = 1; i <= 25; i++)
            {
                wordsWithFonts.Add($"word{i.ToString()}",
                    new Font("Arial", i * 3));
            }

            GenerateTagCloud(new Size(1200, 1200), wordsWithFonts,
                Environment.CurrentDirectory + @"\Examples\second.png");
        }

        public static void GenerateThirdTagCloud()
        {
            var wordsWithFonts = new Dictionary<string, Font>();
            for (var i = 1; i <= 50; i++)
            {
                wordsWithFonts.Add(i.ToString(),
                    new Font("Arial", i * 2));
            }

            GenerateTagCloud(new Size(1200, 1200), wordsWithFonts,
                Environment.CurrentDirectory + @"\Examples\third.png");
        }
    }
}