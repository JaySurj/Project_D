using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace Project_D
{
    public partial class LoginPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;

        public LoginPage()
        {
            InitializeComponent();
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db");
            _connection = new SQLiteAsyncConnection(databasePath);
        }

        public async void Login_Clicked(object sender, EventArgs e)
        {

            User user = new User
            {
                Fullname = usernameEntry.Text.Split("@")[0],
                Email = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            usernameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;

            await DisplayAlert("Success", "Login successful!", "OK");

            // Navigate to a success page/ home page
            await Navigation.PushAsync(new HomePage(user));
        }
    }
}
