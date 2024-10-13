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
		
		if (!GuestInstanceExists("Guest")) //adds guest user to database if guest doesn't already exist
		{
            ProfilePage.UserInstance = new User();
            await App.dbService.CreateUserAuth(new Data.UserAuth
            {
                Username = "Guest",
                Id = 1,
                Password = "Guest",
                Salt = null
            });
			await App.dbService.AddUserDetailsToDatabase(ProfilePage.UserInstance, 1);
			
        }
		else //get any data for guest user.
		{
			var userAuth = await App.dbService.GetUserByName("Guest");
			var userDetail = await App.dbService.GetUserDetail(userAuth.Id);
			ProfilePage.UserInstance = new User
			{
				AuthId = userAuth.Id,
				DetailId = userDetail.Id,
				isGuest = true,
				ArcherName = userDetail.FirstName + " " + userDetail.LastName,
				ClubName = userDetail.ClubName,
				NZFAANumber = userDetail.NzfaaNumber,
				AffiliationNumber = userDetail.ClubNumber,
				DateOfBirth = userDetail.DateOfBirth,
				division = userDetail.Division,
			};

			ProfilePage.UserInstance.Events = await App.dbService.GetUserEvents(ProfilePage.UserInstance.Id);
			foreach (var _event in ProfilePage.UserInstance.Events)
			{
				_event.Rounds = await App.dbService.GetRoundsFromDatabase(_event.Id);
				foreach(var round in _event.Rounds)
				{
					round.Ends = await App.dbService.RetrieveEndsFromDatabase(round.Id);
				}
			}
		}
		
        await Shell.Current.GoToAsync($"///Main");
    }
	private bool GuestInstanceExists(string username)
	{
		if (App.dbService.GetUserByName(username) != null)
		{
            return true;
        }
		else
		{
			return false;
		}
	}
}