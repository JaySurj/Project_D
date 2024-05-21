using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Project_D
{
    public class SignupData
    {
        public string Fullname { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public static class SignupManager
    {
        private static readonly LocalDbService _dbService = new LocalDbService();

        public static async Task SaveSignupData(User user)
        {
            await _dbService.Create(user);
        }

        public static async Task<User> GetUserByEmail(string email)
        {
            var users = await _dbService.GetUsers();
            return users.FirstOrDefault(u => u.Email == email);
        }
    }

    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }

        private async void Signup(object sender, EventArgs e)
        {
            User user = new User
            {
                Fullname = FullnameEntry.Text,
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text
            };

            if (string.IsNullOrWhiteSpace(user.Fullname) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            // Check if email already exists
            var existingUser = await SignupManager.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                await DisplayAlert("Error", "Email already exists", "OK");
                return;
            }

            // Check for valid email format'
            var emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            var emailRegex = new Regex(emailPattern);
            if (!emailRegex.IsMatch(user.Email))
            {
                await DisplayAlert("Error", "Invalid email format", "OK");
                return;
            }
           

            await SignupManager.SaveSignupData(user);

            FullnameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;

            DisplayAlert("Success", "Signup successful!", "OK");

            //  navigate to a success page/ home page

            Navigation.PushAsync(new HomePage());



            // Navigate to a success page/ home page
            await Navigation.PushAsync(new SettingsPage(user));
        }
    }
}
