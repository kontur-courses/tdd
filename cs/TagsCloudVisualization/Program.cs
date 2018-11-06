using System;
using System.IO;
using Fclp;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fileName = string.Empty;
            var rectanglesAmount = 0;
            var step = 0;
            var widthMultiplier = 0;
            var heightMultiplier = 0;
            var centerX = 0;
            var centerY = 0;
            var distanceBetweenSpiralPoints = 0;
            var imageFileName = string.Empty;

            var parser = new FluentCommandLineParser();
            parser.Setup<string>('f').Callback(file => fileName = file).Required();

            var result = parser.Parse(args);
            if (result.HasErrors)
                throw new ArgumentException("options file name undefined");

            parser = new FluentCommandLineParser();
            parser.Setup<int>('a', "rectanglesAmount").Callback(amount => rectanglesAmount = amount).Required();
            parser.Setup<int>('s', "step").Callback(stepSize => step = stepSize).Required();
            parser.Setup<int>('w', "widthMultiplier").Callback(width => widthMultiplier = width).Required();
            parser.Setup<int>('h', "heightMultiplier").Callback(height => heightMultiplier = height).Required();
            parser.Setup<int>('x', "centerX").Callback(x => centerX = x).Required();
            parser.Setup<int>('y', "centerY").Callback(y => centerY = y).Required();
            parser.Setup<int>('d', "distanceBetweenSpiralPoints")
                .Callback(distance => distanceBetweenSpiralPoints = distance).Required();
            parser.Setup<string>('f', "imageFileName").Callback(name => imageFileName = name).Required();

            result = parser.Parse(File.ReadAllText(fileName).Split(' '));
            if (result.HasErrors)
                throw new ArgumentException("wrong file syntax");

            new RectanglesImageGenerator().Generate(rectanglesAmount, step, widthMultiplier, heightMultiplier, centerX,
                centerY, distanceBetweenSpiralPoints, imageFileName);
        }
    }
}