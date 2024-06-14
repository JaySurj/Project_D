using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace Project_D.WearableConcept
{
    public partial class WatchHomePage : ContentPage
    {

        public WatchHomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }
        private async void OnBpmButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WatchBpmPage());
        }

        private async void OnCalmingButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WatchCalmPagexaml());
        }

        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WatchSportModusPage());
        }
    }
}
