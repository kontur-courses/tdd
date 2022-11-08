using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class RectangleVisualisator
    {
        private CircularCloudLayouter _layouter;
        private Bitmap _bitmap;
        public RectangleVisualisator(Point center, Bitmap bitmap)
        {
            _layouter = new CircularCloudLayouter(center);
            _bitmap = bitmap;
            _layouter.AddRandomRectangles(50);
        }

        public void CreateImage()
        {
            Paint();
            Save();
        }

        public void Paint()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(Color.Red);

            foreach (var rectangle in _layouter.Rectangles)
                graphics.DrawRectangle(pen, rectangle);
        }
        
        private void Save()
        {
            _bitmap.Save("Rectangles.png", ImageFormat.Png);
        }
    }
}