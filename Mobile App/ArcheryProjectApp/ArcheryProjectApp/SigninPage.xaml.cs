namespace ArcheryProjectApp;

public partial class SigninPage : ContentPage
{
	public SigninPage()
	{
		InitializeComponent();
	}

    private async void OnSigninClick(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("///Main");
    }
	private async void OnClubSignUpClick(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///SignUp");
	}
	private async void OnAltSignUpClick(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///SignUpAlt");
	}
}