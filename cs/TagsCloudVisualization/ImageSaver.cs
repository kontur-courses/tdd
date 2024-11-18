using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class ImageSaver
    {
        public static void SaveFile(Bitmap image, string fileName)
        {
            if (image is null)
            {
                throw new ArgumentNullException("Передаваемое изображение не должно быть null");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Некорректное имя файла для создания");
            }

            image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}