using System.Drawing;
using System.Drawing.Drawing2D;
using CloudLayouter;

namespace CloudLayouterVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var rectanlges = new RandomRectangleGenerator(
                new Size(30, 15), 
                new Size(90, 25), 
                30);
            var imagesFolder = "../../../pictures/";
            var pen1 = new Pen(new LinearGradientBrush(
                Point.Empty, new Point(100, 100),
                Color.Cyan, Color.Blue));
            var pen2 = new Pen(new LinearGradientBrush(
                new Point(50, 100), Point.Empty, 
                Color.Teal, Color.Aquamarine));
            
            CreatePicture(new Size(500, 500), pen1, 100, 
                rectanlges, imagesFolder + "500x500.png");
            CreatePicture(new Size(1000, 1000), pen2, 410, 
                rectanlges, imagesFolder + "1000x1000.png");
        }

        private static void CreatePicture(Size imageSize, Pen pen, int rectanglesCount,
            RandomRectangleGenerator generator, string saveTo)
        {
            var layouter = new CircularCloudLayouter(new Point(imageSize/2));
            var visualizer = new Visualizer(imageSize, pen);

            for (var i = 0; i < rectanglesCount; i++)
                visualizer.DrawRectangle(
                    layouter.PutNextRectangle(
                        generator.GetRandomRectangle()));
            
            visualizer.SaveImage(saveTo);
        }
    }
}