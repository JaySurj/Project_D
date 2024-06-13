using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class OptionsPage : ContentPage
    {
        private User _currentUser;

        public LocalDbService _dbService = new LocalDbService();
        private string _selectedImage;
        private string _selectedSound;
        private List<string> afbeeldingen = new List<string> { "afbeelding_1", "afbeelding_2", "afbeelding_3" };
        private List<string> geluiden = new List<string> { "muziek_1", "muziek_2", "muziek_3" };

        public string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
                OnPropertyChanged(nameof(SelectedImageText));
            }
        }

        public string SelectedSound
        {
            get => _selectedSound;
            set
            {
                _selectedSound = value;
                OnPropertyChanged(nameof(SelectedSound));
                OnPropertyChanged(nameof(SelectedSoundText));
            }
        }

        public string SelectedImageText => $"You have selected Image: {SelectedImage}";
        public string SelectedSoundText => $"You have selected Sound: {SelectedSound}";

        public OptionsPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            BindingContext = this;
        }

        private void Select_Image(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            int buttonNumber = int.Parse((string)button.CommandParameter);
            SelectedImage = afbeeldingen[buttonNumber];
        }

        private void Select_Sound(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int buttonNumber = int.Parse((string)button.CommandParameter);
            SelectedSound = geluiden[buttonNumber];
        }

        private async void Save_Selection(object sender, EventArgs e)
        {
            try
            {
                var quote = QuoteEntry.Text;

                Preferences.Set("ImagePath", SelectedImage);
                Preferences.Set("SoundPath", SelectedSound);
                Preferences.Set("Quote", quote);

                _currentUser.Fullname = _currentUser.Fullname;
                _currentUser.Email = _currentUser.Email;
                _currentUser.Password = _currentUser.Password;
                _currentUser.Quote = quote;
                _currentUser.Image = SelectedImage;
                _currentUser.Sound = SelectedSound;

                await _dbService.Update(_currentUser);
                DisplayAlert("Success", "Your settings have been saved", "OK");
            }
             catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            
        }
    }
}
