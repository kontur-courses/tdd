using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualisation.Visualisation
{
    public abstract partial class RectanglesVisualiser
    {
        private const float BrushSize = 1;
        private readonly Point sourceCenterPoint;
        private Image image;
        private Graphics graphics;

        protected RectanglesVisualiser(Point sourceCenterPoint)
        {
            this.sourceCenterPoint = sourceCenterPoint;
        }

        public RectanglesVisualiser Draw(Rectangle rectangle)
        {
            var fixedRectangle = FixRectangleCoords(rectangle);
            EnsureBitmapSize(fixedRectangle);
            var rectToDraw = new Rectangle(fixedRectangle.X + image.Width / 2, fixedRectangle.Y + image.Height / 2,
                fixedRectangle.Width, fixedRectangle.Height);
            DrawPrepared(graphics, rectToDraw);
            return this;
        }

        public Image GetImage() => (Image) image.Clone();

        protected abstract void DrawPrepared(Graphics g, Rectangle rectangle);

        private Rectangle FixRectangleCoords(Rectangle rectangle)
        {
            return new Rectangle(
                rectangle.Location.X + sourceCenterPoint.X,
                rectangle.Location.Y + sourceCenterPoint.Y,
                rectangle.Size.Width,
                rectangle.Size.Height);
        }

        private void EnsureBitmapSize(Rectangle nextRectangle)
        {
            var newBitmap = EnsureBitmapSize(image, nextRectangle);
            if (newBitmap == image)
                return;
            image = newBitmap;
            graphics?.Dispose();
            graphics = Graphics.FromImage(image);
        }

        private static Image EnsureBitmapSize(Image bitmap, Rectangle nextRectangle)
        {
            if (bitmap == null)
            {
                bitmap = new Bitmap(100, 100);
                using var g = Graphics.FromImage(bitmap);
            }
            else
            {
                var xMaxDistance = MaxAbs(nextRectangle.Left, nextRectangle.Right, bitmap.Width / 2);
                var yMaxDistance = MaxAbs(nextRectangle.Top, nextRectangle.Bottom, bitmap.Height / 2);
                return ExtendBitmap(bitmap, new Size(xMaxDistance * 2, yMaxDistance * 2));
            }

            return bitmap;
        }

        private static Image ExtendBitmap(Image bitmap, Size newSize)
        {
            if (newSize.Width < bitmap.Width || newSize.Height < bitmap.Height)
                throw new ArgumentException($"{nameof(newSize)} is less than actual bitmap size", nameof(newSize));

            if (newSize == bitmap.Size)
                return bitmap;

            var newBitmap = new Bitmap(newSize.Width, newSize.Height);
            using var g = Graphics.FromImage(newBitmap);

            g.DrawImage(bitmap, new Point((newSize - bitmap.Size) / 2));
            return newBitmap;
        }

        private static int MaxAbs(params int[] numbers) => numbers.Select(Math.Abs).Max();
    }
}