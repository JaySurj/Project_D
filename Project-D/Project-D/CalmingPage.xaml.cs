namespace Project_D;

public partial class CalmingPage : ContentPage
{
	public CalmingPage()
	{
		InitializeComponent();
	}

	private void AreYouOKButton_Clicked(object sender, EventArgs e)
	{
        DisplayAlert("Are you okay?", "You are not alone. We are here for you.", "OK");
    }
	private void HelpRelaxButton_Clicked(object sender, EventArgs e)
	{
        DisplayAlert("Relaxation Techniques", "Take a deep breath. Inhale for 4 seconds, hold for 4 seconds, exhale for 4 seconds. Repeat.", "OK");
    }
}