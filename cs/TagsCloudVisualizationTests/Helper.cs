using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    internal static class Helper
    {
        private static readonly Random Random = new(Environment.TickCount);
        public static Size GetRandomSize()
        {
            return new Size(Random.Next(50, 100), Random.Next(25, 50));
        }

        public static void GenerateRandomLayout(this CircularCloudLayouter layouter, int layoutLength = 100)
        {
            for (var i = 0; i < layoutLength; i++)
                layouter.PutNextRectangle(GetRandomSize());
        }

        public static double CalculateLayoutRadius(this CircularCloudLayouter layouter)
        {
            var layout = layouter.GetLayout();
            var center = layouter.Center;
            var maxUpperLeft = layout.Max(x => center.GetDistance(x.Location));
            var maxLowerLeft = layout.Max(x => center.GetDistance(x.Location - new Size(0, x.Height)));
            var maxUpperRight = layout.Max(x => center.GetDistance(x.Location - new Size(x.Width, 0)));
            var maxLowerRight = layout.Max(x => center.GetDistance(x.Location - new Size(x.Width, x.Height)));

            return Math.Max(Math.Max(maxLowerRight, maxLowerLeft), Math.Max(maxUpperLeft, maxUpperRight));
        }

        public static T GetFieldValue<T>(this object obj, string name)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var field = obj.GetType().GetField(name, bindingFlags);
            return (T)field?.GetValue(obj);
        }
    }
}