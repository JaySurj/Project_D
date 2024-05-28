using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Dropbox.Api;

namespace Project_D
{
    public partial class BPMPage : ContentPage
    {
        private static readonly LocalDbService _dbService = new LocalDbService();
        private List<(int Interval, int BPM)> _heartbeatData;
        private User _currentUser;
        public string _today = DateTime.Now.ToString("yyyy-MM-dd");

        public BPMPage(User user)
        {
            InitializeComponent();
            _heartbeatData = new List<(int Interval, int BPM)>();
            _currentUser = user;
        }

        private void ResetValues()
        {
            _heartbeatData.Clear(); // Clear the list of BPM values
        }

        private async Task GenerateBPMsAsync()
        {
            Random random = new Random();
            _heartbeatData.Clear(); // Clear existing data
            for (int i = 0; i < 48; i++) // 48 intervals for 24 hours
            {
                int bpm = random.Next(50, 130); // Random BPM between 50 and 130
                _heartbeatData.Add((i, bpm)); // Assign interval index and BPM value

                // Add a delay of 1 second
                await Task.Delay(200);
            }
        }

        private async void OnGenerateBPMsClicked(object sender, EventArgs e)
        {
            ResetValues();
            await GenerateBPMsAsync();
            ProcessHeartbeatData();
            SaveDataToJson();

        }

        private async void ProcessHeartbeatData()
        {
            // Calculate average BPM for the entire day
            int totalBPM = _heartbeatData.Sum(d => d.BPM);
            int averageBPM = totalBPM / _heartbeatData.Count;

            // Save average BPM to the database
            var heartbeat = new Heartbeat
            {
                UserId = _currentUser.Id,
                Day = _today,
                AvgBPM = averageBPM
            };

            _dbService.SaveHeartbeatAsync(heartbeat).Wait();
             await _dbService.Update(heartbeat);

            // Display average BPM
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Average BPM", $"Average BPM for {_today}: {averageBPM}", "OK");
            });

            // Display all _heartbeatData intervals and BPM values with time
            BpmLabel.Text = ""; // Clear existing text
            foreach (var dataPoint in _heartbeatData)
            {
                string time = TimeFromInterval(dataPoint.Interval);
                BpmLabel.Text += $"Time: {time}, BPM: {dataPoint.BPM}\n";
            }
        }

        private string TimeFromInterval(int interval)
        {
            int hours = interval / 2;
            int minutes = (interval % 2) * 30;
            return $"{hours:00}:{minutes:00}";
        }

        private void SaveDataToJson()
        {
            var userData = new
            {
                UserId = _currentUser.Id,
                Fullname = _currentUser.Fullname,
                Email = _currentUser.Email,
                HeartbeatData = _heartbeatData.ConvertAll(d => new { Time = TimeFromInterval(d.Interval), d.BPM })
            };

            string json = JsonSerializer.Serialize(userData, new JsonSerializerOptions { WriteIndented = true });

            string localFilePath = Path.Combine(FileSystem.AppDataDirectory, "user_bpm_data.json");
            File.WriteAllText(localFilePath, json);

            // Upload the JSON to Dropbox
            UploadJsonToDropbox(localFilePath);
        }

        private async Task UploadJsonToDropbox(string localFilePath)
        {
            try
            {
                string dropboxFilePath = "/user_bpm_data.json"; // Path in Dropbox
                await LocalDbService.UploadFileAsync(localFilePath, dropboxFilePath);
                await DisplayAlert("Success", "JSON file uploaded to Dropbox successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to upload JSON to Dropbox: {ex.Message}", "OK");
            }
        }
    }
}
