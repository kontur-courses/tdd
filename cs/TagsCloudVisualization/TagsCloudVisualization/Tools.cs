using System;


namespace TagsCloudVisualization
{
    public class Tools
    {
        public static double AngleToStandardValue(double angle)
        {
            return  angle % (Math.PI * 2);
        }
    }
}
