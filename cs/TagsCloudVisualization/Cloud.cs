using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class Cloud
    {
        public readonly List<Rectangle> Rectangles;
        private readonly CircularCloudLayouter layouter;
        private Point center;
        public Cloud(string inputFileName)
        {
            Rectangles = new List<Rectangle>();
            var sizes = ParseInputFile(inputFileName);
            this.layouter = new CircularCloudLayouter(center);
            foreach (var size in sizes)
            {
                Rectangles.Add(layouter.PutNextRectangle(size));
            }
        }

        private List<Size> ParseInputFile(string inputFileName)
        {
            var reader = new StreamReader(inputFileName);
            var centerStr = reader.ReadLine().Split(' ');
            center = new Point(int.Parse(centerStr[0]), int.Parse(centerStr[1]));
            var sizes = new List<Size>();
            while (!reader.EndOfStream)
            {
                var sizeStr = reader.ReadLine().Split(' ');
                sizes.Add(new Size(int.Parse(sizeStr[0]), int.Parse(sizeStr[1])));
            }
            reader.Close();
            return sizes;
        }
    }
}
