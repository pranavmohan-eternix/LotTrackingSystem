using System;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LTS.UI.Animations
{
    public static class StationEffects
    {
        public static async Task PulseAsync(
            UIElement station)
        {
            var scale =
                new ScaleTransform(1, 1);

            station.RenderTransform =
                scale;

            station.RenderTransformOrigin =
                new Point(0.5, 0.5);

            var storyboard =
                new Storyboard();

            var scaleX =
                new DoubleAnimation
                {
                    From = 1,
                    To = 1.08,

                    Duration =
                        TimeSpan.FromMilliseconds(220),

                    AutoReverse = true
                };

            var scaleY =
                new DoubleAnimation
                {
                    From = 1,
                    To = 1.08,

                    Duration =
                        TimeSpan.FromMilliseconds(220),

                    AutoReverse = true
                };

            Storyboard.SetTarget(
                scaleX,
                station);

            Storyboard.SetTargetProperty(
                scaleX,
                new PropertyPath(
                    "RenderTransform.ScaleX"));

            Storyboard.SetTarget(
                scaleY,
                station);

            Storyboard.SetTargetProperty(
                scaleY,
                new PropertyPath(
                    "RenderTransform.ScaleY"));

            storyboard.Children.Add(scaleX);
            storyboard.Children.Add(scaleY);

            var tcs =
                new TaskCompletionSource<bool>();

            storyboard.Completed += (_, __) =>
            {
                tcs.SetResult(true);
            };

            storyboard.Begin();

            await tcs.Task;
        }
    }
}