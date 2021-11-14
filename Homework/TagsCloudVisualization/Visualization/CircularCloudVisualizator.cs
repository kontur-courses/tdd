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
        private CircularCloudLayouter layouter;
        private readonly Random random;

        public CircularCloudVisualizator(CircularCloudLayouter layouter, string imageSavingPath = "./")
        {
            this.layouter = layouter;
            path = imageSavingPath;

            bitmap = new Bitmap(800, 600);
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(Color.Black);
            random = new Random();
        }

        public void PutRectangle(SizeF rectangleSize)
        {
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            graphics.DrawRectangles(pen, new []{ rectangle });
        }

        public void PutRectangles(IEnumerable<SizeF> rectanglesSizes)
        {
            foreach (var rectangleSize in rectanglesSizes)
            {
                PutRectangle(rectangleSize);
            }
        }

        public void PutWordInRectangle(string word, SizeF rectangleSize)
        {
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            var wordSize = (int) rectangle.Width / word.Length;
            graphics.DrawString(word, new Font(FontFamily.GenericSansSerif, wordSize), pen.Brush, rectangle);
        }

        public void PutWordsInRectangles(IEnumerable<string> words, IEnumerable<SizeF> rectanglesSizes)
        {
            if (words.Count() != rectanglesSizes.Count())
            {
                throw new ArgumentException("Texts and Rectangles counts should be the same");
            }

            foreach (var tuple in words.Zip(rectanglesSizes))
            {
                PutWordInRectangle(tuple.First, tuple.Second);
            }
        }

        public void SaveImage(string imageName)
        {
            bitmap.Save(imageName);
        }

        public void SaveImage()
        {
            var randomNumber = random.Next();
            bitmap.Save(randomNumber.ToString() + ".png");
        }
    }
}