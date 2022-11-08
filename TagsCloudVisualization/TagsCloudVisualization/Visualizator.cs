using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class Visualizator
    {
        private TagCloud tagCloud;

        public Visualizator(TagCloud tagCloud)
        {
            this.tagCloud = tagCloud ?? throw new ArgumentNullException();
            tagCloud.Run();
        }

        public void Save(string fileName)
        {
            var srcSize = tagCloud.GetScreenSize();
            var graphics = CreateGraphics(out var g, srcSize);
            var rectangles = tagCloud.GetRectangles();
            foreach (var textRectangle in rectangles)
            {
                g.DrawRectangle(new Pen(Color.Black,1),textRectangle.rectangle);
                var color = Color.FromArgb((int)textRectangle.font.Size%255, 0, (int)(textRectangle.font.Size*2)%255);
                g.DrawString(textRectangle.text,textRectangle.font,new SolidBrush(color),textRectangle.rectangle.Location);
            }
            graphics.Save(fileName);
             
        }

        private static Bitmap CreateGraphics(out Graphics g, Size srcSize)
        {
            var bitmap =
                new Bitmap(srcSize.Width, srcSize.Height);
            g = Graphics.FromImage(bitmap);
            return bitmap;
        }
    }
}
