using System;
using System.Drawing;
using TagsCloudVisualization;


var center = new Point(612, 512);
var imageSize = new Size(1200, 1200);
var layouter = new CircularCloudLayouter(center);
var rand = new Random();
for  (int i = 0; i < 150; i++)
    layouter.PutNextRectangle(new Size(rand.Next(20,100), rand.Next(60,80)));

var cloudVisualizatior = new TagsCloudVisualizator(layouter, imageSize);
cloudVisualizatior.DrawAndSaveCloud("cloud.png");