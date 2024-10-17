namespace ArcheryProjectApp.Pages;
using ArcheryLibrary;
using CommunityToolkit.Maui.Views;

public partial class EventViewPopup : Popup
{
	Event _event;
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
	}
	public async void OnDeleteClicked(object sender, EventArgs e)
	{
		//remove event from database
		if (_event != null)
		{
            ProfilePage.UserInstance.Events.Remove(_event);
            await App.dbService.RemoveEventFromDatabase(_event.Id);
            instance.DisplayItems();
			//needs remove function for api
        }
	}
	public void OnCancelClick(object sender, EventArgs e)
	{
		Close();
	}
}