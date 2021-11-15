using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualization.Visualization
{
    public class CircularCloudVisualizator
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private string path;
        private Pen pen;
        private readonly Random random;

        public CircularCloudVisualizator(string imageSavingPath = "./")
        {
            path = imageSavingPath;

            bitmap = new Bitmap(800, 600);
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(Color.Black);
            random = new Random();
        }

        public void PutRectangle(RectangleF rectangle)
        {
            graphics.DrawRectangles(pen, new []{ rectangle });
        }

        public void PutRectangles(IEnumerable<RectangleF> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                PutRectangle(rectangle);
            }
        }

        public void PutWordInRectangle(string word, RectangleF rectangle)
        {
            var wordSize = (int) rectangle.Width / word.Length;
            graphics.DrawString(word, new Font(FontFamily.GenericSansSerif, wordSize), pen.Brush, rectangle);
        }

        public void PutWordsInRectangles(IEnumerable<string> words, IEnumerable<RectangleF> rectangles)
        {
            if (words.Count() != rectangles.Count())
            {
                throw new ArgumentException("Texts and Rectangles counts should be the same");
            }

            foreach (var tuple in words.Zip(rectangles))
            {
                PutWordInRectangle(tuple.First, tuple.Second);
            }
        }

        public string SaveImage(string imageName)
        {
            var savingPath = path + imageName;
            bitmap.Save(savingPath);
            return savingPath;
        }

        public string SaveImage()
        {
            var randomNumber = random.Next();
            var savingPath = path + randomNumber.ToString() + ".png";
            bitmap.Save(savingPath);
            return savingPath;
        }
    }
}