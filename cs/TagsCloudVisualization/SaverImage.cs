using System;
using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class SaverImage : ICommandImage
    {
        private string filename;

        public SaverImage(string filename)
        {
            this.filename = filename;
        }

        public void Execute(Image image)
        {
            Save(image);
        }

        private void Save(Image image)
        {
            if (image == null)
                throw new ArgumentException("Image is null");

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Не корректное название файла");

            image.Save(filename);
            image.Dispose();
        }
    }
}