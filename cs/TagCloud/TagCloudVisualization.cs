using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloud
{
    public class TagCloudVisualization
    {
        private readonly int rectanglesCount;
        private readonly string name;
        private Graphics graphics;
        private readonly Bitmap image;
        private readonly Random random = new Random();
        private readonly CircularCloudLayouter layouter;

        private readonly Brush[] colors =
        {
            Brushes.Aqua, Brushes.Lime, Brushes.Blue, Brushes.Brown, Brushes.Chartreuse,
            Brushes.Chocolate, Brushes.Coral, Brushes.Crimson, Brushes.MediumSlateBlue,
            Brushes.Gold, Brushes.Green, Brushes.Fuchsia, Brushes.BlueViolet
        };

        public TagCloudVisualization(int rectanglesCount, Size size, string name)
        {
            layouter = new CircularCloudLayouter(new Point(0, 0), size);
            image = new Bitmap(size.Width, size.Height);
            this.rectanglesCount = rectanglesCount;
            this.name = name;
            SetGraphics();
        }

        public TagCloudVisualization(int rectanglesCount, CircularCloudLayouter layouter, string name)
        {
            this.layouter = layouter;
            image = new Bitmap(layouter.screenSize.Width, layouter.screenSize.Height);
            this.rectanglesCount = rectanglesCount;
            this.name = name;
            SetGraphics();
        }

        private void SetGraphics()
        {
            graphics = Graphics.FromImage(image);
            graphics.TranslateTransform(image.Width / 2, image.Height / 2);
        }

        private void PaintRectangleOnCanvas()
        {
            var parameter = random.Next(35, 120);
            var rectangle = layouter.PutNextRectangle(
                new Size(parameter, (int) (parameter / 2.5)));
            graphics.FillRectangle(colors[random.Next(0, colors.Length)], rectangle);
        }

        private void SaveImage()
        {
            image.Save(name + ".png", ImageFormat.Png);
        }

        public void MakeTagCloud()
        {
            for (var i = 0; i < rectanglesCount; i++)
                PaintRectangleOnCanvas();
            SaveImage();
        }

        public void MakeTagCloudForUnitTest()
        {
            foreach (var rectangle in layouter.Rectangles)
                graphics.FillRectangle(colors[random.Next(0, colors.Length)], rectangle);
            //image.Save(Environment.CurrentDirectory + name + ".png");
            image.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".png"));
        }
    }
}