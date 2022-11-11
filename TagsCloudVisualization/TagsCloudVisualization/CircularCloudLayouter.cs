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
        private Bitmap bitmap;
        private Graphics g;

        public CircularCloudLayouter(IDictionary<string, int> sizeDictionary)
        {
            this.sizeDictionary = sizeDictionary;
            bitmap=new Bitmap(1000, 1000);
            g = Graphics.FromImage(bitmap);
        }

        public IEnumerable<Tuple<string, Size, Font>> GetNextRectangleOptions()
        {
            foreach (var pair in sizeDictionary)
            {
                var font = new Font("Times", pair.Value);
                var size = g.MeasureString(pair.Key, font).ToSize();
                yield return new Tuple<string, Size, Font>(pair.Key, size, font);
            }
        }
 
    }
     
}
