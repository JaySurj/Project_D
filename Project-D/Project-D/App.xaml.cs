using Project_D.WearableConcept;

namespace Project_D
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (DeviceInfo.Idiom == DeviceIdiom.Watch)
            {
                MainPage = new NavigationPage(new WatchHomePage());
            }
            else
            {
                MainPage = new AppShell();
                //MainPage = new NavigationPage(new AdminStressAnalysisPage());
            }
        }
    }
}
