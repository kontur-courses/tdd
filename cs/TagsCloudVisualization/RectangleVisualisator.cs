using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class RectangleVisualisator
    {
        private CircularCloudLayouter _layouter;
        private Bitmap _bitmap;
        private Random _random;
        private List<Rectangle> _rectangles;

        public IReadOnlyList<Rectangle> Rectangles => _rectangles;
        
        public RectangleVisualisator(CircularCloudLayouter layouter, Bitmap bitmap)
        {
            _layouter = layouter;
            _bitmap = bitmap;
            _rectangles = new List<Rectangle>();
            _random = new Random();
            AddRandomRectangles(50);
        }
        
        public void AddRandomRectangles(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException();
            for (int i = 0; i < amount; i++)
            {
                _layouter.PutNextRectangle(new Size(_random.Next() % 50, _random.Next() % 50), Rectangles);
            }
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

            foreach (var rectangle in Rectangles)
                graphics.DrawRectangle(pen, rectangle);
        }
        
        private void Save()
        {
            _bitmap.Save("Rectangles.png", ImageFormat.Png);
        }
    }
}