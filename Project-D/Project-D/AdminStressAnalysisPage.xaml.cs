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
                var users = await DatabaseHelper.GetUsersAsync(dbPath);
                UsersListView.ItemsSource = users;
            }

            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
            }
        }
    }
}