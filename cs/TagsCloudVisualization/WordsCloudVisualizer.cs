using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class WordsCloudVisualizer 
    {
        public Bitmap DrawCloud(IList<Word> words)
        {
            var radius = words.Select(w => w.Rectangle).GetSurroundingCircleRadius();
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
