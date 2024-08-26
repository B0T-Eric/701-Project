using ArcheryLibrary;

namespace ArcheryProjectApp
{
    public partial class App : Application
    {
        public static readonly Target[] targets = {new Target() };
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new AppShell());
        }
    }
}
