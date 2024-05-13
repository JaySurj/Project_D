namespace Project_D
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} times";

            else
                CounterBtn.Text = $"Clicked {count} times";

            Navigation.PushAsync(new LoginPage());
        }
    }

}
