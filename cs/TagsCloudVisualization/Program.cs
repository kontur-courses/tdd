using System;
using TagsCloudVisualization.CloudSamples;
using System.CommandLine;
using TagsCloudVisualization.VisualizationSettings;

namespace TagsCloudVisualization
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            new VisualizerSettings().SaveSettingsIntoConfig();
            VisualizationSamples.SampleWithIncreasingRectangleSize();
            // var settings = new VisualizerSettings();
            // settings.SaveSettingsIntoConfig();
        }
    }
}