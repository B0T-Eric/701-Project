using ArcheryLibrary;

namespace ArcheryProjectApp
{
    public partial class App : Application
    {
        public static readonly Target[] targets = {new Target(new List<int>([3,4,5]),"IFAA Field 4 x 20 cm", "Resources\\Images\\Targets\\IFAA_Field_4_x_20.jpg") };
        public static readonly int[] endCountOptions = { 3, 4, 5, 6, 7, 8, 9 };
        public static readonly int[] arrowCountOptions = { 3, 4, 5, 6, 7, 8, 9 };
        public static readonly ShootingPosition[] positionOptions = { ShootingPosition.Stationary, ShootingPosition.WalkUp, ShootingPosition.WalkBack };
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
