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

					//Picker for type
					var typePicker = new Picker { Title = "Select End Type" };
					typePicker.ItemsSource = App.positionOptions;
					typePicker.SetBinding(Picker.SelectedIndexProperty, new Binding("EndType"));

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
			FlintRadioButton.IsChecked=false;
			EndsLabel.Text = "Ends: ";
			EndPicker.SelectedIndex = -1;
			ArrowsLabel.Text = "Arrows: ";
			ArrowPicker.SelectedIndex = -1;
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
	private async void SaveRound()
	{
        if (EndPicker.SelectedItem == null || ArrowPicker.SelectedItem == null)
        {
            // Display an error or return early to prevent a null exception
            await Application.Current.MainPage.DisplayAlert("Error", "Please select both the number of ends and arrows per end.", "OK");
            return;
        }

        if (rounds[current].Ends == null)
        {
            rounds[current].Ends = new List<End>();
        }

        if (rounds[current].Type == "Flint")
		{
			rounds[current].EndCount = (int)EndPicker.SelectedItem;
			//get  type, target and distance for each end in rounds
			for(int i = 0; i < rounds[current].EndCount; i++)
			{
				rounds[current].Ends.Add(new End());
			}
			foreach(End end in rounds[current].Ends)
			{
				end.Position = GetFlintEndPosition();
				end.ArrowCount = (int)ArrowPicker.SelectedItem;
				end.Target = GetFlintEndTargets();
				end.Distance = GetFlintEndDistance();
			}
		}
		else
		{
            rounds[current].Target = StandardTargetPicker.SelectedItem as Target;
            rounds[current].Distance = StandardDistanceEditor.Text;
            rounds[current].ShotsPerEnd = (int)ArrowPicker.SelectedItem;
			rounds[current].EndCount = (int)EndPicker.SelectedItem;
			//for(int i = 0; i < rounds[current].EndCount; i++)
			//{
				rounds[current].Ends.Add(new End(ShootingPosition.Stationary, (int)ArrowPicker.SelectedItem, null, null));
            //}
		}
	}

    private ShootingPosition GetFlintEndPosition()
    {
        throw new NotImplementedException();
    }

    private string? GetFlintEndDistance()
    {
        throw new NotImplementedException();
    }

    private Target? GetFlintEndTargets()
    {
        throw new NotImplementedException();
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
        if ()
        {
            
        }
        SaveRound();
        if(current == _eventItem.Rounds.Count)
		{
            _eventItem.Rounds = rounds;
            Close();
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
	private bool CheckFields()
	{
		if (rounds[current].EndCount == 0)
		{
			return false;
		}
		else if (rounds[current].Type == "Stationary")
		{
			if (rounds[current].Target == null)
			{
				return false;
			}
			else if (rounds[current].Distance == "")
			{
				return false;
			}
		}
		else if (rounds[current].EndCount == 0)
		{
			return false;
		}
	}
}