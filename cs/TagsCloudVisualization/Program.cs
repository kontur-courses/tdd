using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security.Cryptography;

namespace TagsCloudVisualization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
          
            var bitmap = new Bitmapper(1024, 720);
            bitmap.DrawDefaultPictures();
        }
    }
}
