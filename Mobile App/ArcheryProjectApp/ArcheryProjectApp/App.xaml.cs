using ArcheryLibrary;
using ArcheryProjectApp.Data;

namespace ArcheryProjectApp
{
    public partial class App : Application
    {
        //Constants for the entire application - use App.constant to refer to them e.g. App.targets[1] for IFAA Field 4 x 20 cm target.
        public static readonly Target[] targets = { 
            new Target(new List<int>{3,4,5}, "IFAA Field 4 x 20 cm", new Image { Source = "ifaa_field_4_x_20.jpg" }),
            new Target(new List<int>{3,4,5}, "IFAA Field 35 cm", new Image { Source = "ifaa_field_35.jpg" }),
            new Target(new List<int>{3,4,5}, "IFAA Field 50 cm", new Image { Source = "ifaa_field_50.jpg" }),
            new Target(new List<int>{4,5}, "Maple Leaf 40 cm 5-spot", new Image { Source = "maple_leaf_40cm_5_spot.jpg" }),
            new Target(new List<int>{1,2,3,4,5}, "Maple Leaf 40 cm Single Spot", new Image { Source = "maple_leaf_40cm_single_spot.jpeg" }),
            new Target(new List<int>{6,7,8,9,10}, "Fita 3 x 20 cm Las Vegas", new Image { Source = "fita_3_x_20cm_las_vegas.jpg" }),
            new Target(new List<int>{6,7,8,9,10}, "Fita 3 x 20 cm Vertical", new Image { Source = "fita_3_x_20cm_vertical.jpg" }),
            new Target(new List<int>{1,2,3,4,5,6,7,8,9,10}, "Fita 40 cm Target Faces", new Image { Source = "fita_40cm_target_faces.jpg" }),
            new Target(new List<int>{5,6,7,8,9,10}, "Fita 80 cm Target Faces", new Image { Source = "fita_80cm_target_faces.jpg" }),
            new Target(new List<int>{1,2,3,4,5,6,7,8,9,10}, "Fita 122 cm Target Faces", new Image { Source = "fita_122cm_target_faces.jpg" })
        };
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
