using ArcheryLibrary;
using ArcheryProjectApp.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static ArcheryProjectApp.SigninPage;

namespace ArcheryProjectApp;

//To Do: Fix Guest Logging In Parameters, Add Method for accepting event
public partial class ProfilePage : ContentPage
{ 
    public static User UserInstance;
    private readonly LoginService loginService;
    public ReposUser _userRepository;

    private ObservableCollection<EventItemModel> eventItems = new ObservableCollection<EventItemModel>();
    public ProfilePage()
    {
        InitializeComponent();

        loginService = new LoginService();

        EventsCollectionView.ItemsSource = eventItems;
        BindingContext = this;
        if (UserInstance != null)
        {
            ProfileNameLabel.Text = UserInstance.ArcherName;
            if (UserInstance.isGuest)
            {
                _userRepository = new ReposUser();
                LoadUserProfile();
                ProfileNZFAALabel.Text = "To Get Access";
                ProfileClubLabel.Text = "Sign Up";
                ModifyButtonText();
                //temporarily use event call for testing
                FetchEventsFromAPI();
            }
            else
            {
                FetchEventsFromAPI();
            }
            
        }

        
    }
    private async void LoadUserProfile()
    {
        var token = await SecureStorage.GetAsync("token");
        if (string.IsNullOrEmpty(token))
        {
            await DisplayAlert("Error", "User is not authenticated.", "OK");
            return;
        }

        var userDetail = await _userRepository.GetUserProfileAsync(token);
        if (userDetail != null)
        {
            ProfileNameLabel.Text = $"{userDetail.FirstName} {userDetail.LastName}";
            ProfileNZFAALabel.Text = userDetail.NzfaaNumber.ToString();
            ProfileClubLabel.Text = userDetail.Name;
            ProfileDivisionLabel.Text = userDetail.Division;
            ProfileDOBLabel.Text = userDetail.DateOfBirth.ToString();
        }
        else
        {
            await DisplayAlert("Error", "Failed to load user profile.", "OK");
        }
    }

    private void FetchEventsFromAPI()
    {
        //Do API call for this users 
        Event newEvent = new Event("Test", "Test", "Practice", new DateTime(2024, 12, 2), 3, "Outdoor", "3km/h E", "JMHB");
        EventItemModel eventItemModel = new EventItemModel(newEvent.Name, newEvent.Date, newEvent.Type, newEvent.Environment, newEvent);
        eventItems.Add(eventItemModel);

    }

    private void ModifyButtonText()
    {
        ProfileButton.Text = "Sign-Up";
    }
    private async void ProfileButtonClicked(object sender, EventArgs e)
    {
        if (UserInstance.isGuest && UserInstance != null)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
    private void OnEventItemTapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var tappedItem = frame.BindingContext as EventItemModel;
        if(!UserInstance.Events.Contains(tappedItem.UserEvent))
        {
            UserInstance.Events.Add(tappedItem.UserEvent);
            DisplayAlert("Event Added", "Successfully Added Event", "Ok");
        }
        else
        {
            DisplayAlert("Event Exists", "Event Already Exists", "Ok");
        }

    }

    private async void OnSignOutClicked(object sender, EventArgs e)
    {
        try
        {
            await SecureStorage.SetAsync("token", string.Empty);
            Debug.WriteLine("Token cleared successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error clearing token: {ex.Message}");
        }

        await Shell.Current.GoToAsync("//SignIn");

        if (Shell.Current.CurrentPage is SigninPage signinPage)
        {
            signinPage.ClearFields();
        }
    }
}