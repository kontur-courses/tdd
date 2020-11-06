using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Visualization.VisualizationSettings;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class VisualizerSettings_Should
    {
        [Test]
        public void ReadSettingsFromConfig_CallMethod_ConfigFileExists()
        {
            var configPath = Path.Combine(Environment.CurrentDirectory, "Visualization", "VisualizationSettings",
                "config.json");
            File.Exists(configPath).Should().BeTrue();
        }

        [Test]
        public void ReadSettings_AfterSaveNewSettings_CorrectReadingFromNewConfig()
        {
            var settings = new VisualizerSettings();
            settings.SaveSettingsIntoConfig("Temp", 800, 850);
            var changedSettings = VisualizerSettings.ReadSettingsFromConfig();
            changedSettings.WorkDirectory.Should().Be("Temp");
            changedSettings.ImageWidth.Should().Be(800);
            changedSettings.ImageHeight.Should().Be(850);
        }

        [Test]
        public void ReadSettings_AfterSaveNewSettings_BeSaveSettings()
        {
            var settings = new VisualizerSettings();
            settings.SaveSettingsIntoConfig("Temp", 800, 850);
            var changedSettings = VisualizerSettings.ReadSettingsFromConfig();
            changedSettings.Should().BeEquivalentTo(settings);
        }

        [Test]
        public void SaveSettings_WorkDirIsNull_ThrowArgumentException()
        {
            var settings = new VisualizerSettings();
            Action callSave = () => settings.SaveSettingsIntoConfig(null, 100, 100);
            callSave.Should().Throw<ArgumentException>();
        }

        [Test]
        public void SaveSettings_NonexistentDirectory_MakeDirectory()
        {
            var settings = new VisualizerSettings();
            settings.RootDirectory = Path.GetTempPath();
            
            const string workDir = "newDirectory";
            var pathToDir = Path.Combine(Path.Combine(settings.RootDirectory, workDir));
            DeleteDirectoryIfExists(pathToDir);
            
            settings.SaveSettingsIntoConfig(workDir, 100, 100);
            Directory.Exists(pathToDir).Should().BeTrue();
            DeleteDirectoryIfExists(pathToDir);
            
            ReturnToDefaultSettings();

            static void DeleteDirectoryIfExists(string path)
            {
                if (Directory.Exists(path))
                    Directory.Delete(path);
            }

            void ReturnToDefaultSettings()
            {
                var oldSettings = new VisualizerSettings();
                settings.RootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                settings.SaveSettingsIntoConfig("TagClouds", 100, 100);
            }
        }
    }
}