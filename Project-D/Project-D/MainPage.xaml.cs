namespace Project_D
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

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
//            Navigation.PushAsync(new RegisterPage());
        }
    }

}
