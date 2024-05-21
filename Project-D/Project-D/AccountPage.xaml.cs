using System;
using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class AccountPage : ContentPage
    {
        private User _user;
        private LocalDbService _dbService;

        public AccountPage(User user)
        {
            InitializeComponent();
            _user = user;
            _dbService = new LocalDbService(); // Initialize your local database service
            DisplayUserData(_user);
        }

        private void DisplayUserData(User user)
        {
            FullnameEntry.Text = user.Fullname;
            EmailEntry.Text = user.Email;
            PasswordEntry.Text = user.Password; // Assuming Password is also passed. Ensure to handle passwords securely.
        }

        private async void UpdateAccount(object sender, EventArgs e)
        {
            // Update the account data here
            _user.Fullname = FullnameEntry.Text;
            _user.Email = EmailEntry.Text;
            _user.Password = PasswordEntry.Text;

            // Update the data in your database
            await _dbService.Update(_user);

            DisplayAlert("Success", "Account updated successfully!", "OK");
        }

        private void LogOutTapped(object sender, EventArgs e)
        {
            // Handle log out logic here
            DisplayAlert("Logged Out", "You have been logged out.", "OK");
            // Navigate to login page or main page
        }
    }
}
