using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var layout = new CircularCloudLayouter(new Point(0, 0));
            layout.PutNextRectangle(new Size(8, 2));


            Console.WriteLine();
        }
    }
}
