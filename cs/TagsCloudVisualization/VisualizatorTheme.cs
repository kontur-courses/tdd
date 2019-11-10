using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class VisualizatorTheme
    {
        public enum Theme
        {
            Red,
            Indigo,
            Green
        }

        public Brush BackgroundBrush { get; }
        private readonly Random random;
        private readonly List<Brush> rectangleBrushes;

        public VisualizatorTheme(Theme theme)
        {
            BackgroundBrush = GetBackgroundBrush(theme);
            rectangleBrushes = GetRectangleBrushes(theme);
            random = new Random();
        }

        public Brush GetRandomRectangleBrush() => rectangleBrushes[random.Next(0, rectangleBrushes.Count)];

        private static List<Brush> GetRectangleBrushes(Theme theme)
        {
            switch (theme)
            {
                case Theme.Red:
                    return new List<Brush>
                    {
                        GetSolidBrush("#E57373"),
                        GetSolidBrush("#F44336"),
                        GetSolidBrush("#D32F2F"),
                        GetSolidBrush("#B71C1C"),
                    };

                case Theme.Indigo:
                    return new List<Brush>
                    {
                        GetSolidBrush("#7986CB"),
                        GetSolidBrush("#3F51B5"),
                        GetSolidBrush("#303F9F"),
                        GetSolidBrush("#1A237E"),
                    };

                case Theme.Green:
                    return new List<Brush>
                    {
                        GetSolidBrush("#81C784"),
                        GetSolidBrush("#4CAF50"),
                        GetSolidBrush("#388E3C"),
                        GetSolidBrush("#1B5E20"),
                    };

                default:
                    return new List<Brush> {Brushes.Black};
            }
        }

        private static Brush GetBackgroundBrush(Theme theme)
        {
            switch (theme)
            {
                case Theme.Red:
                    return GetSolidBrush("#FFCDD2");

                case Theme.Indigo:
                    return GetSolidBrush("#C5CAE9");

                case Theme.Green:
                    return GetSolidBrush("#C8E6C9");

                default:
                    return Brushes.White;
            }
        }

        private static SolidBrush GetSolidBrush(string hexColor) => new SolidBrush(ColorTranslator.FromHtml(hexColor));
    }
}