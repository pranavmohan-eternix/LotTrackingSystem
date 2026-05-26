using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace LTS.UI.Animations
{
    public static class RouteLineRenderer
    {
        public static Line CreateGlowLine(
            Point start,
            Point end)
        {
            return new Line
            {
                X1 = start.X,
                Y1 = start.Y,

                X2 = end.X,
                Y2 = end.Y,

                Stroke = Brushes.Lime,

                StrokeThickness = 5,

                Opacity = 0.9,

                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,

                Effect = new DropShadowEffect
                {
                    BlurRadius = 18,
                    ShadowDepth = 0,
                    Color = Colors.Lime,
                    Opacity = 1
                }
            };
        }
    }
}