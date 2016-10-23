using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;


namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines;
            try
            {
                var fileName = @"wordsForCloud.txt";
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
                var coefficientFontSize = 2 * ((double)( countLines - i) / countLines) * 
                    (averageLengthString / (averageLengthString + lines[i].Length));
                picture.DrawPhrase(lines[i], coefficientFontSize);
            }
            picture.SaveToFile("image.png");
        }

    }
}
