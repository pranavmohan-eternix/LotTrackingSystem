using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LTS.UI.Animations
{
    public class RouteAnimationEngine
    {
        private readonly Canvas _animationCanvas;

        public RouteAnimationEngine(
            Canvas animationCanvas)
        {
            _animationCanvas =
                animationCanvas;
        }

        public async Task AnimateRouteAsync(
            List<FrameworkElement> stations)
        {
            for (int i = 0; i < stations.Count - 1; i++)
            {
                await AnimateSegmentAsync(
                    stations[i],
                    stations[i + 1]);
            }
        }

        private async Task AnimateSegmentAsync(
            FrameworkElement from,
            FrameworkElement to)
        {
            Point start =
                CoordinateHelper.GetCenterPoint(
                    from,
                    _animationCanvas);

            Point end =
                CoordinateHelper.GetCenterPoint(
                    to,
                    _animationCanvas);

            // ROUTE LINE
            var line =
                RouteLineRenderer.CreateGlowLine(
                    start,
                    end);

            _animationCanvas.Children.Add(line);

            // CARRIER
            Ellipse carrier =
                CarrierRenderer.CreateCarrier();

            _animationCanvas.Children.Add(carrier);

            Canvas.SetLeft(
                carrier,
                start.X - 9);

            Canvas.SetTop(
                carrier,
                start.Y - 9);

            // MOVE
            await AnimateCarrierAsync(
                carrier,
                start,
                end);

            // ARRIVAL PULSE
            await StationEffects.PulseAsync(to);

            // FADE OUT LINE
            await FadeOutAsync(line);

            // CLEANUP
            _animationCanvas.Children.Remove(carrier);
            _animationCanvas.Children.Remove(line);

            // SMALL DELAY
            await Task.Delay(120);
        }

        private Task AnimateCarrierAsync(
            UIElement carrier,
            Point start,
            Point end)
        {
            var tcs =
                new TaskCompletionSource<bool>();

            var xAnimation =
                new DoubleAnimation
                {
                    From = start.X - 9,
                    To = end.X - 9,

                    Duration =
                        TimeSpan.FromSeconds(1.2)
                };

            var yAnimation =
                new DoubleAnimation
                {
                    From = start.Y - 9,
                    To = end.Y - 9,

                    Duration =
                        TimeSpan.FromSeconds(1.2)
                };

            Storyboard.SetTarget(
                xAnimation,
                carrier);

            Storyboard.SetTargetProperty(
                xAnimation,
                new PropertyPath("(Canvas.Left)"));

            Storyboard.SetTarget(
                yAnimation,
                carrier);

            Storyboard.SetTargetProperty(
                yAnimation,
                new PropertyPath("(Canvas.Top)"));

            var storyboard =
                new Storyboard();

            storyboard.Children.Add(xAnimation);
            storyboard.Children.Add(yAnimation);

            storyboard.Completed += (_, __) =>
            {
                tcs.SetResult(true);
            };

            storyboard.Begin();

            return tcs.Task;
        }

        private Task FadeOutAsync(
            UIElement element)
        {
            var tcs =
                new TaskCompletionSource<bool>();

            var animation =
                new DoubleAnimation
                {
                    To = 0,

                    Duration =
                        TimeSpan.FromMilliseconds(350)
                };

            animation.Completed += (_, __) =>
            {
                tcs.SetResult(true);
            };

            element.BeginAnimation(
                UIElement.OpacityProperty,
                animation);

            return tcs.Task;
        }
    }
}