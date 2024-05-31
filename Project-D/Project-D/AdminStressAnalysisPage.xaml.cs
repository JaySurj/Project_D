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

                foreach (var user in users)
                {
                    Console.WriteLine($"User: {user.FullName}, Email: {user.Email}");
                }

                UsersListView.ItemsSource = users;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public class User
    {
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}

