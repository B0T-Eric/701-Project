using ArcheryLibrary;
using ArcheryProjectApp.Data;

namespace ArcheryProjectApp
{
    public partial class App : Application
    {
        //Constants for the entire application - use App.constant to refer to them e.g. App.targets[1] for IFAA Field 4 x 20 cm target.
        public static readonly Target[] targets = { new Target(new List<int>([3, 4, 5]), "IFAA Field 4 x 20 cm", new Image { Source = "ifaa_field_4_x_20.jpg" } )};
        public static LocalDbService dbService = new LocalDbService();
        public static readonly int[] endCountOptions = { 3, 4, 5, 6, 7, 8, 9 };
        public static readonly int[] arrowCountOptions = { 3, 4, 5, 6, 7, 8, 9 };
        //for some reason this is required for pickers.
        public static readonly ShootingPosition[] positionOptions = { ShootingPosition.Stationary, ShootingPosition.WalkUp, ShootingPosition.WalkBack };
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
