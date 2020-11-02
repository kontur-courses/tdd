using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var center = new Point();
            var layouter = new CircularCloudLayouter(center);
            
            var random = new Random();
            var maxWidth = 50;
            var maxHeight = 20;
            for (int i = 0; i < 300; i++)
                layouter.PutNextRectangle(new Size(random.Next(maxWidth),random.Next(maxHeight)));
            layouter.SaveImage();
        }
    }
}