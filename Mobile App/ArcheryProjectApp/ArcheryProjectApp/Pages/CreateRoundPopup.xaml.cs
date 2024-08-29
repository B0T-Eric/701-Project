namespace ArcheryProjectApp.Pages;

using ArcheryLibrary;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;


public partial class CreateRoundPopup : Popup
{
	public event Action<Event> RoundCreated;
	public CreateRoundPopup()
	{
		InitializeComponent();
	}
	private async void OnSaveButtonClicked(object sender, EventArgs e)
	{
		Event newEvent;
		ScoreCard newScoreCard;
		string eventName = EventNameEditor.Text;
		string eventDescription = EventDescriptionEditor.Text;
		string eventType = EventTypePicker.SelectedItem as string;
		DateOnly eventDate = DateOnly.FromDateTime(EventDatePicker.Date);
		int roundCount = (int)(short)RoundCountPicker.SelectedItem;
		string division = DivisionPicker.SelectedItem as string;
		string environment = EnvironmentPicker.SelectedItem as string;
		string weather = null;
		if (environment.Equals("Outdoor") && !environment.Equals(null))
		{
			weather = WeatherEditor.Text;
		}

		if (eventType == null || eventDate.Equals(null) || eventName == null || roundCount.Equals(-1) || division.Equals(null) || environment.Equals(null))
        {
			Toast.Make("Please populate all required fields",duration:CommunityToolkit.Maui.Core.ToastDuration.Short);
        }
        else
        {
			if(eventDescription == "")
			{
				eventDescription = "No Description";
			}
			newScoreCard = new ScoreCard(roundCount, environment, weather, division);
            newEvent = new Event(eventName, eventDescription,eventType,eventDate);
			newEvent.ScoreCard = newScoreCard;
            ProfilePage.UserInstance.Events.Add(newEvent);
			
			RoundCreated?.Invoke(newEvent);
            Console.WriteLine("Saved EVENT!");
        }

		Close();
        
	}
	private async void OnCancelButtonClicked(object sender, EventArgs e)
	{
		Close();
	}
	private void OnEnvironmentPickerIndexChanged(object sender, EventArgs e)
	{
		var picker = (Picker)sender;
		int selectedIndex = picker.SelectedIndex;
		if (selectedIndex != -1)
		{
			if (selectedIndex == 1)
			{
				WeatherEditor.IsEnabled = false;
				WeatherEditor.IsVisible = false;
			}
			else
			{
				WeatherEditor.IsEnabled = true;
				WeatherEditor.IsVisible = true;
			}
		}
	}
}