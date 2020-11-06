using System;
using System.IO;
using System.Text.Json;

namespace TagsCloudVisualization.Visualization
{
    public class VisualizerSettings
    {
        private static readonly string ConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "Visualization",
            "VisualizerSettings", "config.json");

        public string RootDirectory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

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
            if (string.IsNullOrEmpty(workDir))
                throw new ArgumentException("WorkDirectory shouldn't be null or empty");

            var outputDirectory = Path.Combine(RootDirectory, workDir);
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            WorkDirectory = workDir;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;

            var jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(ConfigPath, jsonString);
        }
    }
}