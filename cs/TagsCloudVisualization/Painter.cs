using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace TagsCloudVisualization
{
    public class Painter
    {
        public readonly CircularCloudLayouter Layouter;
        private readonly Bitmap field;
        private readonly Graphics image;
        private Brush brush;
        
        public Painter(Size size, CircularCloudLayouter layouter)
        {
            field = new Bitmap(size.Width, size.Height);
            image = Graphics.FromImage(field);
            image.Clear(Color.White);
            Layouter = layouter;
            brush = new SolidBrush(Color.Black);
        }

        public void GetSingleColorCloud(Color color)
        {
            var rectangles = Layouter.Recatangles;
            foreach (var rectangle in rectangles)
            {
                brush = new SolidBrush(color);
                image.FillRectangle(brush, rectangle);
            }
        }
        
        public void GetMultiColorCloud()
        {
            var rectangles = Layouter.Recatangles;
            foreach (var rectangle in rectangles)
            {
                brush = new SolidBrush(getRandomColor());
                image.FillRectangle(brush, rectangle);
            }
        }
        
        private Color getRandomColor()
        {
            var random = new Random();
            return Color.FromArgb(random.Next(256),
                random.Next(256), random.Next(256));
        }
        
        public void saveImageToDefaultDirectory(string name)
        {
            var path = GetImagesPath(name);
            field.Save(path);
        }

        public string GetImagesPath(string name)
        {
            var directoryName =
                new StringBuilder(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory))))
                    .Append(@"\Images\");

            return Path.HasExtension(name)
                ? directoryName.Append(name).ToString()
                : directoryName.Append(name).Append(".png").ToString();
        }
    }
}