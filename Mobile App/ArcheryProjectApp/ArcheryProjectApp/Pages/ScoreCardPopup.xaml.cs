namespace ArcheryProjectApp.Pages;
using CommunityToolkit.Maui.Views;
using ArcheryLibrary;
using System.Xml.Serialization;

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
		StandardTargetPicker.ItemsSource = App.targets;
		UpdateDisplay();
	}
	private void OnEndSliderChange(object sender, ValueChangedEventArgs e)
	{
        //change slider values to be whole numbers
        var slider = (Slider)sender;
		slider.Value = Math.Round(e.NewValue);
		EndsLabel.Text = $"Ends: {EndSlider.Value}";
		//get value of slider and cast to int, then send to populate the pickers in flint layout
        int value = (int)slider.Value;
        PopulateFlintLayout(value);
	}
	private void OnArrowSliderChange(object sender, ValueChangedEventArgs e)
	{
		//change slider values to be whole numbers
		var slider = (Slider)sender;
		slider.Value = Math.Round(e.NewValue);
		ArrowsLabel.Text = $"Arrows: {ArrowSlider.Value}";
	}
	private void PopulateFlintLayout(int count)
	{
		//Adds a variable amount of pickers to the flint details layout for providing users with ability to customise each end per round
		FlintDetailsLayout.Children.Clear();
		for(int i = 0; i < count; i++)
		{
			var container = new HorizontalStackLayout();
			var label = new Label
			{
				Text = $"End {i}",
			};
			//picker for selecting target
			var picker = new Picker
			{
				Title = $"Select End Target",
				HorizontalOptions = LayoutOptions.Fill,
			};
			//define list for picker - all possible targets available
			picker.ItemsSource = App.targets;

			container.Children.Add(label);
			container.Children.Add(picker);
			FlintDetailsLayout.Children.Add(container);
		}
		
	}
	private async void OnNextRoundClicked(object sender, EventArgs e)
	{
        //Save Round Info
        SaveRound();
        //Load Round Data from next round, if current is equal to rounds.count turn off other wise on
        current++;
		currentRound = rounds[current];
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
		//Save current Round
		SaveRound();
		//Load Round Data from previous round, if current is equal to 0 turn off otherwise on
		current--;
		currentRound = rounds[current];
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
	private bool IsRoundPopulated(Round round)
	{
		if(!round.Type.Equals("") && round.EndCount != 0 && round.ShotsPerEnd != 0 && !round.PositionType.Equals(""))
		{
			if (round.Type.Equals("Flint"))
			{
				if ((round.DistancePerEnd.Count() < 0 || round.DistancePerEnd.Equals(null)) && (round.TargetsPerEnd.Count() < 0 || round.TargetsPerEnd.Equals(null)))
				{
					return false;
				}
				else if(round.Distance == 0 && round.Unit == "" && round.Target == null) 
				{
					return false;
				}
				else { return true; }
			}
			else return true;
			
		}
		else return false;
	}
	private void UpdateDisplay()
	{
		if(IsRoundPopulated(currentRound))
		{
			if (currentRound.Type.Equals("Standard"))
			{
				StandardRadioButton.IsChecked = true;
				FlintRadioButton.IsChecked = false;
                StandardDetailsLayout.IsEnabled = true;
                StandardDetailsLayout.IsVisible = true;
                FlintDetailsLayout.IsVisible = false;
                FlintDetailsLayout.IsEnabled = false;
				StandardDistanceEditor.Text = currentRound.Distance.ToString();
				StandardDistanceUnitEditor.Text = currentRound.Unit.ToString();
				//StandardTargetPicker.SelectedItem = ;
            }
			else
			{
				StandardRadioButton.IsChecked = false;
				FlintRadioButton.IsChecked = true;
                StandardDetailsLayout.IsEnabled = false;
                StandardDetailsLayout.IsVisible = false;
                FlintDetailsLayout.IsVisible = true;
                FlintDetailsLayout.IsEnabled = true;

            }
			EndSlider.Value = currentRound.EndCount;
			ArrowSlider.Value = currentRound.ShotsPerEnd;
			if(currentRound.PositionType.Equals(ShootingPosition.Stationary))
			{
                StationaryRadioButton.IsChecked = true;
                WalkBackRadioButton.IsChecked = false;
                WalkUpRadioButton.IsChecked = false;
            }
			else if (currentRound.PositionType.Equals(ShootingPosition.WalkUp))
			{
                StationaryRadioButton.IsChecked = false;
                WalkBackRadioButton.IsChecked = false;
                WalkUpRadioButton.IsChecked = true;
            }
			else
			{
                StationaryRadioButton.IsChecked = false;
                WalkBackRadioButton.IsChecked = true;
                WalkUpRadioButton.IsChecked = false;
            }
		}
		else
		{
			StandardRadioButton.IsChecked=true;
			FlintRadioButton.IsChecked=false;
			EndsLabel.Text = "Ends: ";
			EndSlider.Value = 3;
			ArrowsLabel.Text = "Arrows: ";
			ArrowSlider.Value = 3;
			StandardDetailsLayout.IsEnabled = true;
			StandardDetailsLayout.IsVisible = true;
			FlintDetailsLayout.IsVisible = false;
			FlintDetailsLayout.IsEnabled = false;
			StationaryRadioButton.IsChecked = true;
			WalkBackRadioButton.IsChecked = false;
			WalkUpRadioButton.IsChecked = false;
		}
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
	private async void OnStandardCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (sender is RadioButton radioButton && e.Value)
		{
			currentRound.Type = "Standard";
		}
	}
    private async void OnFlintCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            currentRound.Type = "Flint";
        }
    }
    private async void OnStationaryCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            currentRound.PositionType = ShootingPosition.Stationary;
        }
    }
    private async void OnWalkUpCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            currentRound.PositionType = ShootingPosition.WalkUp;
        }
    }
    private async void OnWalkBackCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            currentRound.PositionType = ShootingPosition.WalkBack;
        }
    }
	private void DisplayFlintFields()
	{
		FlintDetailsLayout.IsEnabled = true;
		FlintDetailsLayout.IsVisible = true;
	}
}