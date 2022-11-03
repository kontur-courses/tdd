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
        public IDictionary<string, int> sizeDictionary;
        private Bitmap bitmap = new Bitmap(Image.FromFile("C:\\Users\\Lodgent\\Desktop\\git clones\\tdd\\TagsCloudVisualization\\white.jpg"));
        private Graphics g;
        public CircularCloudLayouter(IDictionary<string, int> sizeDictionary)
        {
            this.sizeDictionary = sizeDictionary;
            g = Graphics.FromImage(bitmap);
        }
        public Tuple<string,Size,Font> GetRectangleOptions()
        {
            if (sizeDictionary.Count == 0)
                return new Tuple<string, Size, Font>("",new Size(0,0),new Font("Times",1));
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
