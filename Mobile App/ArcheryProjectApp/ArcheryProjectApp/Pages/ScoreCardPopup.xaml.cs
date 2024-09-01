namespace ArcheryProjectApp.Pages;
using CommunityToolkit.Maui.Views;
using ArcheryLibrary;
public partial class ScoreCardPopup : Popup
{
	private INavigation _navigation;
	private List<Round> rounds = new List<Round>();
	private Event _eventItem;
	private Round currentRound;
	private int current;
	public ScoreCardPopup(Event eventItem, INavigation navigation)
	{
		InitializeComponent();
		_navigation = navigation;
		_eventItem = eventItem;
		for (int i = 0; i < eventItem.ScoreCard.RoundCount; i++)
		{
			rounds.Add(new Round());
		}
		PreviousRoundButton.IsEnabled = false;
		PreviousRoundButton.IsVisible = false;
		currentRound = rounds[0];
		current = 0;
		UpdateDisplay();
	}
	private void OnEndSliderChange(object sender, ValueChangedEventArgs e)
	{
		var slider = (Slider)sender;
		slider.Value = Math.Round(e.NewValue);
		EndsLabel.Text = $"Ends: {EndSlider.Value}";
	}
	private void OnArrowSliderChange(object sender, ValueChangedEventArgs e)
	{
		var slider = (Slider)sender;
		slider.Value = Math.Round(e.NewValue);
		ArrowsLabel.Text = $"Arrows: {ArrowSlider.Value}";
	}
	private async void OnNextRoundClicked(object sender, EventArgs e)
	{
		//Load Round Data from next round, if current is equal to rounds.count turn off other wise on
		current++;
		currentRound = rounds[current];
		//Save Round Info
		SaveRound();
		//Load Round Info
		UpdateDisplay();
		if(current == rounds.Count())
		{
			NextRoundButton.IsEnabled = false;
			NextRoundButton.IsVisible = false ;
		}
		else
		{
			NextRoundButton.IsVisible = true;
			NextRoundButton.IsEnabled = true;
		}
	}
    private async void OnPreviousRoundClicked(object sender, EventArgs e)
    {
		//Load Round Data from previous round, if current is equal to 0 turn off otherwise on
		current--;
		currentRound = rounds[current];
		//save round

		//load round
		UpdateDisplay();
		if(current == 0)
		{
			PreviousRoundButton.IsVisible=false;
			PreviousRoundButton.IsEnabled=false;
		}
		else
		{
			PreviousRoundButton.IsVisible=true;
			PreviousRoundButton.IsEnabled=true;
		}
    }
	private void UpdateDisplay()
	{

	}
	private void SaveRound()
	{

	}
	private async void OnContinueButtonClick()
	{ 
		_eventItem.ScoreCard.Rounds = rounds;
		Close();
		await _navigation.PushAsync(new ScoresPage());
	}
	
}