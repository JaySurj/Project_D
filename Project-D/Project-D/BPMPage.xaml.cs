using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace Project_D
{
    public partial class BPMPage : ContentPage
    {
        private static readonly LocalDbService _dbService = new LocalDbService();
        private List<(int Interval, int BPM)> _heartbeatData;
        private User _currentUser;
        public string _today = DateTime.Now.ToString("yyyy-MM-dd");
        private DropboxClient _dbx;

        public BPMPage(User user)
        {
            InitializeComponent();
            _heartbeatData = new List<(int Interval, int BPM)>();
            _currentUser = user;
            _dbx = DropboxClientFactory.GetClient();
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
                int bpm = random.Next(50, 101); // Random BPM between 50 and 130
                _heartbeatData.Add((i, bpm)); // Assign interval index and BPM value
            }
        }

        private async void OnGenerateBPMsClicked(object sender, EventArgs e)
        {
            ResetValues();
            await GenerateBPMsAsync();
            ProcessHeartbeatData();
            await SaveDataToJson();

            // Navigate to NotificationPage
            await Navigation.PushAsync(new NotificationPage(_currentUser));
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

        private async Task SaveDataToJson()
        {
            var userData = new UserData
            {
                UserId = _currentUser.Id,
                Fullname = _currentUser.Fullname,
                Email = _currentUser.Email,
                HeartbeatData = _heartbeatData.ConvertAll(d => new HeartbeatData { Time = TimeFromInterval(d.Interval), BPM = d.BPM })
            };

            var notificationData = new UserNotification
            {
                UserId = _currentUser.Id,
                AvgBPM = _heartbeatData.Sum(d => d.BPM) / _heartbeatData.Count,
                NotificationId = Guid.NewGuid().ToString(),
                ShowNotification = true,
                NotificationType = "AverageBPM"
            };

            var highBPMNotifications = _heartbeatData
                .Where(d => d.BPM >= 100)
                .Select(d => new UserNotification
                {
                    UserId = _currentUser.Id,
                    AvgBPM = d.BPM,
                    NotificationId = Guid.NewGuid().ToString(),
                    ShowNotification = true,
                    NotificationType = "HighBPM",
                    Time = TimeFromInterval(d.Interval)
                })
                .ToList();

            string dropboxFolderPath = "/Json";
            await SaveOrAppendJson("user_bpm_data.json", userData, dropboxFolderPath);
            await SaveOrAppendJson("user_notification_data.json", notificationData, dropboxFolderPath);
            foreach (var notification in highBPMNotifications)
            {
                await SaveOrAppendJson("user_notification_data.json", notification, dropboxFolderPath);
            }
        }

        private async Task SaveOrAppendJson<T>(string filename, T data, string dropboxFolderPath)
        {
            string localFilePath = Path.Combine(FileSystem.AppDataDirectory, filename);
            List<T> dataList;

            if (File.Exists(localFilePath))
            {
                string existingJson = File.ReadAllText(localFilePath);
                try
                {
                    dataList = System.Text.Json.JsonSerializer.Deserialize<List<T>>(existingJson) ?? new List<T>();
                }
                catch
                {
                    // If deserialization fails, reset the file to an empty list
                    dataList = new List<T>();
                }
            }
            else
            {
                dataList = new List<T>();
            }

            dataList.Add(data);

            string newJson = System.Text.Json.JsonSerializer.Serialize(dataList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(localFilePath, newJson);

            await UploadJsonToDropbox(localFilePath, dropboxFolderPath);
        }

        private async Task UploadJsonToDropbox(string localFilePath, string dropboxFolderPath)
        {
            try
            {
                // Ensure the dropboxFilePath includes the folder path
                string dropboxFilePath = $"{dropboxFolderPath}/{Path.GetFileName(localFilePath)}"; // Path in Dropbox
                using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var memStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memStream);
                        memStream.Position = 0;

                        await _dbx.Files.UploadAsync(
                            dropboxFilePath,
                            WriteMode.Overwrite.Instance,
                            body: memStream);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to upload JSON to Dropbox: {ex.Message}", "OK");
            }
        }

        private void NotificationButton_Clicked(object sender, EventArgs e)
        {
            // Handle the button click event here
            Navigation.PushAsync(new NotificationPage(_currentUser));
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            // Handle the button click event here
            Navigation.PushAsync(new SettingsPage(_currentUser));
        }

        private async Task DownloadJsonFromDropbox(string filename, string dropboxFolderPath)
        {
            string localFilePath = Path.Combine(FileSystem.AppDataDirectory, filename);
            try
            {
                var response = await _dbx.Files.DownloadAsync($"{dropboxFolderPath}/{filename}");
                using (var fileContentStream = await response.GetContentAsStreamAsync())
                {
                    using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileContentStream.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to download JSON from Dropbox: {ex.Message}", "OK");
            }
        }
    }
}
