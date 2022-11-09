using System.Collections.Generic;
using TagsCloudVisualization.Core.Helpers;

namespace TagsCloudVisualization.Sandbox
{
    internal class Program
    {
        private static void Main() 
        {
            var scenarios = new CircularCloudDrawScenarios(new List<DrawScenario>()
            {
                new DrawScenario(300, 50, 10, 50, 10, "img1"),
                new DrawScenario(200, 60, 20, 70, 30, "img2"),
                new DrawScenario(100, 40, 30, 40, 10, "img3"),
            });

            scenarios.DrawAndSave();
        }
    }
}
