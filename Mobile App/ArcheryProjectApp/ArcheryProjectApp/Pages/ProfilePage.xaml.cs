using ArcheryLibrary;
using System.Collections.ObjectModel;
using static ArcheryProjectApp.SigninPage;

namespace ArcheryProjectApp;

//To Do: Fix Guest Logging In Parameters, Add Method for accepting event
public partial class ProfilePage : ContentPage
{ 
    public static User UserInstance;

    private ObservableCollection<EventItemModel> eventItems = new ObservableCollection<EventItemModel>();
    public ProfilePage()
    {
        InitializeComponent();
        EventsCollectionView.ItemsSource = eventItems;
        BindingContext = this;
        if (UserInstance != null)
        {
            if(UserInstance.isGuest)
            {
                ProfileNameLabel.Text = "Guest";
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

    private void FetchEventsFromAPI()
    {
        //Do API call for this users 
        Event newEvent = new Event("Test", "Test", "Practice", new DateOnly(2024, 12, 2));
        ScoreCard scoreCard = new ScoreCard(3, "Outdoor", "3km/h E", "JMHB");
        newEvent.ScoreCard = scoreCard;
        EventItemModel eventItemModel = new EventItemModel(newEvent.Name, newEvent.Date, newEvent.Type, newEvent);
        
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
}