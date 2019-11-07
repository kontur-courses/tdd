using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        private readonly Pen rectangleBorderPen = new Pen(Color.Black);
        private readonly Brush rectangleFillBrush = new SolidBrush(Color.Gray);
        private readonly Brush backgroundBrush = new SolidBrush(Color.White);
        private readonly Brush textBrush = new SolidBrush(Color.Blue);

        public Bitmap VisualizeLayout(CircularCloudLayouter layouter)
        {
            var rectangles = layouter.GetRectangles();
            if (layouter.CloudBottomBorder == 0 && layouter.CloudRightBorder == 0)
            {
                return null;
            }

            var image = new Bitmap(
                layouter.CloudRightBorder + layouter.CloudLeftBorder,
                layouter.CloudBottomBorder + layouter.CloudTopBorder);

            var backgroundRectangle = new Rectangle(
                0,
                0,
                layouter.CloudRightBorder + layouter.CloudLeftBorder,
                layouter.CloudBottomBorder + layouter.CloudTopBorder);

            using (var graphics = Graphics.FromImage(image))
            {
                graphics.FillRectangle(backgroundBrush, backgroundRectangle);
                var rectangleNumber = 0;
                foreach (var rectangle in rectangles)
                {
                    rectangleNumber++;
                    graphics.FillRectangle(rectangleFillBrush, rectangle);
                    graphics.DrawRectangle(rectangleBorderPen, rectangle);
                    graphics.DrawString(
                        rectangleNumber.ToString(),
                        new Font(FontFamily.GenericSansSerif, rectangle.Height / 3),
                        textBrush,
                        rectangle);
                }
            }

            return image;
        }
    }
}