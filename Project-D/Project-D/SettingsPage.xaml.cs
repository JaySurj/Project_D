using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Project_D
{
    public partial class SettingsPage : ContentPage
    {
        private User _user;

        public SettingsPage(User user)
        {
            InitializeComponent();
            _user = user;
            DisplayUserData(user);
        }

        private void DisplayUserData(User user)
        {
            FullnameLabel.Text = user.Fullname;
            EmailLabel.Text = user.Email;
        }

        private void Logout(object sender, EventArgs e)
        {
            // Handle logout logic here
            Navigation.PopToRootAsync(); // Example: Go back to the main page
        }

        private void Account(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AccountPage(_user));
        }

        private void Notifications(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotificationPage(_user));
        }

        private void Preferences(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OptionsPage(_user));
        }
    }
}
