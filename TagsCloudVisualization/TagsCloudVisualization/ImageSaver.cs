using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
	public class ImageSaver
	{
		public ImageFormat Format { get; }
		public string Directory { get; }

		public ImageSaver(string directory, ImageFormat format)
		{
			Format = format;
			Directory = directory;
		}

		public void Save(Image image, string name)
		{
			var path = Directory + name + "." + Format.ToString().ToLower();
			image.Save(path, Format);
		}
	}
}
