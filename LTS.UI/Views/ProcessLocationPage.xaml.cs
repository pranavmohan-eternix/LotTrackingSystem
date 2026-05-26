using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using LTS.Application.Services;
using LTS.Data.Repositories;
using LTS.UI.Animations;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class ProcessLocationPage : Page
    {
        private RouteAnimationEngine _engine;

        public ProcessLocationPage()
        {
            InitializeComponent();

            var repo =
                new ProcessLocationRepository();

            var service =
                new ProcessLocationService(repo);

            DataContext =
                new ProcessLocationViewModel(service);

            Loaded += ProcessLocationPage_Loaded;
        }

        private void ProcessLocationPage_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(
                new Action(async () =>
                {
                    _engine =
                        new RouteAnimationEngine(
                            AnimationCanvas);

                    await _engine.AnimateRouteAsync(
                        new List<FrameworkElement>
                        {
                            S1,
                            S2,
                            S3,
                            S4,
                            S5,
                            S6,
                            S7,
                            S8,
                            S9,
                            S10
                        });
                }),
                DispatcherPriority.Render);
        }
    }
}