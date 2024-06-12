using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class OptionsPage : ContentPage
    {
        private User _currentUser;

        public LocalDbService _dbService = new LocalDbService();
        private string afbeelding;
        private string geluid;
        private List<string> afbeeldingen = new List<string> { "afbeelding_1", "afbeelding_2", "afbeelding_3" };
        private List<string> geluiden = new List<string> { "muziek_1", "muziek_2", "muziek_3" };

        public OptionsPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            this.BindingContext = this;
        }

        private void Select_Image(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int buttonNumber = int.Parse((string)button.CommandParameter);

            switch (buttonNumber)
            {
                case 0:
                    DisplayAlert("Image Selection", "You selected image 1", "OK");
                    break;
                case 1:
                    DisplayAlert("Image Selection", "You selected image 2", "OK");
                    break;
                case 2:
                    DisplayAlert("Image Selection", "You selected image 3", "OK");
                    break;
            }
        }

        private void Select_Sound(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var index = int.Parse((string)button.CommandParameter);
            geluid = geluiden[index];
            DisplayAlert("Sound Selected", $"You selected {geluid}", "OK");
        }

        private async void Save_Selection(object sender, EventArgs e)
        {
            var quote = QuoteEntry.Text;

            Preferences.Set("ImagePath", afbeelding);
            Preferences.Set("SoundPath", geluid);
            Preferences.Set("Quote", quote);
            

            _currentUser.Fullname =  _currentUser.Fullname;
            _currentUser.Email =  _currentUser.Email;
            _currentUser.Password =  _currentUser.Password;
            _currentUser.Quote = "quote";
            _currentUser.Image = "afbeelding_1";
            _currentUser.Sound = "geluid_3";

            // Update the data in your database
            await _dbService.Update(_currentUser);
        }

        private void Display_Selection(object sender, EventArgs e)
        {
            afbeelding = Preferences.Get("ImagePath", "");
            geluid = Preferences.Get("SoundPath", "");
            var quote = Preferences.Get("Quote", "");
            DisplayAlert("Saved Selection", $"You saved:\nImage: {afbeelding}\nSound: {geluid}\nQuote: {quote}", "OK");
        }

        void Toggle_Image(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                DisplayAlert("Image Toggle", "Image functionality enabled", "OK");
            }
            else
            {
                DisplayAlert("Image Toggle", "Image functionality disabled", "OK");
            }
        }

        void Toggle_Sound(object sender, ToggledEventArgs e)
        {
            string a;   
            if (e.Value)
            {
                DisplayAlert("Sound Toggle", "Sound functionality enabled", "OK");
            }
            else
            {
                DisplayAlert("Sound Toggle", "Sound functionality disabled", "OK");
            }
        }

        void Toggle_Quote(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                DisplayAlert("Quote Toggle", "Quote functionality enabled", "OK");
            }
            else
            {
                DisplayAlert("Quote Toggle", "Quote functionality disabled", "OK");
            }
        }
    }
}
