namespace ArcheryProjectApp.Pages;
using ArcheryLibrary;
using CommunityToolkit.Maui.Views;

public partial class EventViewPopup : Popup
{
	Event? _event;
	RoundsPage instance;
	public EventViewPopup(Event _event, RoundsPage instance)
	{
        InitializeComponent();
		this.instance = instance;
		this._event = _event;
		DisplayDetails();
	}
	public void DisplayDetails()
	{
		if (_event != null)
		{
			EventNameText.Text = _event.Name;
			EventDescriptionText.Text = _event.Description;
			EventDateText.Text = DateOnly.FromDateTime(_event.Date).ToString();
			EventRoundCountText.Text = _event.RoundCount.ToString();
			EventEnvironmentText.Text = _event.Environment.ToString();
			if(_event.Weather != null) 
			{
				EventWeatherText.Text = _event.Weather.ToString();
			}
			else
			{
				WeatherLabel.IsVisible = false;
				EventWeatherText.IsVisible = false;
			}
		}
		else
		{
            EventNameText.Text = "   ";
            EventDescriptionText.Text = "    ";
            EventDateText.Text = "   ";
            EventRoundCountText.Text = "   ";
            EventEnvironmentText.Text = "    ";
			EventWeatherText.Text = "   ";
			DeleteEventButton.IsVisible = false;
			DeleteEventButton.IsEnabled = false;
        }
	}
	public async void OnDeleteClicked(object sender, EventArgs e)
	{
		if(_event != null)
		{
            //notify check for delete
            bool result = await Application.Current.MainPage.DisplayAlert("Delete Event", "Are you sure you want to delete this event?", "Yes", "No");
            if (result)
            {
                //remove event from database
                ProfilePage.UserInstance.Events.Remove(_event);
                await App.dbService.RemoveEventFromDatabase(_event.Id);
                _event = null;
				DisplayDetails();
				instance.DisplayItems();


                //needs remove function for api

            }
        }
		
	}
	public void OnCancelClick(object sender, EventArgs e)
	{
		Close();
	}
}