using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    static class TagsCloudVisualizer
    {
        public static Bitmap GetCloudVisualization(CircularCloudLayouter cloud)
        {
            var pictureRectangle = GetPictureRectangle(cloud);
            var bitmap = new Bitmap(pictureRectangle.Width, pictureRectangle.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.LightGray);
                graphics.TranslateTransform(-pictureRectangle.X, -pictureRectangle.Y);
                graphics.DrawRectangles(new Pen(Color.DodgerBlue, 5),cloud.Rectangles.ToArray());
            }

            return bitmap;
        }

        private static Rectangle GetPictureRectangle(CircularCloudLayouter cloud)
        {
            var minX = cloud.Rectangles.Min(rect => rect.Left);
            var minY = cloud.Rectangles.Min(rect => rect.Top);
            var maxX = cloud.Rectangles.Max(rect => rect.Right);
            var maxY = cloud.Rectangles.Max(rect => rect.Bottom);
            return new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
        }
    }
}
