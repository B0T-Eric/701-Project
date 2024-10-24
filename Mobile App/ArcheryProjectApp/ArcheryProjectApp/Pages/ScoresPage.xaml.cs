using ArcheryLibrary;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
namespace ArcheryProjectApp.Pages;

public partial class ScoresPage : ContentPage
{
    Event currentEvent; //user event.
    private VerticalStackLayout mainLayout;
    //scores page is passed reference to a user event which it then creates views to display each of the ends and rounds
    public ScoresPage(Event userEvent)
    {
        InitializeComponent();
        currentEvent = userEvent;
        mainLayout = RoundScoringLayout; //host layout for collection view.
        
        int roundNum = 0;
        //divide into rounds
        foreach(Round round in currentEvent.Rounds)
        {
            round.XTotal = 0;
            round.RoundTotal = 0;
            roundNum++;
            var roundVertical = new VerticalStackLayout {BindingContext = round };
            var roundLabel = new Label {Text=$"Round {roundNum}", FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };
            roundVertical.Children.Add(roundLabel);
            //if there the round is standard, display target and distance information at the topp of the scorecard otherwise don't show anything.
            if (round.Type == "Standard")
            {
                var detailHorizontal = new HorizontalStackLayout();
                detailHorizontal.HorizontalOptions = LayoutOptions.FillAndExpand;
                detailHorizontal.AlignSelf(FlexAlignSelf.Center);

                var distanceText = new Label { FontSize = 10, Margin = 4};
                distanceText.SetBinding(Label.TextProperty, new Binding("Distance", stringFormat: "{0} Distance, Shooting at:"));
                detailHorizontal.Children.Add(distanceText);

                var targetText = new Label { FontSize = 10, Margin = 4};
                targetText.SetBinding(Label.TextProperty, new Binding("Target", stringFormat: "{0}"));
                detailHorizontal.Children.Add(targetText);

                roundVertical.Children.Add(detailHorizontal);
            }
            //Collection View for End score entry.
            CollectionView endCollectionView = GenerateEndViews(round.Ends, round.Type);
            endCollectionView.SelectionMode = SelectionMode.None;
            roundVertical.Children.Add(endCollectionView);

            //at bottom of each round display x totals, and total score.
            var horizontals = new HorizontalStackLayout();
            horizontals.HorizontalOptions = LayoutOptions.CenterAndExpand;
            var roundText = new Label { Text = "Round Total: "};
            horizontals.Children.Add(roundText);
            var roundTotalLabel = new Label { Text = $"{round.RoundTotal} " };
            roundTotalLabel.SetBinding(Label.TextProperty, "RoundTotal");
            horizontals.Children.Add(roundTotalLabel);
            //x total
            var xText = new Label { Text = "Total X's: " };
            horizontals.Children.Add(xText);
            var roundXTotal = new Label { Text = $"{round.XTotal}"};
            roundXTotal.SetBinding(Label.TextProperty, "XTotal");
            horizontals.Children.Add(roundXTotal);


            roundVertical.Children.Add(horizontals);
            mainLayout.Children.Add(roundVertical);
        }
    }
    //create a set of collection view items based on the ends provided by selected events rounds.
    private CollectionView GenerateEndViews(List<End> ends, string type)
    {
        var endCollectionView = new CollectionView
        {
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical),
            ItemsSource = ends, //round ends
            ItemTemplate = new DataTemplate(() =>
            {
                //grid layout parent
                var grid = new Grid
                {
                    ColumnSpacing = 5,
                    RowSpacing = 5,
                    ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto }, // End number
                    new ColumnDefinition { Width = GridLength.Auto }, // Position, distance, target (for Flint rounds only - displayed as empty if standard)
                    new ColumnDefinition { Width = GridLength.Star }, // Score entry fields
                    new ColumnDefinition { Width = GridLength.Auto }, // X Count
                    new ColumnDefinition { Width = GridLength.Auto }, // End Total
                    new ColumnDefinition { Width = GridLength.Auto }  // Running Total
                }
                };

                // End Number Label
                var endNumberLabel = new Label();
                endNumberLabel.SetBinding(Label.TextProperty, new Binding("EndNum"));
                grid.Children.Add(endNumberLabel); // Add the label
                Grid.SetColumn(endNumberLabel, 0); // Set to the first column

                if (type == "Flint")
                {
                    //popuplate view if flint
                    var flintStack = new VerticalStackLayout();
                    // Shooting position
                    var positionLabel = new Label();
                    positionLabel.FontSize = 8;
                    positionLabel.SetBinding(Label.TextProperty, "Position");
                    flintStack.Children.Add(positionLabel);

                    // End Distance
                    var distanceLabel = new Label();
                    distanceLabel.FontSize = 8;
                    distanceLabel.SetBinding(Label.TextProperty, "Distance");
                    flintStack.Children.Add(distanceLabel);

                    // End Target
                    var targetLabel = new Label();
                    targetLabel.FontSize = 8;
                    targetLabel.SetBinding(Label.TextProperty, "Target");
                    flintStack.Children.Add(targetLabel);

                    grid.Children.Add(flintStack);
                    Grid.SetColumn(flintStack, 1);
                }
                else
                {
                    //empty view if not flint
                    var standardStack = new VerticalStackLayout();

                    grid.Children.Add(standardStack);
                    Grid.SetColumn(standardStack, 1);
                }

                // Score Entries (generate dynamically based on ArrowCount)
                var scoreGrid = new Grid
                {
                    ColumnSpacing = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                for (int i = 0; i < ends.FirstOrDefault()?.ArrowCount; i++)
                {
                    scoreGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    var scoreEntry = new Entry
                    {
                        Placeholder = $"{i + 1}",
                        StyleId = i.ToString(),
                        MaxLength = 2,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    scoreEntry.SetBinding(Entry.TextProperty, new Binding($"Score[{i}]"));
                    scoreEntry.TextChanged += ValidateEntry;
                    scoreEntry.TextChanged += OnScoreEntryChanged;
                    scoreEntry.Focused += EntryFocused;
                    Grid.SetColumn(scoreEntry, i);
                    scoreGrid.Children.Add(scoreEntry);
                }
                grid.Children.Add(scoreGrid);
                Grid.SetColumn(scoreGrid, 2);

                // X Count Label
                var xCountLabel = new Label { TextColor = Colors.Green };
                xCountLabel.SetBinding(Label.TextProperty, new Binding("XCount", stringFormat: "{0}"));
                grid.Children.Add(xCountLabel); 
                Grid.SetColumn(xCountLabel, 3); 

                // End Total Label
                var endTotalLabel = new Label { TextColor = Colors.Blue };
                endTotalLabel.SetBinding(Label.TextProperty, new Binding("EndTotal", stringFormat: "{0}"));
                grid.Children.Add(endTotalLabel); 
                Grid.SetColumn(endTotalLabel, 4); 

                // Running Total Label
                var runningTotalLabel = new Label { TextColor = Colors.Red };
                runningTotalLabel.SetBinding(Label.TextProperty, new Binding("RunningTotal", stringFormat: "{0}"));
                grid.Children.Add(runningTotalLabel); 
                Grid.SetColumn(runningTotalLabel, 5); 

                Frame frame = new Frame();
                frame.Content = grid;
                frame.Margin = 5;
                frame.Padding = 5;

                return frame;
            })
        };

