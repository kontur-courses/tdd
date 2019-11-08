using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace TagsCloudVisualization
{
    public class Painter
    {
        private readonly Bitmap Field;
        private readonly Graphics Image;
        private Brush Brush;
        
        public Painter(Size size)
        {
            Field = new Bitmap(size.Width, size.Height);
            Image = Graphics.FromImage(Field);
            Image.Clear(Color.White);
            Brush = new SolidBrush(Color.Black);
        }
        
        public void GetSingleColorCloud(Color color, IEnumerable<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                Brush = new SolidBrush(color);
                Image.FillRectangle(Brush, rectangle);
            }
        }
        
        public Bitmap GetMultiColorCloud(IEnumerable<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                Brush = new SolidBrush(GetRandomColor());
                Image.FillRectangle(Brush, rectangle);
            }

            return Field;
        }
        
        private Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromArgb(random.Next(256),
                random.Next(256), random.Next(256));
        }
    }
}