namespace ArcheryProjectApp.Pages;
using System.Collections.ObjectModel;
using ArcheryLibrary;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;


public partial class CreateEventPopup : Popup
{
	public event Action<Event> RoundCreated;
	public CreateEventPopup()
	{
		InitializeComponent();
	}
	private async void OnSaveButtonClicked(object sender, EventArgs e)
	{
		Event newEvent;
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
			
        }
        else
        {
			if(eventDescription == "")
			{
				eventDescription = "No Description";
			}
            newEvent = new Event(eventName, eventDescription,eventType,eventDate, roundCount, environment, weather, division);
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