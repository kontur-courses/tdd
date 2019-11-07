using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudDrawing
    {
        private Size _sizeImage;
        private CircularCloudLayouter _layouter;
        private Bitmap _bitmap;
        private Graphics _graphics;
        private StringFormat _stringFormat;
        private Pen _pen;
        private Brush _brush;
        
        public CircularCloudDrawing(Size sizeImage)
        {
            _bitmap = new Bitmap(sizeImage.Width, sizeImage.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _brush = Brushes.Cyan;
            _sizeImage = sizeImage;
            _layouter = new CircularCloudLayouter(new Point(sizeImage.Width / 2, sizeImage.Height / 2));
            _pen = new Pen(Brushes.MediumBlue);
            _stringFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Center
            };
        }

        public void DrawStrings(string str, Font font)
        {
            var sizeStr = (_graphics.MeasureString(str, font) + new SizeF(1, 1)).ToSize();
            var rectangleStr = _layouter.PutNextRectangle(sizeStr);
            _graphics.DrawString(str, font, _brush, rectangleStr, _stringFormat);
        }

        public void SaveImage(string filename)
        {
            _bitmap.Save(filename);
        }
    }
}