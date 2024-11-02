namespace ArcheryProjectApp;
using ArcheryLibrary;
using ArcheryProjectApp.Data;
using System.Threading.Tasks;

public partial class SigninPage : ContentPage
{
    private readonly LoginService loginService;

    public SigninPage()
	{
		InitializeComponent();

        loginService = new LoginService();

		if(App.dbService == null)
		{
            App.dbService = new Data.LocalDbService();
        }
		
	}

    public void ClearFields()
    {
        usernameEditor.Text = string.Empty;
        passwordEditor.Text = string.Empty;
    }


    private async void OnSigninClick(object sender, EventArgs e)
    {
        //process user information (check for validity and verification from local db (if online check api aswell))
        string userName = usernameEditor.Text;
        string password = passwordEditor.Text;

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Input Error", "Please enter both username and password.", "OK");
            return;
        }

        ValidateUserCredentials(userName, password);
        //await Shell.Current.GoToAsync($"///Main");
    }

    private async void ValidateUserCredentials(string username, string password)
    {
        //If user details are valid go to profile page, if not alert user to try again or register with their club credentials
        try
        {
            var token = await loginService.LoginAsync(username, password);
            if (!string.IsNullOrEmpty(token))
            {
                ProfilePage.UserInstance = new User(username);
                await Shell.Current.GoToAsync($"//Main");
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid Username or Password. Please try again", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnClubSignUpClick(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SignUpPage());
	}
	private async void GuestSignInClick(object sender, EventArgs e)
	{
		
		if (await GuestInstanceExistsAsync("Guest") == false) //adds guest user to database if guest doesn't already exist
		{
            ProfilePage.UserInstance = new User();
            await App.dbService.CreateUserAuth(new Data.UserAuth
            {
                Username = "Guest",
                Password = "Guest",
                Salt = null
            });
			await App.dbService.AddUserDetailsToDatabase(ProfilePage.UserInstance, await App.dbService.GetUserAuthId("Guest"));
			
        }
		else //get any data for guest user.
		{
			var userAuth = await App.dbService.GetUserByName("Guest");
			var userDetail = await App.dbService.GetUserDetail(userAuth.Id);
			if(userDetail != null)
			{
                ProfilePage.UserInstance = new User
                {
                    AuthId = userAuth.Id,
                    DetailId = userDetail.Id,
                    isGuest = true,
                    ArcherName = userDetail.FirstName + " " + userDetail.LastName,
                    ClubName = userDetail.Name,
                    NZFAANumber = userDetail.NzfaaNumber,
                    AffiliationNumber = userDetail.ClubNumber,
                    DateOfBirth = userDetail.DateOfBirth,
                    division = userDetail.Division,
                };
            }
            else
            {
                ProfilePage.UserInstance = new User
                {
                    AuthId = userAuth.Id,
                    ArcherName = "Guest User",
                    ClubName = null,
                    NZFAANumber = null,
                    AffiliationNumber = null,
                    DateOfBirth = null,
                    division = null,
                };
                await App.dbService.AddUserDetailsToDatabase(ProfilePage.UserInstance, await App.dbService.GetUserAuthId("Guest"));
                ProfilePage.UserInstance.DetailId = await App.dbService.GetUserDetailId(ProfilePage.UserInstance.AuthId);
            }
			List<Event> events = await App.dbService.GetUserEvents(ProfilePage.UserInstance.AuthId);
			if(events != null)
			{
                ProfilePage.UserInstance.Events = events;
                foreach (var _event in ProfilePage.UserInstance.Events)
                {
                    _event.Rounds = await App.dbService.GetRoundsFromDatabase(_event.Id);
                    foreach (var round in _event.Rounds)
                    {
                        round.Ends = await App.dbService.RetrieveEndsFromDatabase(round.Id);
                        foreach(var end in round.Ends)
                        {
                            end.SetEndTotals(round.Target);
                        }
                        round.SetRoundTotals();
                    }
                }
            }
		}
		
        await Shell.Current.GoToAsync($"///Main");
    }
	private async Task<bool> GuestInstanceExistsAsync(string username)
	{
		if ( await App.dbService.GetUserByName(username) != null)
		{
            return true;
        }
		else
		{
			return false;
		}
	}
}