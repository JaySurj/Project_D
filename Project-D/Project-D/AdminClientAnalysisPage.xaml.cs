using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class AdminClientAnalysisPage : ContentPage
    {
        public AdminClientAnalysisPage(User user)
        {
            InitializeComponent();
            // Set the user details to the labels
            FullNameLabel.Text = user.Fullname;
            EmailLabel.Text = user.Email;
            PasswordLabel.Text = user.Password;
        }
    }
}

