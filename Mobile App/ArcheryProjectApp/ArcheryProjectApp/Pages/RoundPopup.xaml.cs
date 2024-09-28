namespace ArcheryProjectApp.Pages;
using CommunityToolkit.Maui.Views;
using ArcheryLibrary;
using System.Collections.ObjectModel;

//To Do: Have loading of round information from existing information, Have Saving of Round Data, Fix Navigation Buttons, Fix Targets Stuff
public partial class RoundPopup : Popup
{
	private INavigation _navigation;
	private List<Round> rounds = new List<Round>();
	private Event _eventItem;
	private int current;
	private ObservableCollection<FlintEnd> flintEnds = new ObservableCollection<FlintEnd>();
	public RoundPopup(Event eventItem, INavigation navigation)
    {
        InitializeComponent();
        _navigation = navigation;
        _eventItem = eventItem;
        ToggleContinueButton();
        if (eventItem.Rounds == null)
        {
            for (int i = 0; i < eventItem.RoundCount; i++)
            {
                rounds.Add(new Round());
            }
        }
        else
        {
            rounds = eventItem.Rounds;
        }

        ToggleNavButtons();
        current = 0;
        StandardTargetPicker.ItemsSource = App.targets;
        //Check for existing round data and update display fields if possible
        UpdateDisplay();
    }

    private void ToggleContinueButton()
    {
        ContinueButton.IsEnabled = !ContinueButton.IsEnabled;
        ContinueButton.IsVisible = !ContinueButton.IsVisible;
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

    [Obsolete]
	//this is super buggy idk why
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
        // Check if removal is required
        else if (count < flintEnds.Count)
        {
            for (int i = flintEnds.Count - 1; i >= count; i--)
            {
				if (FlintDetailsLayout.Children.Count > i)
				{
					var childView = FlintDetailsLayout.Children[i];
					if (childView is Editor editor)
					{
						editor.Text = string.Empty;
					}
					if (childView is Picker picker)
					{
						picker.SelectedIndex = -1;
					}
					await Device.InvokeOnMainThreadAsync(() =>
					{
						if (FlintDetailsLayout.Children.Contains(childView))
						{
                            FlintDetailsLayout.Children.RemoveAt(i);
                        }
						
					});
				}
                // Remove the FlintEnd object from the collection
                flintEnds.RemoveAt(i);
            }
        }
        //create collection view with new flintend items
        if (FlintDetailsLayout.Children.Count == 0)
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
                    return container;
                })
			};
			FlintDetailsLayout.Children.Clear();
			FlintDetailsLayout.Children.Add(collectionView);
		}
	}
	private async void OnNextRoundClicked(object sender, EventArgs e)
	{
        //Save Round Info
        SaveRound();
        //Load Round Data from next round, if current is equal to rounds.count turn off other wise on
        current++;
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
				int successCount = 0;
				foreach(End end in round.Ends)
				{
					if(end != null && (!end.Distance.Equals("") || end.Distance != null)&& end.Target != null)
					{
						successCount++;
					}
				}
				if(successCount == round.EndCount)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else return true;
			
		}
		else return false;
	}
	//create methods for loading existing round data
	private void UpdateDisplay()
	{
		CurrentRoundLabel.Text = $"Round: {current + 1}";
		if (IsRoundPopulated(rounds[current]))
		{
			if (rounds[current].Type.Equals("Standard"))
			{
				//populated standard round input
				StandardRadioButton.IsChecked = true;
				FlintRadioButton.IsChecked = false;
                StandardDetailsLayout.IsEnabled = true;
                StandardDetailsLayout.IsVisible = true;
                FlintDetailsLayout.IsVisible = false;
                FlintDetailsLayout.IsEnabled = false;
				StandardDistanceEditor.Text = rounds[current].Distance;
				
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
			EndSlider.Value = rounds[current].EndCount;
			ArrowSlider.Value = rounds[current].ShotsPerEnd;
			if(rounds[current].PositionType.Equals(ShootingPosition.Stationary))
			{
                StationaryRadioButton.IsChecked = true;
                WalkBackRadioButton.IsChecked = false;
                WalkUpRadioButton.IsChecked = false;
            }
			else if (rounds[current].PositionType.Equals(ShootingPosition.WalkUp))
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
		if(rounds[current].Type == "Flint")
		{
			//get  type, target and distance for each end in round.
			
		}
		else
		{
            rounds[current].Target = StandardTargetPicker.SelectedItem as Target;
            rounds[current].Distance = StandardDistanceEditor.Text;
            rounds[current].ShotsPerEnd = (int)ArrowSlider.Value;
			rounds[current].EndCount = (int)EndSlider.Value;
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
	private void OnStandardCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (sender is RadioButton radioButton && e.Value)
		{
            rounds[current].Type = "Standard";
			StationaryRadioButton.IsChecked = true;
			ToggleFlintDisplay();
			ToggleRadioButtons();
			ToggleStandardDisplay();
		}
	}
    private void OnFlintCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            rounds[current].Type = "Flint";
			ToggleFlintDisplay();
			ToggleRadioButtons();
			ToggleStandardDisplay();
        }
    }
    private void OnStationaryCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            rounds[current].PositionType = ShootingPosition.Stationary;
        }
    }
    private void OnWalkUpCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            rounds[current].PositionType = ShootingPosition.WalkUp;
        }
    }
    private void OnWalkBackCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            rounds[current].PositionType = ShootingPosition.WalkBack;
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
			NextRoundButton.IsVisible = false;
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
		SaveRound();
        _eventItem.Rounds = rounds;
		Console.WriteLine($"{rounds.Count} rounds.");
        Close();
        await _navigation.PushAsync(new ScoresPage(_eventItem));
    }
}