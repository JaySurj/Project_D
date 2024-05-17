using System;

namespace Project_D
{
    public partial class SettingsAccountPage : ContentPage
    {
        private SignupData _data;

        public SettingsAccountPage(SignupData data)
        {
            InitializeComponent();
            _data = data;
            DisplayUserData(data);
        }

        private void DisplayUserData(SignupData data)
        {
            FullnameEntry.Text = data.Fullname;
            EmailEntry.Text = data.Email;
            PasswordEntry.Text = data.Password; // Assuming Password is also passed. Ensure to handle passwords securely.
        }

        private void UpdateAccount(object sender, EventArgs e)
        {
            // Update the account data here
            _data.Fullname = FullnameEntry.Text;
            _data.Email = EmailEntry.Text;
            _data.Password = PasswordEntry.Text;

            // Add code to update the data in your backend or database.

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
