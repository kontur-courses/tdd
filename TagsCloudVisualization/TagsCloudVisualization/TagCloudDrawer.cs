using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class TagCloudDrawer
    {
        private Bitmap bitmap;
        private Graphics graphics;
        private ICloudLayouter layouter;
        public int Scale = 1;

        public string SavePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(),
            "SavedImages", "img.jpg");

        public Pen pen = new Pen(Color.Red, 1);

        public TagCloudDrawer(ICloudLayouter layouter)
        {
            this.layouter = layouter;
        }

        private void DrawRectangle(Rectangle rectangle)
        {
            graphics.FillRectangle(Brushes.Blue, rectangle);
            graphics.DrawRectangle(pen, rectangle);
        }

        public void DrawImage()
        {
            Rectangle borders = layouter.GetBorders();
            bitmap = new Bitmap(borders.Width * Scale, borders.Height * Scale);
            graphics = Graphics.FromImage(bitmap);

            var ShiftedRectangles = layouter.PlacedRectangles.Select(r => new Rectangle(
                (r.X - borders.X) * Scale,
                (r.Y - borders.Y) * Scale,
                r.Width * Scale,
                r.Height * Scale));

            foreach (Rectangle rectangle in ShiftedRectangles)
            {
                DrawRectangle(rectangle);
            }
        }


        public void SaveImage()
        {
            if (!Directory.Exists(Directory.GetParent(SavePath).ToString()))
            {
                Directory.CreateDirectory(Directory.GetParent(SavePath).ToString());
            }

            bitmap.Save(SavePath, ImageFormat.Jpeg);
        }
    }
}