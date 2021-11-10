using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualiser
    {
        private CircularCloudLayouter layouter;

        public TagsCloudVisualiser(Point center)
        {
            layouter = new CircularCloudLayouter(center);
        }

        public RectangleF PutRectangle(Size size)
        {
            return layouter.PutNextRectangle(size);
        }

        public Bitmap DrawCloud(Point frameLocation, Size frameSize, Size bitmapSize)
        {
            var image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var graphics = Graphics.FromImage(image);
            graphics.ScaleTransform((float)bitmapSize.Width / frameSize.Width, 
                                    (float)bitmapSize.Height / frameSize.Height);
            graphics.TranslateTransform(frameLocation.X, frameLocation.Y);
            graphics.FillRectangles(Brushes.Red, layouter.Rectangles.ToArray());
            graphics.DrawRectangles(new Pen(Color.Black, (float)frameSize.Width / bitmapSize.Width * 4), layouter.Rectangles.ToArray());
            graphics.DrawPolygon(new Pen(Color.Blue, (float)frameSize.Width / bitmapSize.Width * 4), layouter.figure.Vertexes.ToArray());
            return image;
        }
    }
}