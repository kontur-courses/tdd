using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CloudImageGenerator
    {
        public readonly Size ImageSize;

        public readonly ICloudLayouter Layouter;

        public Color RectangleBorderColor { get; set; }

        public CloudImageGenerator(ICloudLayouter layouter, Size imageSize, Color rectangleBorderColor)
        {
            Layouter = layouter;
            ImageSize = imageSize;
            RectangleBorderColor = rectangleBorderColor;
        }

        public Bitmap Generate(IReadOnlyList<Rectangle> layout)
        {
            var bitmap = new Bitmap(ImageSize.Width, ImageSize.Height);

            var graphics = Graphics.FromImage(bitmap);

            var pen = new Pen(RectangleBorderColor, 1);

            graphics.DrawRectangles(pen, layout.ToArray());

            return bitmap;
        }

        public Bitmap Generate(IReadOnlyList<Size> rectanglesSizes)
        {
            var layout = GetLayout(rectanglesSizes);

            return Generate(layout);
        }

        private Rectangle[] GetLayout(IReadOnlyList<Size> rectanglesSizes)
        {
            var rectangles = new List<Rectangle>();

            foreach (var size in rectanglesSizes)
                rectangles.Add(Layouter.PutNextRectangle(size));

            return rectangles.ToArray();
        }
    }
}
