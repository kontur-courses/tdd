using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layout1 = new CircularCloudLayouter(new Point(0, 0));
            var layout2 = new CircularCloudLayouter(new Point(2, 3));
            var layout3 = new CircularCloudLayouter(new Point(-1, 5));
            var layout4 = new WordsCloudLayouter(new Point(0, 0));

            var text = ReadFromFile($"{System.IO.Directory.GetCurrentDirectory()}/textExample.txt");
            var frequencyWords = text.GetFrequency();
            DrawCircleCloudLayout(layout1, 100, (100, 200), (100, 200), "result_0'0_100.png");
            DrawCircleCloudLayout(layout2, 1000, (100, 200), (50, 100), "result_2'3_1000.png");
            DrawCircleCloudLayout(layout3, 500, (50, 100), (100, 200), "result_-1'5_500.png");
            DrawWordsCloudLayout(layout4, frequencyWords, "words_result.png");
            
        }

        public static string ReadFromFile(string filename)
        {
            using (var streamReader = new StreamReader(filename, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }

        public static void DrawCircleCloudLayout(CircularCloudLayouter layout, int times, 
            (int width, int height) minSize, (int width, int height) maxSize, string filename)
        {
            var rectangles = new List<Rectangle>();
            var random = new Random();
            for (var i = 0; i < times; i++)
                rectangles.Add(layout.PutNextRectangle(new Size(random.Next(minSize.width, minSize.height), 
                    random.Next(maxSize.width, maxSize.height))));
            new CircularCloudVisualizer()
                .DrawCloud(rectangles, layout.Radius)
                .Save(filename);
        }

        public static void DrawWordsCloudLayout(WordsCloudLayouter layout, Dictionary<string, int> frequencyWords, string filename)
        {
            var fontCoefficient = 30;
            var words = frequencyWords
                .Select(pair => 
                    layout.PutNextWord(pair.Key, new Font(FontFamily.GenericSansSerif, pair.Value * fontCoefficient, FontStyle.Bold)));

            new WordsCloudVisualizer()
                .DrawCloud(words.ToList(), layout.Radius)
                .Save(filename);
        }

    }
}
