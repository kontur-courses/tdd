using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    internal class TagCloudDrawer
    {
        private Bitmap bitmap;
        private Graphics graphics;
        private CircularCloudLayouter layout;
        public int Scale = 1;

        public string SavePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(),
            "SavedImages", "img.jpg");

        public Pen pen = new Pen(Color.Red, 1);
        public TagCloudDrawer(CircularCloudLayouter layout)
        {
            this.layout = layout;
        }

        private void DrawRectangle(Rectangle rectangle)
        {
            graphics.FillRectangle(Brushes.Blue, rectangle);
            graphics.DrawRectangle(pen, rectangle);
        }

        public void DrawImage()
        {
            Rectangle borders = layout.GetBorders();
            bitmap = new Bitmap(borders.Width * Scale, borders.Height * Scale);
            graphics = Graphics.FromImage(bitmap);

            var ShiftedRectangles = layout.PlacedRectangles.Select(r => new Rectangle(
                (r.X - borders.X) * Scale,
                (r.Y - borders.Y) * Scale,
                r.Width * Scale,
                r.Height * Scale));

            foreach (var rectangle in ShiftedRectangles)
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
