using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class AdminStressAnalysisPage : ContentPage
    {
        public AdminStressAnalysisPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            try
            {
                var dbPath = await DropboxHelper.DownloadDatabaseAsync();
                Console.WriteLine($"Database path: {dbPath}");
                var users = await DatabaseHelper.GetUsersAsync(dbPath);

                if (users != null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        Console.WriteLine($"User: {user.Fullname}, Email: {user.Email}");
                    }

                    // Set the ItemsSource for the ListView
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
    }
}


