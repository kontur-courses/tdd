using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            DrawExample(Examples.GenerateFirstExample("Arial"), new Size(1000, 800),"../../../ExamplesIMG/1.png");
            DrawExample(Examples.GenerateSecondExample("Arial"), new Size(1000, 800), "../../../ExamplesIMG/2.png");
            DrawExample(Examples.GenerateThirdExample("Arial"), new Size(1000, 800), "../../../ExamplesIMG/3.png");
        }

        private static void DrawExample(List<(string, Font)> words, Size size, string filename)
        {
            var visualizer = new Visualizer(size, Color.Bisque);
            visualizer.Draw(words, filename);
        }

        
    }
}