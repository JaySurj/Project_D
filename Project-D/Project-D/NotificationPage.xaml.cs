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

        public NotificationPage(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            LoadNotificationsAsync();
        }

        private async Task LoadNotificationsAsync()
        {
            DropboxAPIToken dropboxAPIToken = new DropboxAPIToken();
            string localFilePath = Path.Combine(FileSystem.AppDataDirectory, "user_notification_data.json");

            await dropboxAPIToken.DownloadFileFromDropbox("/Json", "user_notification_data.json", "user_notification_data.json");

            if (File.Exists(localFilePath))
            {
                try
                {
                    string json = File.ReadAllText(localFilePath);

                    List<UserNotification> notifications = new List<UserNotification>();

                    // Check if the JSON starts with an array or an object
                    if (json.Trim().StartsWith("["))
                    {
                        // JSON is an array
                        notifications = System.Text.Json.JsonSerializer.Deserialize<List<UserNotification>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    else
                    {
                        // JSON is a single object
                        var singleNotification = System.Text.Json.JsonSerializer.Deserialize<UserNotification>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        notifications.Add(singleNotification);
                    }

                    if (notifications != null)
                    {
                        var userNotifications = notifications.Where(n => n.UserId == _currentUser.Id && n.ShowNotification).ToList();
                        AddNotifications(userNotifications);
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
                var frame = new Frame
                {
                    BorderColor = Colors.Gray,
                    CornerRadius = 5,
                    Margin = new Thickness(10),
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new Label { Text = "BPM Data", FontAttributes = FontAttributes.Bold },
                            new Label { Text = $"Today's average BPM: {notification.AvgBPM}" }
                        }
                    }
                };

                NotificationLayout.Children.Add(frame);
            }
        }
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class UserNotification
    {
        public int UserId { get; set; }
        public int AvgBPM { get; set; }
        public string NotificationId { get; set; }
        public bool ShowNotification { get; set; }
    }
}
