using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace Project_D
{
    public partial class HomePage : ContentPage, INotifyPropertyChanged
    {
        private int _buttonHeight = 50; // Initial button height

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

        public HomePage()
        {
            InitializeComponent();
            BindingContext = this; // Set the BindingContext to the instance of HomePage
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
            // Handle button click
        }

        private void OnButton2Clicked(object sender, EventArgs e)
        {
            // Handle button click
        }

        private void OnButton3Clicked(object sender, EventArgs e)
        {
            // Handle button click
        }

        private void OnButton4Clicked(object sender, EventArgs e)
        {
            // Handle button click
        }
    }
}