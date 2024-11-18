using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    internal class ImageSaverTest
    {
        private string _directoryPath = "TempFilesForTests";

        [OneTimeSetUp]
        public void Init()
        {
            Directory.CreateDirectory(_directoryPath);
        }

        [TestCase("Test.png")]
        public void SaveFile_ArgumentNullException_WithNullBitmap(string filename)
        {
            var path = Path.Combine(_directoryPath, filename);
            Assert.Throws<ArgumentNullException>(() => ImageSaver.SaveFile(null, path));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SaveFile_ThrowsArgumentException_WithInvalidFilename(string? filename)
        {
            var dummyImage = new Bitmap(1, 1);
            Assert.Throws<ArgumentException>(() => ImageSaver.SaveFile(dummyImage, filename));
        }

        [TestCase("Test.png", ExpectedResult = true)]
        public bool SaveFile_SavesFile(string filename)
        {
            var dummyImage = new Bitmap(1, 1);
            var path = Path.Combine(_directoryPath, filename);

            File.Delete(path);
            ImageSaver.SaveFile(dummyImage, path);
            return File.Exists(path);
        }


        [OneTimeTearDown]
        public void OneTimeCleanup()
        {
            if (Directory.Exists(_directoryPath))
            {
                Directory.Delete(_directoryPath, true);
            }
        }
    }
}