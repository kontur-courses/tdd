using System.IO;

namespace TagsCloudVisualization
{
    public class PathGenerator
    {
        private string Root { get; }

        public PathGenerator()
        {
            Root = new DirectoryInfo("..\\..\\..\\Sampels").FullName;
        }

        public string GetNewFilePath()
        {
            var dateTimeProvider = new DateTimeProvider();
            var dateTime = dateTimeProvider.GetDateTimeNow();
            return $"{Root}\\{dateTime:MMddyy-HHmmssffffff}.jpg";
        }
    }
}