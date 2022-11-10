using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
 

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        public Size screensize;
        public IDictionary<string, int> sizeDictionary;
        private Bitmap bitmap;
        private Graphics g;
        private bool lastRectangle;
        
        public CircularCloudLayouter(IDictionary<string, int> sizeDictionary,Size screensize)
        {
            this.sizeDictionary = sizeDictionary;
            this.screensize = screensize;
            bitmap=new Bitmap(screensize.Width, screensize.Height);
            g = Graphics.FromImage(bitmap);
            lastRectangle = false;
        }

        public Tuple<string, Size, Font> GetRectangleOptions()
        {
            if (lastRectangle)
                throw new InvalidOperationException("Have not any tag");
            if (sizeDictionary.Count == 0)
            {
                lastRectangle = true;
                return new Tuple<string, Size, Font>(null, Size.Empty, null);
            }
            var nextRectangleSize = sizeDictionary.First();
            sizeDictionary.Remove(nextRectangleSize);
            var font = new Font("Times", nextRectangleSize.Value);
            var size = g.MeasureString(nextRectangleSize.Key, font).ToSize();
            return new Tuple<string, Size, Font>(nextRectangleSize.Key, size, font);
        }

        public Rectangle GetNextRectangle(Point location)
        {
            var size = GetRectangleOptions();
            return new Rectangle(location, size.Item2);
        }
    }
     
}
