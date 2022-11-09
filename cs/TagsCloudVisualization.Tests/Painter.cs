using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    public abstract class Painter<T>
    {
        protected Brush RectangleColor { get; private set; } = Brushes.Aqua;
        protected readonly Random Random = new Random();
        
        public abstract void Paint(IEnumerable<T> figures, Image bitmap, Action colorChanger);
        public abstract Size GetBitmapSize(IEnumerable<T> figures);

        public void SetRandomRectangleColor()
        {
            // var brush = Brushes.Transparent;
            var brushesType = typeof(Brushes);
            var properties = brushesType.GetProperties();
            var random = Random.Next(properties.Length);
            
            var brush = (Brush)properties[random].GetValue(null, null);
            RectangleColor = brush;
        }
    }
}