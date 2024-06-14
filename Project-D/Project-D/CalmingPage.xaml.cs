namespace Project_D;

public partial class CalmingPage : ContentPage
{
	public CalmingPage()
	{
		InitializeComponent();
	}

	private void AreYouOKButton_Clicked(object sender, EventArgs e)
	{
        DisplayAlert("Inademen. Uitademen", "Alles komt goed", "OK");
    }
	private void HelpRelaxButton_Clicked(object sender, EventArgs e)
	{
        DisplayAlert("Ontspanningstechnieken", "Neem een diepe ademhaling. Adem in gedurende 4 seconden, houd vast gedurende 4 seconden, adem uit gedurende 4 seconden. Herhaal dit.", "OK");
    }
}