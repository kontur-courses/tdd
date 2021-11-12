using System;
using System.IO;
using System.Linq;
using CommandLine;

namespace TagCloudUsageSample
{
    public class CommandLineCloudOptions
    {
        private int cloudCount;
        private int rectangleCount;
        private string savePath;
        private string fileName;
        private int sizeCoefficient;
        private int minimumRectHeight;

        [Option('c', "count", Default = 1, HelpText = "Set required tag cloud count.")]
        public int CloudCount
        {
            get => cloudCount;
            private set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentOutOfRangeException($"count should be grate then 0 and less then 100");
                cloudCount = value;
            }
        }

        [Option("rectangleCount", Default = 100, HelpText = "Set required rectangles count.")]
        public int RectangleCount
        {
            get => rectangleCount;
            private set
            {
                if (value < 1 || value > 10000)
                    throw new ArgumentOutOfRangeException($"count should be grate then 0 and less then 10000");
                rectangleCount = value;
            }
        }

        [Option('p', "path", Default = "..\\..\\CloudSamples", HelpText = "Set path to save tag clouds.")]
        public string SavePath
        {
            get => savePath;
            private set
            {
                if (!Directory.Exists(value))
                    throw new ArgumentException($"unknown directory");
                savePath = value;
            }
        }

        [Option('n', "name", Required = true, HelpText = "Set name to save tag clouds.")]
        public string FileName
        {
            get => fileName;
            private set
            {
                var invalidSymbols = Path.GetInvalidFileNameChars();
                if (value.Any(x => invalidSymbols.Contains(x)))
                    throw new ArgumentException($"invalid file name");
                fileName = value;
            }
        }

        [Option('s', "size", Default = 100, HelpText = "Set rectangle size coefficient.")]
        public int SizeCoefficient
        {
            get => sizeCoefficient;
            private set
            {
                if (value < 1 || value > 500)
                    throw new ArgumentOutOfRangeException($"size coefficient should be grate then 0 and less then 500");
                sizeCoefficient = value;
            }
        }

        [Option('m', "minimumRectHeight", Default = 2, HelpText = "Set minimum rectangle height.")]
        public int MinimumRectHeight
        {
            get => minimumRectHeight;
            private set
            {
                if (value < 1 || value > 250)
                    throw new ArgumentOutOfRangeException($"minimum rect height should be grate then 0 and less then 250");
                minimumRectHeight = value;
            }
        }
    }
}