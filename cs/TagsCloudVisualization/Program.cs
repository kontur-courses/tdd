using System.Drawing;
using TagsCloudVisualization.Task_2;


namespace TagsCloudVisualization;

internal class Program
{
    private const string Path = "../../../../TagsCloudVisualization/Task 2";

    static void Main()
    {
        var colors = new[] { Color.Red, Color.Green, Color.Brown, Color.Yellow, Color.Blue };

        var visual = new VisualizationCloudLayout(800, 600, Color.White, colors);

        visual.CreateImage(new CircularCloudLayouter(new Point(400, 300)), 175, new Size(30, 5), new Size(100, 25))
            .Save($"{Path}/CentralСloud.png");

        visual.CreateImage(new CircularCloudLayouter(new Point(250, 150)), 50, new Size(30, 5), new Size(80, 25))
            .Save($"{Path}/SmalСloud.png");
    }
}
