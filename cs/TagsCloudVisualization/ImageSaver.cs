﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class ImageSaver : IFileSaver
    {
        public void SaveImage(Bitmap bitmap, string fileName, ImageFormat imageFormat, string path = ".")
        {
            IsValidFileName(fileName);

            Directory.CreateDirectory(path);
            bitmap.Save($"{path}/{fileName}", imageFormat);
        }

        private static void IsValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name is null or consists only of white-space characters");
            }

            const string invalidCharacters = @"/\<>*:?""|";
            if (fileName.Any(symbol => invalidCharacters.Contains(symbol)))
            {
                throw new ArgumentException("File name contains invalid characters");
            }
        }
    }
}