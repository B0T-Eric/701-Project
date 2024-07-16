namespace ArcheryProjectApp;

public partial class SignUpAltPage : ContentPage
{
	public SignUpAltPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///Main");
    }
}