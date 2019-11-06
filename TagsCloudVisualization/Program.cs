using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        private static List<Bitmap> Bitmaps = new List<Bitmap>();
        static void Main(string[] args)
        {
            Bitmaps.Add(GetImgOf200IncreazingRectangle());
            Bitmaps.Add(GetImgOf200DecreazingRectangle());
            Bitmaps.Add(GetImgOf300VerticalRectangle());
            SaveBitmaps();
        }

        private static Bitmap GetImgOf200IncreazingRectangle() =>
            GetCloudImage(200, SizeGenerator.GenerateRandomIncreasingRectanglesSize(), new Point(0, 0));
        private static Bitmap GetImgOf200DecreazingRectangle() =>
            GetCloudImage(200, SizeGenerator.GenerateRandomDecreasingRectanglesSize(), new Point(0, 0));
        private static Bitmap GetImgOf300VerticalRectangle() =>
            GetCloudImage(300, SizeGenerator.GenerateVerticalRectanglesSize(), new Point(0, 0));


        private static Bitmap GetCloudImage(int rectCount, IEnumerable<Size> sizeGenerator, Point centerPoint)
        {
            var cloud = ConstructNewTagCloud(rectCount, sizeGenerator, centerPoint);

            return Visualizer.GetCloudVisualization(cloud);
        }

        private static void SaveBitmaps()
        {
            for(var i = 0; i < Bitmaps.Count;i++)
            {
                Bitmaps[i].Save(String.Format(@"cloud{0}.jpg",i), ImageFormat.Jpeg);
            }
        }

        private static CircularCloudLayouter ConstructNewTagCloud(int count, IEnumerable<Size> sizeGen, Point centerPoint)
        {
            var cloud =  new CircularCloudLayouter(centerPoint);
            foreach (var size in sizeGen)
            {
                cloud.PutNextRectangle(size);
                count--;
                if (count <= 0) break;
            }
            return cloud;
        }
    }
}