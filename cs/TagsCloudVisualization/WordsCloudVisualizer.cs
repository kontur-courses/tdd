using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public class WordsCloudVisualizer 
    {
        public Bitmap DrawCloud(IList<Word> words, int radius)
        {
            var bmp = new Bitmap(radius * 2, radius * 2);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DimGray);
                if (words.Any())
                    foreach (var word in words)
                        g.DrawString(word.Text, word.Font, Brushes.Salmon , word.Rectangle.ShiftRectangleToBitMapCenter(bmp));
            }
            return bmp;
        }
    }
}
