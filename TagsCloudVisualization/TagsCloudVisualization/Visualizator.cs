using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class Visualizator
    {
        public int heightSize;
        private FrequencyTags tags;
        public List<Rectangle> rectangles=new List<Rectangle>();
        public Size srcSize=new Size();

        public Visualizator(FrequencyTags tags=null,int heightSize=1000)
        {
            this.heightSize = heightSize;
            this.tags = tags;
            srcSize = new Size(heightSize * 2, heightSize);
        }

        public void Run()
        { 
            var graphics = CreateGraphics(out var g, srcSize);
            var arithmeticSpiral = new ArithmeticSpiral(new Point(srcSize / 2));
            var sizeDictionary = new DivideTags((srcSize.Height + srcSize.Width) * 2, tags).sizeDictionary;
            var circularCloudLayouter = new CircularCloudLayouter(sizeDictionary, srcSize);
            var point = arithmeticSpiral.GetPoint();
            var nextSizeRectangle = circularCloudLayouter.GetRectangleOptions();

            while (true)
            {
                point = arithmeticSpiral.GetPoint();
                if (nextSizeRectangle.Item2.IsEmpty)
                    break;
                if (!Contains(rectangles, point, nextSizeRectangle))
                {
                    var rectangle = new Rectangle(point - nextSizeRectangle.Item2 / 2, nextSizeRectangle.Item2);
                    rectangles.Add(rectangle);
                    arithmeticSpiral = new ArithmeticSpiral(new Point(srcSize / 2));
                    nextSizeRectangle = circularCloudLayouter.GetRectangleOptions();
                }
            }

        }
        public void RunWithSave(string fileName)
        {
            var srcSize = new Size(heightSize * 2, heightSize); 
            var graphics = CreateGraphics(out var g, srcSize);
           
            var arithmeticSpiral = new ArithmeticSpiral(new Point(srcSize / 2));
            var sizeDictionary = new DivideTags((srcSize.Height + srcSize.Width) * 2,tags).sizeDictionary;
            var circularCloudLayouter = new CircularCloudLayouter(sizeDictionary, srcSize);
            var point = arithmeticSpiral.GetPoint();
            DrawFirstTag(circularCloudLayouter, point, rectangles, g);
            var nextSizeRectangle = circularCloudLayouter.GetRectangleOptions();

            while (true)
            {
                
                point = arithmeticSpiral.GetPoint();
                if (nextSizeRectangle.Item2.IsEmpty)
                    break;
                if (!new Rectangle(point,nextSizeRectangle.Item2).IntersectsWith(rectangles.Last()) && !Contains(rectangles, point, nextSizeRectangle))
                {
                    DrawRectangle(point, rectangles, g, circularCloudLayouter, ref nextSizeRectangle);
                    arithmeticSpiral = new ArithmeticSpiral(new Point(srcSize / 2));
                }
            }
       
            graphics.Save(fileName);
             
        }

        private static Bitmap CreateGraphics(out Graphics g, Size srcSize)
        {
            var bitmap =
                new Bitmap(srcSize.Width, srcSize.Height);
            g = Graphics.FromImage(bitmap);
            return bitmap;
        }

        private static void DrawFirstTag(CircularCloudLayouter circularCloudLayouter, Point point,
            List<Rectangle> rectangles, Graphics g)
        {
            var options = circularCloudLayouter.GetRectangleOptions();
            var rectangle = new Rectangle(point - options.Item2 / 2, options.Item2);
            rectangles.Add(rectangle);
            Draw(point, g, options, rectangle);
        }

        private static bool Contains(List<Rectangle> rectangles, Point point,
            Tuple<string, Size, Font> nextSizeRectangle)
        {
            return rectangles
                .Select(x =>
                    x.IntersectsWith(new Rectangle(point - nextSizeRectangle.Item2 / 2, nextSizeRectangle.Item2)))
                .Contains(true);
        }

        private static void DrawRectangle(Point point, List<Rectangle> rectangles, Graphics g,
            CircularCloudLayouter circularCloudLayouter,
            ref Tuple<string, Size, Font> nextSizeRectangle)
        {
            var rectangle = new Rectangle(point - nextSizeRectangle.Item2 / 2, nextSizeRectangle.Item2);
            rectangles.Add(rectangle);
            Draw(point, g, nextSizeRectangle, rectangle);
            nextSizeRectangle = circularCloudLayouter.GetRectangleOptions();

        }

        private static void Draw(Point point, Graphics g, Tuple<string, Size, Font> nextSizeRectangle,
            Rectangle rectangle)
        {
            g.DrawString(nextSizeRectangle.Item1, nextSizeRectangle.Item3, new SolidBrush(Color.Red),
                point - nextSizeRectangle.Item2 / 2);
            g.DrawRectangle(new Pen(Color.Green), rectangle);
        }
    }
}
