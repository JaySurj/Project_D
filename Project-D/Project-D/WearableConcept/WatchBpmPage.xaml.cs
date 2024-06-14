namespace Project_D.WearableConcept;

public partial class WatchBpmPage : ContentPage
{
    private static Random random = new Random();
    private int totalCount = 0;
    private int totalBpm = 0;
    private int highestBpm = 0;

    public WatchBpmPage()
    {
        InitializeComponent();
        StartBpmSimulation();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void StartBpmSimulation()
    {
        int highCount = 0;

        for (int i = 0; i < 10; i++)
        {
            int bpm = random.Next(80, 111);
            CurrentBpmLabel.Text = $"{bpm} BPM";

            // Bereken gemiddelde
            totalCount++;
            totalBpm += bpm;
            double averageBpm = (double)totalBpm / totalCount;

            // Update hoogste BPM
            if (bpm > highestBpm)
            {
                highestBpm = bpm;
            }

            // Controleer of hartslag hoog is
            if (bpm > 100)
            {
                highCount++;
                if (highCount >= 3)
                {
                    await DisplayAlert("             Neem even rust.", "Uw hartslag is hoger dan normaal", "kalmeren");
                    await Navigation.PushAsync(new WatchCalmPagexaml());
                    return;
                }
            }

            // Update labels
            PeakBpmLabel.Text = $"{highestBpm} BPM";
            AverageBpmLabel.Text = $"{Math.Round(averageBpm)} BPM";

            await Task.Delay(1000);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
