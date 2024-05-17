namespace Project_D
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void LogIn(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }

        private void SignUp(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignupPage());
        }
    }

}
