namespace Project_D
{

    using System.Timers;
    using Microsoft.Maui.Controls;
    using System.Linq;
    using System;

    public partial class BPMPage : ContentPage
    {
        private Timer _timer;
        private int[] _bpmArray;
        private int _currentIndex;
        private int _countHighBPM;
        private Queue<int> _lastTenBPMs; // Store the last 10 BPM values

        public BPMPage()
        {
            InitializeComponent();
            _lastTenBPMs = new Queue<int>(10); // Initialize the queue to store 10 items
        }

        private void ResetValues()
        {
            _currentIndex = 0;
            _countHighBPM = 0;
            _lastTenBPMs.Clear(); // Clear the queue of BPM values
        }

        private async Task GenerateBPMsAsync()
        {
            Random random = new Random();
            _bpmArray = new int[10];
            for (int i = 0; i < _bpmArray.Length; i++)
            {
                int bpm = random.Next(50, 130); // Random BPM between 50 and 150
                _bpmArray[i] = bpm;

                // Add a delay of 1 second
                await Task.Delay(1000);

                // Update the label with the current BPM value
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    BpmLabel.Text = $"BPM: {_bpmArray[i]}";

                    // Increment high BPM count if current BPM is higher than 100
                    if (_bpmArray[i] > 100)
                    {
                        _countHighBPM++;
                        HighBPM.Text = $"High BPM: {_countHighBPM}";
                    }
                });
            }
        }

        private async void OnGenerateBPMsClicked(object sender, EventArgs e)
        {
            // Reset values before generating new BPMs
            ResetValues();

            // Generate BPMs with delay
            await GenerateBPMsAsync();

            // Check if there are 3 or more BPM values higher than 100 in the last 10 readings
            if (_countHighBPM >= 3)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new CalmingPage());
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("BPM Check", "Your BPM is stable.", "OK");
                });
            }
        }


    }
}
