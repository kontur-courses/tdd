using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CloudTag
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var saveDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Example Images");

            #region pic1

            var pic1Layouter = new CircularCloudLayouter(300, 300);
            pic1Layouter.PutNextRectangle(new Size(30, 30));
            pic1Layouter.PutNextRectangle(new Size(50, 25));
            pic1Layouter.PutNextRectangle(new Size(30, 60));
            pic1Layouter.PutNextRectangle(new Size(50, 50));
            pic1Layouter.PutNextRectangle(new Size(60, 35));

            var pic1 = CloudPainter.DrawTagCloud(Pens.Blue, pic1Layouter);
            pic1.Save($"{saveDirectoryPath}\\pic1.png");

            #endregion
            
            #region pic2

            var pic2Layouter = new CircularCloudLayouter(300, 300);
            for (var i = 0; i < 20; i++)
            {
                pic2Layouter.PutNextRectangle(new Size((i * 10 + 20) % 80 + 30, (i * 5 + 20) % 60 + 20));
            }

            var pic2 = CloudPainter.DrawTagCloud(Pens.Red, pic2Layouter);
            pic2.Save($"{saveDirectoryPath}\\pic2.png");

            #endregion
            
            #region pic3

            var pic3Layouter = new CircularCloudLayouter(400, 400);
            for (var i = 0; i < 60; i++)
            {
                pic3Layouter.PutNextRectangle(new Size((i * 15 + 25) % 90 + 30, (i * 10 + 20) % 60 + 20));
            }

            var pic3 = CloudPainter.DrawTagCloud(Pens.Green, pic3Layouter);
            pic3.Save($"{saveDirectoryPath}\\pic3.png");

            #endregion
        }
    }
}