using System.Drawing;
using TagsCloudVisualization.Core.Interfaces;

namespace TagsCloudVisualization.Core.Helpers
{
    public class BitmapSaver : IImgSaver
    {
        private readonly Bitmap _bitmap;
        private readonly Graphics _graphics;
        private readonly Pen _pen;

        public BitmapSaver(Size imgSize)
        {
            _bitmap = new Bitmap(imgSize.Width, imgSize.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _pen = new Pen(Color.White, 2);
        }
        public void Draw(IEnumerable<Rectangle> rectangles)
        {
            foreach (var rect in rectangles)
            {
                _graphics.FillRectangle(Brushes.Red, rect);
                _graphics.DrawRectangle(_pen, rect);
            }
        }

        public void Save(string path)
        {
           _bitmap.Save(path);
        }
    }
}
