using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        private string directory;
        private CircularCloudLayouter layouter;
        private string fileName;
        public string FilePath => Directory.GetCurrentDirectory() + directory + fileName + ".png";

        private Pen rectangleBorderPen = new Pen(Color.Black);
        private Brush rectangleFillBrush = new SolidBrush(Color.Gray);
        private Brush backgroundBrush = new SolidBrush(Color.White);
        private Brush textBrush = new SolidBrush(Color.Blue);

        public CircularCloudVisualizer(CircularCloudLayouter layouter, string fileName , string directory = null)
        {
            this.layouter = layouter;
            this.fileName = fileName;
            this.directory = directory ?? @"\visualization\";
        }
        public void VisualizeLayout()
        {
            var rectangles = layouter.GetRectangles();
            if (layouter.BottomBorder == 0 && layouter.RightBorder == 0)
            {
                return;
            }
            var image = new Bitmap(layouter.RightBorder + layouter.LeftBorder,layouter.BottomBorder + layouter.TopBorder);
            var imageRectangle = new Rectangle(0, 0, layouter.RightBorder + layouter.LeftBorder, layouter.BottomBorder + layouter.TopBorder);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.FillRectangle(backgroundBrush, imageRectangle);
                var rectangleNumber = 0;
                foreach (var rectangle in rectangles)
                {
                    rectangleNumber++;
                    graphics.FillRectangle(rectangleFillBrush, rectangle);
                    graphics.DrawRectangle(rectangleBorderPen,rectangle);
                    graphics.DrawString(rectangleNumber.ToString(), new Font(FontFamily.GenericSansSerif, rectangle.Height / 3), textBrush, rectangle);
                }
            }

            image.Save(@"D:\repos\tdd\cs\TagsCloudVisualization\visualization\" + fileName + ".png", ImageFormat.Png);
            
            if (!Directory.Exists(Directory.GetCurrentDirectory() + this.directory))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + this.directory);
            }
            image.Save(FilePath, ImageFormat.Png);
        }
    }
}