using System;
using System.Drawing;
using TagsCloudVisualization;


Point center = new Point(612, 512);
Size imageSize = new Size(1200, 1200);
CircularCloudLayouter layouter = new CircularCloudLayouter(center);
Random rand = new Random();
for  (int i = 0; i < 150; i++)
    layouter.PutNextRectangle(new Size(rand.Next(20,100), rand.Next(60,80)));

TagsCloudVisualizator cloudVisualizatior = new TagsCloudVisualizator(layouter, imageSize);
cloudVisualizatior.drawCloud();
cloudVisualizatior.saveImage("cloud.png");