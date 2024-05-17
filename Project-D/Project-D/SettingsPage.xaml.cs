using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Project_D
{
    public partial class SettingsPage : ContentPage
    {

        private SignupData _data;
        public SettingsPage(SignupData data)
        {
            InitializeComponent();
            _data = data;
            DisplayUserData(data);
        }



        private void DisplayUserData(SignupData data)
        {
            FullnameLabel.Text = data.Fullname;
            EmailLabel.Text = data.Email;
        }

        private void Logout(object sender, EventArgs e)
        {
            // Handle logout logic here
            Navigation.PopToRootAsync(); // Example: Go back to the main page
        }

        private void Account(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AccountPage(_data));
        }


        
}
}
