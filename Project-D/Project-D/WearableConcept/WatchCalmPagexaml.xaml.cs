namespace Project_D.WearableConcept;

public partial class WatchCalmPagexaml : ContentPage
{
	public WatchCalmPagexaml()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        InitializeComponent();
	}
    private async void Backbuttoncalm(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}