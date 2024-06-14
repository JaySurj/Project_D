using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class NotificationPage : ContentPage
    {
        private User _currentUser;
        private DropboxClient _dbx;

        public NotificationPage(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _dbx = DropboxClientFactory.GetClient();
            LoadLatestAverageBPMNotification();
            LoadHighBPMNotifications();
        }

        private async Task LoadNotificationsAsync(string notificationType)
        {
            await DownloadJsonFromDropbox("user_notification_data.json", "/Json");
            string localFilePath = Path.Combine(FileSystem.AppDataDirectory, "user_notification_data.json");

            if (File.Exists(localFilePath))
            {
                try
                {
                    string json = File.ReadAllText(localFilePath);

                    var notifications = System.Text.Json.JsonSerializer.Deserialize<List<UserNotification>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (notifications != null)
                    {
                        var userNotifications = notifications
                            .Where(n => n.UserId == _currentUser.Id && n.ShowNotification && n.NotificationType == notificationType)
                            .OrderByDescending(n => n.NotificationId) // Assuming NotificationId is a GUID
                            .ToList();

                        if (notificationType == "AverageBPM")
                        {
                            var latestNotification = userNotifications.FirstOrDefault();
                            if (latestNotification != null)
                            {
                                AddNotification(latestNotification);
                            }
                        }
                        else
                        {
                            AddNotifications(userNotifications);
                        }
                    }
                }
                catch (JsonException ex)
                {
                    await DisplayAlert("Error", $"Failed to deserialize JSON: {ex.Message}", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", $"File does not exist at {localFilePath}", "OK");
            }
        }

        public void AddNotifications(List<UserNotification> notifications)
        {
            foreach (var notification in notifications)
            {
                AddNotification(notification);
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


        public void AddNotification(UserNotification notification)
        {
            Color _bgc;

            if (notification.NotificationType == "HighBPM")
            {
                _bgc = Color.FromRgb(255, 171, 175);
            }
            else
            {
                _bgc = Colors.White;
            }

            var frame = new Frame
            {
                BorderColor = Colors.Gray,
                CornerRadius = 5,
                BackgroundColor = _bgc,
                Margin = new Thickness(10),
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label { Text = notification.NotificationType == "HighBPM" ? "High BPM Alert" : "BPM Data", FontAttributes = FontAttributes.Bold },
                        new Label { Text = notification.NotificationType == "HighBPM" ? $"BPM at {notification.Time} on {notification.Date}: {notification.AvgBPM}" : $"Today's average BPM: {notification.AvgBPM}" }
                    }
                }
            };

            NotificationLayout.Children.Add(frame);
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

        private async void LoadLatestAverageBPMNotification()
        {
            await LoadNotificationsAsync("AverageBPM");
        }

        private async void LoadHighBPMNotifications()
        {
            await LoadNotificationsAsync("HighBPM");
        }
    }

    public class UserNotification
    {
        public int UserId { get; set; }
        public int AvgBPM { get; set; }
        public string NotificationId { get; set; }
        public bool ShowNotification { get; set; }
        public string NotificationType { get; set; } // New property for notification type

        public string Date { get; set; } // Optional: For date-specific notifications
        public string Time { get; set; } // Optional: For time-specific notifications
    }
}
