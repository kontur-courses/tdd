using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            Draw(Examples.GenerateFirstExample("Arial"), "../../../ExamplesIMG/1.png");
            Draw(Examples.GenerateSecondExample("Arial"), "../../../ExamplesIMG/2.png");
            Draw(Examples.GenerateThirdExample("Arial"), "../../../ExamplesIMG/3.png");
        }

        private static void Draw(List<(string, Font)> example1, string filename)
        {
            var handler = new WordsHandler(new CircularCloudLayouter(new Spiral(0.1f, 0.8, new PointF(0, 0))));
            var template = handler.Handle(example1);
            var visualizer = new Visualizer(template.Size, Color.Bisque);
            visualizer.Draw(template, filename);
        }
    }
}