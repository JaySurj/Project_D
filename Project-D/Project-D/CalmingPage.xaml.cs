namespace Project_D;

public partial class CalmingPage : ContentPage
{
    public string ImageSource { get; set; }

    public CalmingPage(User user)
    {
        InitializeComponent();
        BindingContext = this;
        LoadUserData(user);
    }

    private void LoadUserData(User user)
    {
        ImageSource = user.Image;
        UserQuote.Text = user.Quote;
        OnPropertyChanged(nameof(ImageSource));
    }

    private void AreYouOKButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Inademen. Uitademen", "Alles komt goed", "OK");
    }

    private void HelpRelaxButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Take a Deep Breath", "In this moment, remember that you are safe. Everything is going to be okay. Close your eyes if you feel comfortable, and focus on your breathing. Breathe in slowly through your nose for four seconds, hold for four seconds, and then gently exhale through your mouth for four seconds. Repeat this rhythm.", "OK");
    }
}
