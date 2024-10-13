namespace ArcheryProjectApp.Pages;
using CommunityToolkit.Maui.Views;
using ArcheryLibrary;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Markup;

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
		ArrowPicker.ItemsSource = App.arrowCountOptions;
		EndPicker.ItemsSource = App.endCountOptions;
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
        current = 0;
        StandardTargetPicker.ItemsSource = App.targets;
        //Check for existing round data and update display fields if possible
        UpdateDisplay();
    }
	public class FlintEnd
	{
		public string Distance { get; set; }
		public Target EndTarget { get; set; }
		public ShootingPosition? Position { get; set; }

	}

    //this is super buggy idk why
    [Obsolete]
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
                    picker.SetBinding(Picker.SelectedItemProperty, new Binding("EndTarget"));


					// Editors for distance and unit
					var distEditor = new Entry {
						Placeholder = "Enter Distance",
						Behaviors =
						{
							new TextValidationBehavior
							{
								InvalidStyle = new Style<Entry>(Entry.TextColorProperty,Color.Red),
								ValidStyle = new Style<Entry>(Entry.TextColorProperty, Color.FromArgb("000000")),
								Flags = ValidationFlags.ValidateOnValueChanged,
								MinimumLength = 1, 
								MaximumLength = 10,
							}
						}
					};
					
					distEditor.SetBinding(Editor.TextProperty, new Binding("Distance"));

					//Picker for type
					var typePicker = new Picker { Title = "Select Shooting Position" };
					typePicker.ItemsSource = App.positionOptions;
					typePicker.SetBinding(Picker.SelectedItemProperty, new Binding("Position"));

                    container.Children.Add(distEditor);
                    container.Children.Add(picker);
					container.Children.Add(typePicker);	
                    return container;
                })
			};
			FlintDetailsLayout.Children.Clear();
			FlintDetailsLayout.Children.Add(collectionView);
		}
	}
	private void EndPickerChanged(object sender, EventArgs e)
	{
		PopulateFlintLayout((int)EndPicker.SelectedItem);
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
					if(end != null && (!end.Distance.Equals("") || end.Distance != null)&& end.Target != null && end.Position != null)
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
				StandardTargetPicker.SelectedIndex = StandardTargetPicker.Items.IndexOf(rounds[current].Target.ToString()); 
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
			EndPicker.SelectedItem = rounds[current].EndCount;
			ArrowPicker.SelectedItem = rounds[current].ShotsPerEnd;
		}
		else
		{
			//default empty views
			StandardRadioButton.IsChecked=true;
            rounds[current].Type = "Standard";
            FlintRadioButton.IsChecked=false;
			EndPicker.SelectedIndex = 0;
			ArrowPicker.SelectedIndex = 0;
			StandardTargetPicker.SelectedIndex = -1;
			StandardDistanceEditor.Text = "";
			StandardDetailsLayout.IsEnabled = true;
			StandardDetailsLayout.IsVisible = true;
			FlintDetailsLayout.IsVisible = false;
			FlintDetailsLayout.IsEnabled = false;
		}
	}
	private void OnFlintCheckedChanged(object sender, EventArgs e)
	{
		ToggleFlintDisplay();
		rounds[current].Type = "Flint";
	}
	private void OnStandardCheckedChanged(object sender, EventArgs e)
	{
		ToggleStandardDisplay();
        rounds[current].Type = "Standard";
    }
	private void SaveRound()
	{
        if (rounds[current].Type == "Flint")
		{
			rounds[current].EndCount = (int)EndPicker.SelectedItem;
			//get  type, target and distance for each end in rounds
			for(int i = 0; i < rounds[current].EndCount; i++)
			{
				rounds[current].Ends.Add(new End(i+1));
			}
			int index = 0;
			foreach(End end in rounds[current].Ends)
			{
				end.Position = GetFlintEndPosition(index);
				end.ArrowCount = (int)ArrowPicker.SelectedItem;
				end.Target = GetFlintEndTargets(index);
				end.Distance = GetFlintEndDistance(index);
                for (int j = 0; j < end.ArrowCount; j++)
                {
                    end.Score.Add("");
                }
                index++;
			}
		}
		else
		{
            rounds[current].Target = StandardTargetPicker.SelectedItem as Target;
            rounds[current].Distance = StandardDistanceEditor.Text;
            rounds[current].ShotsPerEnd = (int)ArrowPicker.SelectedItem;
			rounds[current].EndCount = (int)EndPicker.SelectedItem;
            rounds[current].Type = "Standard";
            for (int i = 0; i < rounds[current].EndCount; i++)
			{
				rounds[current].Ends.Add(new End(i+1, ShootingPosition.Stationary, (int)ArrowPicker.SelectedItem, null, null));
				rounds[current].Ends[i].Score = new List<string>(rounds[current].EndCount);
                for (int j = 0; j < rounds[current].Ends[i].ArrowCount; j++)
                {
                    rounds[current].Ends[i].Score.Add(""); 
                }
            }
		}
	}

    private ShootingPosition GetFlintEndPosition(int index)
    {
        if(index >= 0 && index < flintEnds.Count)
		{
			return (ShootingPosition)flintEnds[index].Position;
		}
		return default(ShootingPosition);
    }

    private string? GetFlintEndDistance(int index)
    {
        if (index >= 0 && index < flintEnds.Count)
        {
            return flintEnds[index].Distance;
        }
        return default;
    }

    private Target? GetFlintEndTargets(int index)
    {
        if (index >= 0 && index < flintEnds.Count)
        {
            return flintEnds[index].EndTarget;
        }
        return default;
    }

    private void ToggleFlintDisplay()
	{
		FlintDetailsLayout.IsEnabled = !FlintDetailsLayout.IsEnabled;
		FlintDetailsLayout.IsVisible = !FlintDetailsLayout.IsVisible;
	}
	private void ToggleStandardDisplay()
	{
		StandardDetailsLayout.IsEnabled = !StandardDetailsLayout.IsEnabled;
		StandardDetailsLayout.IsVisible = !StandardDetailsLayout.IsVisible;
	}
    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {
		//if all fields are not empty save round
		if(CheckFields() == true)
		{
            SaveRound();
            if (current == rounds.Count-1)
            {
				//save rounds to event
                _eventItem.Rounds = rounds;
                Close();
				//load scoring page
                await _navigation.PushAsync(new ScoresPage(_eventItem));
            }
            else
            {
                //Load Round Data from next round, if current is equal to rounds.count turn off other wise on
                current++;
                //Load Round Info
                UpdateDisplay();
            }
        }
		else
		{ 
			
		}
    }
	private bool CheckFields()
	{
		if(EndPicker.SelectedIndex == -1) return false;
		if(ArrowPicker.SelectedIndex == -1) return false;
		if(StandardRadioButton.IsChecked == true)
		{
			if(StandardDistanceEditor.Text == "" || StandardDistanceEditor.Text == null)
			{
				return false;
			}
			if(StandardTargetPicker.SelectedIndex == -1) return false;
		}
		else if(FlintRadioButton.IsChecked == true)
		{
			if(EndPicker.SelectedItem != null)
			{
				for(int i = 0; i < (int)EndPicker.SelectedItem; i++)
				{
					if (flintEnds[i].Distance == "" || flintEnds[i].Distance == null)
					{
						return false;
					}
					if (flintEnds[i].EndTarget == null)
					{
						return false;
					}
					if (flintEnds[i].Position == null)
					{
						return false;
					}
				}
			}
		}
		return true;
	}
}