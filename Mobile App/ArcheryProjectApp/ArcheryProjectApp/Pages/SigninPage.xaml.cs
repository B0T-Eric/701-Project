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
		//process user information (check for validity and verification from local db (if online check api aswell))
		ValidateUserCredentials();
        await Shell.Current.GoToAsync($"///Main");
    }

    private void ValidateUserCredentials()
    {
		//If user details are valid go to profile page, if not alert user to try again or register with their club credentials

    }

    private async void OnClubSignUpClick(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SignUpPage());
	}
	private async void GuestSignInClick(object sender, EventArgs e)
	{
		ProfilePage.UserInstance = new User();
        await Shell.Current.GoToAsync($"///Main");
    }
}