using System.IO;
using System.Text.Json;

namespace TagsCloudVisualization.VisualizationSettings
{
    public class VisualizerSettings
    {
        private static readonly string ConfigPath =
            Path.Combine(Directory.GetCurrentDirectory(), "config.json");

        public string WorkDirectory { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        public static VisualizerSettings ReadSettingsFromConfig()
        {
            var jsonString = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<VisualizerSettings>(jsonString);
        }

        public void SaveSettingsIntoConfig(string workDir, int imageWidth, int imageHeight)
        {
            WorkDirectory = workDir;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            
            var jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(ConfigPath, jsonString);
        }
    }
}