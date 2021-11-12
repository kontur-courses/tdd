using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagsCloudVisualization;
using TagsCloudVisualization.Layouters;
using CommandLine;

namespace TagCloudUsageSample
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<CommandLineCloudOptions>(args)
                .WithParsed(options =>
                {
                    string firstFileName = null;
                    
                    for (var j = 0; j < options.CloudCount; j++)
                    {
                        var fullFileName = options.SavePath.TrimEnd(Path.DirectorySeparatorChar) +
                                           Path.DirectorySeparatorChar +
                                           options.FileName +
                                           (options.CloudCount == 1 ? "" : $"({j})") +
                                           ".jpg";
                        
                        firstFileName ??= fullFileName;

                        var rects = GetRectangles(
                            new CircularCloudLayouter(Point.Empty),
                            options.RectangleCount,
                            options.SizeCoefficient,
                            options.MinimumRectHeight);
                        
                        if (File.Exists(fullFileName))
                            File.Delete(fullFileName);
                
                        RectanglePainter
                            .GetBitmapWithRectangles(rects)
                            .Save(fullFileName, ImageFormat.Jpeg);
                    }
                    
                    if (firstFileName is not null && options.OpenFirst)
                        System.Diagnostics.Process.Start(firstFileName);
                });
        }
    
        private static IEnumerable<Rectangle> GetRectangles(
            CircularCloudLayouter layouter, int count, 
            int sizeCoefficient, int minimumRectHeight)
        {
            var rnd = new Random();
            
            for (var i = 0; i < count; i++)
            {
                var h = rnd.Next(
                    minimumRectHeight,
                    sizeCoefficient - i < minimumRectHeight ? minimumRectHeight : sizeCoefficient - i);
                
                var w = rnd.Next(
                    h,
                    sizeCoefficient - i < h ? h : sizeCoefficient - i);
                
                yield return layouter.PutNextRectangle(new Size(w, h));
            }
        }
    }
}