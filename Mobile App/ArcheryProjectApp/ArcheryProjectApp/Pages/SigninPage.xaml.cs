namespace ArcheryProjectApp;
using ArcheryLibrary;
public partial class SigninPage : ContentPage
{
	public SigninPage()
	{
		InitializeComponent();
	}

    private async void OnSigninClick(object sender, EventArgs e)
    {
		
    }
	private async void OnClubSignUpClick(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SignUpPage());
	}
	private async void GuestSignInClick(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new ProfilePage(new User()));
	}
}