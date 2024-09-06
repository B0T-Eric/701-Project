namespace ArcheryProjectApp.Pages;
using CommunityToolkit.Maui.Views;
using ArcheryLibrary;
using System.Collections.ObjectModel;

//To Do: Have loading of round information from existing information, Have Saving of Round Data, Fix Navigation Buttons, Fix Targets Stuff
public partial class ScoreCardPopup : Popup
{
	private INavigation _navigation;
	private List<Round> rounds = new List<Round>();
	private Event _eventItem;
	private Round currentRound;
	private int current;
	private ObservableCollection<FlintEnd> flintEnds = new ObservableCollection<FlintEnd>();
	public ScoreCardPopup(Event eventItem, INavigation navigation)
	{
		InitializeComponent();
		_navigation = navigation;
		_eventItem = eventItem;
		for (int i = 0; i < eventItem.ScoreCard.RoundCount; i++)
		{
			rounds.Add(new Round());
		}
		ToggleNavButtons();
		currentRound = rounds[0];
		current = 0;
		StandardTargetPicker.ItemsSource = App.targets;
		//Check for existing round data and update display fields if possible
		UpdateDisplay();
	}
	public class FlintEnd
	{
		public string Distance { get; set; }
		public Target? EndTarget { get; set; }

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
	private async void OnArrowSliderChange(object sender, ValueChangedEventArgs e)
	{
		//change slider values to be whole numbers
		var slider = (Slider)sender;
		slider.Value = Math.Round(e.NewValue);
		ArrowsLabel.Text = $"Arrows: {ArrowSlider.Value}";
	}
	private async void PopulateFlintLayout(int count)
	{
		//Check if more are required
		if(count > flintEnds.Count)
		{
			for (int i = flintEnds.Count; i < count; i++)
			{
				flintEnds.Add(new FlintEnd { Distance = "", EndTarget = null });
			}
		}
		//check if removal required
		else if( count < flintEnds.Count)
		{
			for(int i = flintEnds.Count - 1; i >= count; i--)
			{
				if(FlintDetailsLayout.Children.Count > i)
				{
					FlintDetailsLayout.Children.RemoveAt(i);
				}
				flintEnds.RemoveAt(i);
			}
		}
		//create collection view with new flintend items
		if(FlintDetailsLayout.Children.Count == 0)
		{
			var collectionView = new CollectionView
			{
				ItemsSource = flintEnds,
				ItemTemplate = new DataTemplate(() =>
				{
                    var container = new HorizontalStackLayout();
                    // Picker for target selection
                    var picker = new Picker { Title = "Select End Target" };
                    picker.ItemsSource = App.targets;
                    picker.SetBinding(Picker.SelectedItemProperty, new Binding("Target"));

                    // Editors for distance and unit
                    var distEditor = new Editor { Placeholder = "Enter Distance" };
					distEditor.SetBinding(Editor.TextProperty, new Binding("Distance"));

                    container.Children.Add(distEditor);
                    container.Children.Add(picker);
                    Console.WriteLine("Container Returned Successfully To Collection View!");
                    return container;
                })
			};
			FlintDetailsLayout.Children.Clear();
			FlintDetailsLayout.Children.Add(collectionView);
			Console.WriteLine("Collection View Added Successfully!");
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
		ToggleNavButtons();
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
		ToggleNavButtons();
    }
	private bool IsRoundPopulated(Round round)
	{
		if(round.Type != null && round.EndCount != 0 && round.ShotsPerEnd != 0)
		{
			if (round.Type.Equals("Flint"))
			{
				if ((round.DistancePerEnd.Count() < 0 || round.DistancePerEnd.Equals(null)) && (round.TargetsPerEnd.Count() < 0 || round.TargetsPerEnd.Equals(null)))
				{
					return false;
				}
				else if(round.Distance == null  && round.Target == null) 
				{
					return false;
				}
				else { return true; }
			}
			else return true;
			
		}
		else return false;
	}
	//create methods for loading existing round data
	private void UpdateDisplay()
	{
		CurrentRoundLabel.Text = $"Round: {current + 1}";
		if(IsRoundPopulated(currentRound))
		{
			if (currentRound.Type.Equals("Standard"))
			{
				//populated standard round input
				StandardRadioButton.IsChecked = true;
				FlintRadioButton.IsChecked = false;
                StandardDetailsLayout.IsEnabled = true;
                StandardDetailsLayout.IsVisible = true;
                FlintDetailsLayout.IsVisible = false;
                FlintDetailsLayout.IsEnabled = false;
				StandardDistanceEditor.Text = currentRound.Distance;
				
            }
			else
			{
				//flint round stuff
				StandardRadioButton.IsChecked = false;
				FlintRadioButton.IsChecked = true;
                StandardDetailsLayout.IsEnabled = false;
                StandardDetailsLayout.IsVisible = false;
                FlintDetailsLayout.IsVisible = true;
                FlintDetailsLayout.IsEnabled = true;
                StandardTargetPicker.SelectedIndex = -1;
                StandardDistanceEditor.Text = "";

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
			//default empty views
			StandardRadioButton.IsChecked=true;
			FlintRadioButton.IsChecked=false;
			EndsLabel.Text = "Ends: ";
			EndSlider.Value = 3;
			ArrowsLabel.Text = "Arrows: ";
			ArrowSlider.Value = 3;
			StandardTargetPicker.SelectedIndex = -1;
			StandardDistanceEditor.Text = "";
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
		if(currentRound.Type == "Flint")
		{
			currentRound.TargetsPerEnd = GetListFromChildren();
			
		}
		else
		{

		}
	}
	//To Do: recreate this method
    private Dictionary<double, string> GetDictFromChildren()
    {
        Dictionary<double, string> keyValuePairs = new Dictionary<double, string>();

		foreach (var child in FlintDetailsLayout.Children)
		{
			if (child is HorizontalStackLayout layout)
			{
				double key = 0;
				string value = null;
				foreach (var child2 in layout.Children)
				{
					if (child2 is Editor editor)
					{
						if (editor.AutomationId.StartsWith("DistEditor_"))
						{
							if (double.TryParse(editor.Text, out key))
							{
								Console.WriteLine($"Successful Parse of {key}");
							}
						}
						else if (editor.AutomationId.StartsWith("UnitEditor_"))
						{
							value = editor.Text;
						}
					}
				}
				// After both key and value are set, add them to the dictionary
				if (!string.IsNullOrEmpty(value))
				{
					keyValuePairs[key] = value;
				}
			}
		}
        // Return populated dictionary
        return keyValuePairs;
    }
	//to do: recreate this method
    private List<Target> GetListFromChildren()
	{
		List<Target> list = new List<Target>();
        foreach (var child in FlintDetailsLayout.Children)
        {
            if (child is HorizontalStackLayout layout)
            {
                foreach (var child2 in layout.Children)
                {
					if(child2 is Picker picker)
					{
						if (picker.SelectedItem is Target target)
						{
							list.Add(target);
						}
					}
                }
            }
        }
		//return populated list of targets
		return list;
    }
	private async void OnStandardCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (sender is RadioButton radioButton && e.Value)
		{
			currentRound.Type = "Standard";
			ToggleFlintDisplay();
			ToggleRadioButtons();
			ToggleStandardDisplay();
		}
	}
    private async void OnFlintCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            currentRound.Type = "Flint";
			ToggleFlintDisplay();
			ToggleRadioButtons();
			ToggleStandardDisplay();
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
	private void ToggleNavButtons()
	{
		if (current == 0)
		{
			if (rounds.Count == 1)
			{
				TurnOffNextButton();
				TurnOffPrevButton();
			}
			else
			{
                TurnOffPrevButton();
				TurnOnNextButton();
            }
        }
		else if (current > 0 && current < rounds.Count)
		{
			TurnOnNextButton();
			TurnOnPrevButton();
		}
		else if(current == rounds.Count - 1)
		{
			TurnOffNextButton();
			TurnOnPrevButton();
		}
	}
	private void TurnOffPrevButton()
	{
        PreviousRoundButton.IsVisible = false;
        PreviousRoundButton.IsEnabled = false;
    }
	private void TurnOffNextButton()
	{
		NextRoundButton.IsVisible = false;
		NextRoundButton.IsEnabled = false;
	}
	private void TurnOnPrevButton() 
	{
        PreviousRoundButton.IsVisible = true;
        PreviousRoundButton.IsEnabled = true;
    }
	private void TurnOnNextButton()
	{
		NextRoundButton.IsVisible = true;
		NextRoundButton.IsEnabled = true;
	}
	private void ToggleFlintDisplay()
	{
		FlintDetailsLayout.IsEnabled = !FlintDetailsLayout.IsEnabled;
		FlintDetailsLayout.IsVisible = !FlintDetailsLayout.IsVisible;
	}
	private void ToggleRadioButtons()
	{
		WalkUpRadioButton.IsEnabled = !WalkUpRadioButton.IsEnabled;
		WalkBackRadioButton.IsEnabled = !WalkBackRadioButton.IsEnabled;
		WalkBackRadioButton.IsVisible = !WalkBackRadioButton.IsVisible;
		WalkUpRadioButton.IsVisible = !WalkUpRadioButton.IsVisible;
	}
	private void ToggleStandardDisplay()
	{
		StandardDetailsLayout.IsEnabled = !StandardDetailsLayout.IsEnabled;
		StandardDetailsLayout.IsVisible = !StandardDetailsLayout.IsVisible;
	}
	private void ToggleContinueDisplay()
	{
		ContinueButton.IsEnabled = !ContinueButton.IsEnabled;
		ContinueButton.IsVisible = !ContinueButton.IsVisible;
	}

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {
        _eventItem.ScoreCard.Rounds = rounds;
        Close();
        await _navigation.PushAsync(new ScoresPage());
    }
}