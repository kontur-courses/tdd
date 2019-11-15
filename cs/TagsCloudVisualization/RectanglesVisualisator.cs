using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectanglesVisualisator
    {
        private List<Rectangle> rectanglesList;        

        public RectanglesVisualisator(Size imageSize, List<Rectangle> rectangles)
        {
            rectanglesList = rectangles;
        }

        public void DrawRectangles(string filename)
        {            
            using (Bitmap bmp = new Bitmap(2000, 2000))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {               
                    var pen = new Pen(Color.FromArgb(255, 0, 255, 0), 2);
                    foreach (var rectangle in rectanglesList)
                    {
                        g.DrawRectangle(pen, rectangle);
                        g.FillRectangle(Brushes.Blue, rectangle);
                    }                    
                }               
                bmp.Save(AppDomain.CurrentDomain.BaseDirectory + String.Format(@"\{0}", filename));             
            }
        }                
    }
}
