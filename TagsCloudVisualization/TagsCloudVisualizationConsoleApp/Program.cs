using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationApp;

public static class Program
{
    public static void Main()
    {
        // Раскладка фиксированных прямоугольников
        var layout = new LayoutGenerator();
        layout.GenerateBaseLayout();
        layout.DrawLayout("static-cloud.bmp");
        
        // Раскладка случайных прямоугольников
        var layout2 = new LayoutGenerator();
        layout2.GenerateRandomLayout();
        layout2.DrawLayout("random-cloud.bmp");
    }
}