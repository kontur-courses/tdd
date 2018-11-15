using System.Drawing;

namespace TagsCloudVisualization
{
    public static class TagCloudImage
    { 
        public static Bitmap GenerateTagCloudBitmap(Rectangle[] rectangles, Size tagCloudSize)
        {
            var wigth = tagCloudSize.Width * 2;
            var height = tagCloudSize.Height*2;
            var resultBitmap = new Bitmap(wigth, height);
            var graphics = Graphics.FromImage(resultBitmap);
            graphics.TranslateTransform(wigth/2, height/2);           
            DrawRectangles(rectangles, graphics);
            graphics.FillEllipse(Brushes.Red, -1, -1, 3, 3);
            graphics.DrawEllipse(Pens.Red, new Rectangle(-tagCloudSize.Width/2, -tagCloudSize.Height/2,tagCloudSize.Width, tagCloudSize.Height));
            return resultBitmap;
        }

        private static void DrawRectangles(Rectangle[] rectangles, Graphics g)
        {
            foreach (var rect in rectangles)
            {
                g.FillRectangle(new SolidBrush(ColorPallete.GetRandomColor()), rect);
                g.DrawRectangle(new Pen(Color.Black, 4), rect);
            }
               
        }
    }
}
    