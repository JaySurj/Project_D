namespace Project_D.WearableConcept
{
    public partial class WatchSportModusPage : ContentPage
    {
        // Variable to store the state of Sport Mode
        private bool isSportModeOn = false;

        public WatchSportModusPage()
        {
            InitializeComponent();

            // Load saved state of Sport Mode if previously turned on
            SportModus.IsToggled = isSportModeOn;

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Backbuttonsport(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void SportModus_Toggled(object sender, ToggledEventArgs e)
        {
            // Update state variable when Sport Mode switch is toggled
            isSportModeOn = e.Value;
        }
    }
}
