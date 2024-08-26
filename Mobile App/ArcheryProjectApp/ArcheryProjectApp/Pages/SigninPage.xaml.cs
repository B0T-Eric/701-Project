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
    }
	private async void OnClubSignUpClick(object sender, EventArgs e)
	{
		
	}
	private async void GuestSignInClick(object sender, EventArgs e)
	{
		LoginType loginType = LoginType.Guest;
		try
		{
            await Shell.Current.GoToAsync($"///Main?loginType={loginType}");
        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + "Navigation Failure");
		}
		
	}
}