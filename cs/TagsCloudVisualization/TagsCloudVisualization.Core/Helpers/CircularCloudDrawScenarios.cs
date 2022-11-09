namespace TagsCloudVisualization.Core.Helpers
{
    public class CircularCloudDrawScenarios
    {
        public List<DrawScenario> Scenarios { get; set; }

        public CircularCloudDrawScenarios() { }

        public CircularCloudDrawScenarios(IEnumerable<DrawScenario> drawScenarios)
        {
            Scenarios = drawScenarios.ToList();
        }

        public void DrawAndSave()
        {
            foreach (var drawScenario in Scenarios)
            {
                drawScenario.DrawAndSave();
            }
        }
    }
}
