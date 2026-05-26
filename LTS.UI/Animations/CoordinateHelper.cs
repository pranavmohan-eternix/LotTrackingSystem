using System.Windows;
using System.Windows.Media;

namespace LTS.UI.Animations
{
    public static class CoordinateHelper
    {
        public static Point GetCenterPoint(
            FrameworkElement element,
            Visual relativeTo)
        {
            var transform =
                element.TransformToVisual(relativeTo);

            var point =
                transform.Transform(new Point(0, 0));

            return new Point(
                point.X + element.ActualWidth / 2,
                point.Y + element.ActualHeight / 2);
        }
    }
}