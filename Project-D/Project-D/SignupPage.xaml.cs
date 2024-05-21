using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
        private static readonly string SignupFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "signup_data.json");

        public static void SaveSignupData(SignupData data)
        {
            List<SignupData> existingData = new List<SignupData>();
            if (File.Exists(SignupFilePath))
            {
                string jsonData = File.ReadAllText(SignupFilePath);
                if (!string.IsNullOrWhiteSpace(jsonData))
                {
                    try
                    {
                        existingData = JsonSerializer.Deserialize<List<SignupData>>(jsonData);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Failed to deserialize existing signup data: {ex.Message}");
                    }
                }
            }

            existingData.Add(data);
            string updatedJsonData = JsonSerializer.Serialize(existingData);
            File.WriteAllText(SignupFilePath, updatedJsonData);


        }
    }

    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }

        private void Signup(object sender, EventArgs e)
        {
            SignupData data = new SignupData
            {
                Fullname = FullnameEntry.Text,
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text
            };

            if (string.IsNullOrWhiteSpace(data.Fullname) || string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Password))
            {
                DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }
            SignupManager.SaveSignupData(data);

            FullnameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;

            DisplayAlert("Success", "Signup successful!", "OK");

            //  navigate to a success page/ home page

            Navigation.PushAsync(new HomePage());



        }

    }
}
