﻿using System.Drawing;

namespace TagsCloud
{
    public abstract class Painter<T>
    {
        protected Brush RectangleColor { get; private set; }
        protected Bitmap? Bitmap = null;
        private readonly Random random = new Random();
        
        public abstract void Paint(IEnumerable<T> figures, Image bitmap, Action colorChanger);
        public abstract Size GetBitmapSize(IEnumerable<T> figures);

        // public void CreateBitmap()
        
        public void SetRandomRectangleColor()
        {
            // var brush = Brushes.Transparent;
            var brushesType = typeof(Brushes);
            var properties = brushesType.GetProperties();
            var randomValue = this.random.Next(properties.Length);
            
            var brush = (Brush)properties[randomValue].GetValue(null, null)!;
            RectangleColor = brush;
        }
    }
}