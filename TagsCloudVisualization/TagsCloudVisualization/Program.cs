﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            var layouter = new CircularCloudLayouter(new Point(400, 400));
            for (var i = 0; i < 200; i++)
            {
                layouter.PutNextRectangle(new Size(rnd.Next(5,40), rnd.Next(5, 20)));
            }

            var visualiser = new CircularCloudVisualiser(Color.RoyalBlue, Color.DarkBlue, Color.LightBlue);
            visualiser.SaveBitmap("test", 800, 800, layouter.Result);
        }
    }
}
