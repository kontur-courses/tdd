using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CloudImageHandler
    {
        private const int minFontSize = 6;

        private const int maxFontSize = 20;

        private int maxRectangleSquare;

        private int minRectangleSquare;

        public Size Size { get; set; }

        public CircularCloudLayouter Layouter { get; set; }

        public Bitmap Bitmap { get; private set; }

        public CloudImageHandler(Size size, CircularCloudLayouter layouter)
        {
            Size = size;
            Layouter = layouter;
            maxRectangleSquare = Layouter.Rectangles.Max(r => r.Width * r.Height);
            minRectangleSquare = Layouter.Rectangles.Min(r => r.Width * r.Height);
            Bitmap = GenerateCloudImage();
        }

        public void SaveImage(string fullFilePath)
        {
            Bitmap.Save(fullFilePath);
        }

        private Bitmap GenerateCloudImage()
        {
            var bitmap = new Bitmap(Size.Width, Size.Height);

            var graphics = Graphics.FromImage(bitmap);

            graphics.FillRectangle(Brushes.AliceBlue, 0, 0, bitmap.Width, bitmap.Height);

            foreach (var rectangle in Layouter.Rectangles)
                graphics.DrawRectangle(new Pen(Color.Black, 1), rectangle);

            return bitmap;
        }

        private Bitmap GenerateCloudImageWithTexts()
        {
            var bitmap = new Bitmap(Size.Width, Size.Height);

            var graphics = Graphics.FromImage(bitmap);

            graphics.FillRectangle(Brushes.AliceBlue, 0, 0, bitmap.Width, bitmap.Height);

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            for (int i = 0; i < Layouter.Rectangles.Count; i++)
            {
                graphics.DrawString(
                    i.ToString(),
                    new Font("Arial", GetFontSize(Layouter.Rectangles[i])),
                    Brushes.Black,
                    Layouter.Rectangles[i],
                    stringFormat);

                graphics.DrawRectangle(new Pen(Color.Black, 2), Layouter.Rectangles[i]);
            }

            return bitmap;
        }

        private int GetFontSize(Rectangle layoutRectangle)
        {
            if (minRectangleSquare == maxRectangleSquare)
                return maxFontSize;

            int square = layoutRectangle.Width * layoutRectangle.Height;

            int fontSize = minFontSize + (maxFontSize - minFontSize) * (square - minRectangleSquare) /
                (maxRectangleSquare - minRectangleSquare);

            return fontSize;
        }
    }
}
