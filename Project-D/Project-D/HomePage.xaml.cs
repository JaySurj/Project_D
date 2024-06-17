using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class HomePage : ContentPage, INotifyPropertyChanged
    {
        private int _buttonHeight = 50; // Initial button height
        private NotificationPage _notificationPage;

        public int ButtonHeight
        {
            get { return _buttonHeight; }
            set
            {
                if (_buttonHeight != value)
                {
                    _buttonHeight = value;
                    OnPropertyChanged(nameof(ButtonHeight));
                }
            }
        }

        private string _greetingText;
        public string GreetingText
        {
            get { return _greetingText; }
            set
            {
                if (_greetingText != value)
                {
                    _greetingText = value;
                    OnPropertyChanged(nameof(GreetingText));
                }
            }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        public HomePage(User user)
        {
            InitializeComponent();
            _user = user;
            _notificationPage = new NotificationPage(user); // Initialize NotificationPage
            GreetingText = _user.Fullname;

            BindingContext = this; // Set the BindingContext to the instance of HomePage
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage(_user));
        }

        private void NotificationButton_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new NotificationPage(_user));
        }

        private void ChartButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChartPage());
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Event handlers for button clicks
        private void OnButton1Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BPMPage(_user));
        }

        private void OnButton2Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChartPage());
        }

        private void OnButton3Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotificationPage(_user));
        }
        private void OnButton4Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OptionsPage(_user));
        }
        private void OnButton5Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CalmingPage(_user));
        }
    }
}
