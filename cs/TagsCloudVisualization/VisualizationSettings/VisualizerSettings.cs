using System.IO;
using System.Text.Json;

namespace TagsCloudVisualization.VisualizationSettings
{
    public class VisualizerSettings
    {
        private static readonly string ConfigPath =
            Path.Combine(Directory.GetCurrentDirectory(), "VisualizationSettings", "config.json");

        public string WorkDirectory { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        public static VisualizerSettings ReadSettingsFromConfig()
        {
            var jsonString = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<VisualizerSettings>(jsonString);
        }

        public void SaveSettingsIntoConfig(string workDir = "TagClouds", int imageWidth = 1280, int imageHeight = 720)
        {
            WorkDirectory = workDir;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            
            var jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(ConfigPath, jsonString);
        }
    }
}