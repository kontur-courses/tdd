using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        private const string FileName = "bitmap.png";
        public static int ImageHeight { get; set; }
        public static int ImageWidht { get; set; }

        static void Main(string[] args)
        {
            #region Delete existing image
            if (File.Exists(FileName))
                File.Delete(FileName);
            #endregion

            var cloudLayouter = new CircularCloudLayouter(new Point(500, 500));

            var random = new Random();

            for (int i = 0; i < 400; i++)
            {
                cloudLayouter.PutNextRectangle(new Size(random.Next(20, 80), random.Next(10, 40)));
            }

            DrawTagCloudImage(cloudLayouter);
        }

        private static void DrawTagCloudImage(CircularCloudLayouter cloudLayouter)
        {
            ImageWidht = Math.Max(cloudLayouter.Radius, cloudLayouter.CloudCenterPoint.X) * 2;
            ImageHeight = Math.Max(cloudLayouter.Radius, cloudLayouter.CloudCenterPoint.Y) * 2;

            Bitmap image = new Bitmap(ImageWidht, ImageHeight);

            using (var canvas = Graphics.FromImage(image))
            {
                canvas.FillRectangle(Brushes.White, new Rectangle(0, 0, ImageWidht, ImageHeight));

                Random random = new Random();

                foreach (var rectangle in cloudLayouter.CloudData)
                {
                    canvas.FillRectangle(new SolidBrush(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255))),
                        rectangle);
                }
            }

            image.Save("bitmap.png", ImageFormat.Png);
        }
    }
}
