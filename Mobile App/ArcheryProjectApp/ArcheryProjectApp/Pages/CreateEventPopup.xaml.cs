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
		if (!ProfilePage.UserInstance.isGuest)
		{
			DivisionPicker.SelectedItem = ProfilePage.UserInstance.division;
			DivisionPicker.IsEnabled = false;
		}
		WeatherEditor.IsEnabled = false;
		WeatherEditor.IsVisible = false;
	}
	private async void OnSaveButtonClicked(object sender, EventArgs e)
	{
		if(!String.IsNullOrEmpty(EventNameEditor.Text) && !String.IsNullOrEmpty(EventDescriptionEditor.Text) && !String.IsNullOrEmpty(EventTypePicker.SelectedItem as string) && !String.IsNullOrEmpty(DivisionPicker.SelectedItem as string) && !String.IsNullOrEmpty(EnvironmentPicker.SelectedItem as string))
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
            if (eventDescription == "")
            {
                eventDescription = "No Description";
            }
            newEvent = new Event(eventName, eventDescription, eventType, eventDate, roundCount, environment, weather, division);
            ProfilePage.UserInstance.Events.Add(newEvent);

            RoundCreated?.Invoke(newEvent);
            Console.WriteLine("Saved EVENT!");
            Close();
        }
		else
		{
			await Application.Current.MainPage.DisplayAlert("Incomplete", "Please fill out all fields!", "Return");
		}
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