
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
           var circularCloudDrawing = new CircularCloudDrawing(new Size(1600, 900));
           for (int i = 1; i < 100; i++)
           {
               circularCloudDrawing.DrawStrings(i.ToString(), new Font("Arial", 20));
           }
           
           /*circularCloudDrawing.DrawStrings("Как дела?", new Font("Arial", 10));
           circularCloudDrawing.DrawStrings("Чем занимаешься?", new Font("Arial", 15));
           circularCloudDrawing.DrawStrings("Делаю уроки", new Font("Arial", 11));
           circularCloudDrawing.DrawStrings("Играю", new Font("Arial", 12));
           circularCloudDrawing.DrawStrings("В инете сижу", new Font("Arial", 5));
           circularCloudDrawing.DrawStrings("Вот такие дела", new Font("Arial", 9));*/
           circularCloudDrawing.SaveImage("1.png");
        }
    }
}