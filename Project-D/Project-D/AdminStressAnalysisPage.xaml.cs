using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class AdminStressAnalysisPage : ContentPage
    {
        private User _user;

        public AdminStressAnalysisPage()
        {
            InitializeComponent();
            LoadUsers();

            // Subscribe to item selected event
            UsersListView.ItemSelected += OnItemSelected;
        }

        private async void LoadUsers()
        {
            try
            {
                string dropboxFilePath = "/Project_D/app.db";
                string localFileName = "app.db";

                var dbPath = await DropboxHelper.DownloadDatabaseAsync(dropboxFilePath, localFileName);
                Console.WriteLine($"Database path: {dbPath}");

                var users = await DatabaseHelper.GetUsersAsync(dbPath);

                if (users != null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        Console.WriteLine($"User: {user.Fullname}, Email: {user.Email}");
                    }

                    UsersListView.ItemsSource = users;
                }
                else
                {
                    Console.WriteLine("No users found in the database.");
                    await DisplayAlert("Info", "No users found in the database.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is User selectedUser)
            {
                // Deselect the item
                UsersListView.SelectedItem = null;

                // Navigate to AdminClientAnalysisPage
                await Navigation.PushAsync(new AdminClientAnalysisPage(selectedUser));
            }
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage(_user));
        }
    }
}