        return endCollectionView;
    }
    //limit entry to x and m and only the values of the score zones (1 digit only as to not let mistakes through, any 10s and x's).
    private async void ValidateEntry(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry != null)
        {
            string allowedInput;
            var end = entry.BindingContext as End;
            if (end != null && end.Target != null)
            {
                allowedInput = string.Join(",", end.Target.ZoneValues) + ",X,M,x,m";
            }
            else
            {
                var round = currentEvent.Rounds.FirstOrDefault(r => r.Ends.Contains(end));
                allowedInput = string.Join(",", round.Target.ZoneValues) + ",X,M,x,m";
            }
            var allowedValues = allowedInput.Split(',').ToList();
            string newText = e.NewTextValue;
            if (!string.IsNullOrEmpty(newText) && !allowedValues.Contains(newText))
            {
                entry.Text = e.OldTextValue;
            }
        }
    }
    //attempt to scroll to the view.
    private void EntryFocused(object sender, FocusEventArgs e) => ScoreScrollView.ScrollToAsync((VisualElement)sender, ScrollToPosition.Start, true);
    //On user input determine running totals, x count and end scores.
    public void OnScoreEntryChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if(entry == null || string.IsNullOrEmpty(entry.StyleId)) return;
        if(int.TryParse(entry.StyleId, out int scoreIndex))
        {
            var currentEnd = (entry.BindingContext as End);
            if(currentEnd != null && scoreIndex < currentEnd.ArrowCount)
            {
                currentEnd.Score[scoreIndex] = e.NewTextValue;
                var currentRound = currentEvent.Rounds.FirstOrDefault(r => r.Ends.Contains(currentEnd));
                if(currentRound != null)
                {
                    currentRound.RoundTotal = 0;
                    currentRound.XTotal = 0;
                    int currentEndIndex = currentRound.Ends.IndexOf(currentEnd);
                    List<int> zoneValues = null;
                    if(currentRound.Type == "Standard")
                    {
                        zoneValues = currentRound.Target?.ZoneValues;
                    }
                    else if(currentEnd.Target != null)
                    {
                        zoneValues = currentEnd.Target.ZoneValues;
                    }
                    UpdateEndTotals(currentEnd, currentRound.Ends, zoneValues);
                    for(int i = currentEndIndex; i < currentRound.Ends.Count; i++)
                    {
                        End end = currentRound.Ends[i];
                        if(currentRound.Type == "Standard")
                        {
                            UpdateEndTotals(end, currentRound.Ends, currentRound.Target.ZoneValues);
                        }
                        else if(currentRound.Type == "Flint")
                        {
                            UpdateEndTotals(end, currentRound.Ends, end.Target.ZoneValues);
                        }
                        
                        
                    }
                    foreach (End _end in currentRound.Ends)
                    {
                        currentRound.RoundTotal += _end.EndTotal;
                        currentRound.XTotal += _end.XCount;
                    }


                }
                
            }
        }
    }

    private void UpdateEndTotals(End end, List<End> roundEnds, List<int>? zoneValues)
    {
        end.XCount = end.Score.Count(s => s.Equals("X", StringComparison.OrdinalIgnoreCase));
        int totalScore = 0;
        foreach (var score in end.Score)
        {
            if (int.TryParse(score, out int parsedScore))
            {
                totalScore += parsedScore;
            }
            else if (score.Equals("X", StringComparison.OrdinalIgnoreCase))
            {
                //if there is a target for the end (flint round) then use the maximum value of the target
                if(end.Target != null)
                {
                    totalScore += end.Target.ZoneValues[^1];
                }
                //if there is zoneValues param (standard round) then use the maximum value of target
                else if(zoneValues != null)
                {
                    totalScore += zoneValues[^1];
                }
            }
            else if (score.Equals("M", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
        }
        end.EndTotal = totalScore;
        int runningTotal = 0;
        foreach (var endItem in roundEnds)
        {
            if(endItem.EndNum <= end.EndNum)
            {
                runningTotal += endItem.EndTotal;
            }
        }
        end.RunningTotal = runningTotal;
        
    }
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        bool allFieldsChecked = true;
        foreach(var round in currentEvent.Rounds)
        {
            foreach(var end in round.Ends)
            {
                for(int i = 0; i < end.ArrowCount; i++)
                {
                    if (string.IsNullOrWhiteSpace(end.Score[i]))
                    {
                        allFieldsChecked = false;
                        break;
                    }
                }
                if (!allFieldsChecked)
                {
                    break;
                }
            }
            if (!allFieldsChecked)
            {
                break;
            }
        }
        if (!allFieldsChecked)
        {
            DisplayAlert("Incomplete Rounds", "Please Fill all fields before continuing", "Return");
        }
        else
        {
            foreach (var round in currentEvent.Rounds)
            {
                round.IsComplete = true;
            }
            if (await App.dbService.CheckIfEventExists(currentEvent) == false) //if doesn't exist add event to database
            {
                await App.dbService.AddEventsToUserDatabase(currentEvent, ProfilePage.UserInstance.DetailId);
            }
            else //if it does exist ask for update
            {
                bool answer = await DisplayAlert("Update Database?", "Entry already exists, Would you like to Update it?", "Yes", "No");
                if(answer == true)
                {
                    await App.dbService.UpdateCompleteEvent(currentEvent);
                }
            }
            //send to API if not guest
            
            await Navigation.PopAsync();
        }

    }
}


