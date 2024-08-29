namespace ArcheryProjectApp;
using ArcheryLibrary;
public partial class SigninPage : ContentPage
{
	public enum LoginType {Guest, RegisteredUser};
	public SigninPage()
	{
		InitializeComponent();
	}

    private async void OnSigninClick(object sender, EventArgs e)
    {
		LoginType loginType = LoginType.RegisteredUser;
        await Shell.Current.GoToAsync($"///Main");
    }
	private async void OnClubSignUpClick(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SignUpPage());
	}
	private async void GuestSignInClick(object sender, EventArgs e)
	{
		LoginType loginType = LoginType.Guest;
		string loginTypeString = loginType.ToString();
        await Shell.Current.GoToAsync($"///Main?loginType={loginTypeString}");
    }
}