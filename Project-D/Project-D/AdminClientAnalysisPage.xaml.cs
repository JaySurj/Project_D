using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class AdminClientAnalysisPage : ContentPage
    {
        private User _user;

        public AdminClientAnalysisPage(User user)
        {
            InitializeComponent();
            _user = user;
            // Set the user details to the labels
            FullNameLabel.Text = _user.Fullname;
            EmailLabel.Text = _user.Email;

        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage(_user));
        }
    }
}
