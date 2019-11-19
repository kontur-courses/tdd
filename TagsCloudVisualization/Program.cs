using System.Drawing;

namespace TagsCloudVisualization
{
    public static class Program
    {
        static void Main()
        {
            var visualizer = new Visualizer();
            visualizer.DrawRectangles(
                RandomLayoutGenerator.CreateRandomCircularLayout(new Point(0, 0), 1, 15, 60, 100)).Save(
                "onSmallRectangleDifferenceAndSmallThickness.png");
            visualizer.DrawRectangles(
                RandomLayoutGenerator.CreateRandomCircularLayout(new Point(0, 0), 5, 15, 60, 100)).Save(
                "onSmallRectangleDifferenceAndLargeThickness.png");
            visualizer.DrawRectangles(
                RandomLayoutGenerator.CreateRandomCircularLayout(new Point(0, 0), 1, 10, 150, 100)).Save(
                "onBigRectangleDifferenceAndSmallThickness.png");
            visualizer.DrawRectangles(
                RandomLayoutGenerator.CreateRandomCircularLayout(new Point(0, 0), 5, 10, 150, 100)).Save(
                "onBigRectangleDifferenceAndLargeThickness.png");
        }
    }
}