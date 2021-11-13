using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualiser
    {
        public CircularCloudLayouter layouter;

        public TagsCloudVisualiser(Point center)
        {
            layouter = new CircularCloudLayouter(center);
        }

        public TagsCloudVisualiser(CircularCloudLayouter layouter)
        {
            this.layouter = layouter;
        }

        public RectangleF PutRectangle(Size size)
        {
            return layouter.PutNextRectangle(size);
        }

        public Bitmap DrawCloud(Rectangle frame, Size bitmapSize)
        {
            var image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var graphics = Graphics.FromImage(image);
            graphics.ScaleTransform((float)bitmapSize.Width / frame.Width, 
                                    (float)bitmapSize.Height / frame.Height);
            graphics.TranslateTransform(-frame.Location.X, -frame.Location.Y);
            graphics.FillRectangles(Brushes.Red, layouter.Rectangles.ToArray());
            graphics.DrawRectangles(new Pen(Color.Black, (float)frame.Width / bitmapSize.Width * 4), layouter.Rectangles.ToArray());
            graphics.DrawPolygon(new Pen(Color.Blue, (float)frame.Width / bitmapSize.Width * 4), layouter.CurrentPerimeter.Vertexes.ToArray());
            return image;
        }

        public Bitmap DrawCloud(Size bitmapSize)
        {
            var radius = (int)(layouter.CurrentPerimeter.Vertexes.Select(p => p.DistanceTo(layouter.Center)).Max() * 1.1);
            return DrawCloud(new Rectangle(-radius, -radius, radius * 2, radius * 2), bitmapSize);
        }
    }
}