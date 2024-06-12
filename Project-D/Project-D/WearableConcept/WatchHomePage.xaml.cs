using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace Project_D.WearableConcept
{
    public partial class WatchHomePage : ContentPage
    {
        private readonly Random _random = new Random();
        private IDispatcherTimer _dispatcherTimer;

        public WatchHomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            StartHeartRateSimulation();
        }

        private void StartHeartRateSimulation()
        {
            _dispatcherTimer = Dispatcher.CreateTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _dispatcherTimer.Tick += OnTimedEvent;
            _dispatcherTimer.Start();
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            // Generate a random BPM value between 60 and 110
            int currentBpm = _random.Next(60, 111);

            // Update the UI
            CurrentBpmLabel.Text = $"?? {currentBpm} BPM";
            AnimateHeart();
        }

        private void AnimateHeart()
        {
            HeartImage.ScaleTo(1.2, 250, Easing.CubicInOut).ContinueWith((t) =>
            {
                HeartImage.ScaleTo(1.0, 250, Easing.CubicInOut);
            });
        }
    }
}
