using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layout = new CircularCloudLayout(new Point(200, 200));
            var sizes = SizeListBulder.Shuffle(SizeListBulder.GetCustomSizes());
            var rectanglesOutput = new List<Rectangle>();
            sizes.ForEach(x =>
            {
                if (layout.PutNextRectangle(x, out Rectangle rectangle))
                    rectanglesOutput.Add(rectangle);
            });

            Bitmap picture = new(410, 410);
            Graphics g = Graphics.FromImage(picture);
            g.Clear(Color.White);
            rectanglesOutput.ForEach(x => g.DrawRectangle(Pens.Black, x));
            g.DrawEllipse(Pens.Black, 0, 0, 400, 400);
            picture.Save(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Layout.bmp"));
        }

    }

}