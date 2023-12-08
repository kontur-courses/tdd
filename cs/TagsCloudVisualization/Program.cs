using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static void Main()
        {
            var decImage = DrawDecreasingSize();
            var rndImage = DrawRandomSize();
            var sameImage = DrawSameSize();

            decImage.Save("CloudTag_DecreasingSize.png");
            rndImage.Save("CloudTag_RandomSize.png");
            sameImage.Save("CloudTag_SameSize.png");
        }

        private static Image DrawDecreasingSize()
        {
            var layouter = new CircularCloudLayouter(new Point(340, 340));
            layouter.PutNextRectangle(new Size(200, 80));
            
            for (var i = 0; i < 20; i++)
                layouter.PutNextRectangle(new Size(80, 40));
            
            for (var i = 0; i < 200; i++)
                layouter.PutNextRectangle(new Size(40, 20));
            
            return Drawer.GetImage(new Point(340, 340), layouter.Rectangles);
        }

        private static Image DrawSameSize()
        {
            var layouter = new CircularCloudLayouter(new Point(340, 340));
            for (var i = 0; i < 200; i++) 
                layouter.PutNextRectangle(new Size(30, 30));

            return Drawer.GetImage(new Point(340, 340), layouter.Rectangles);
        }

        private static Image DrawRandomSize()
        {
            var layouter = new CircularCloudLayouter(new Point(340, 340));
            var rnd = new Random();
            for (var i = 0; i < 200; i++) 
                layouter.PutNextRectangle(new Size(rnd.Next(5, 50), rnd.Next(5, 50)));

            return Drawer.GetImage(new Point(340, 340), layouter.Rectangles);
        }
    }
}
