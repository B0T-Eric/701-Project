using ArcheryLibrary;
using System.Text.RegularExpressions;
namespace ArcheryProjectApp.Pages;

public partial class ScoresPage : ContentPage
{
    Event currentEvent;
    private VerticalStackLayout mainLayout;
    public ScoresPage(Event userEvent)
    {
        InitializeComponent();
        currentEvent = userEvent;
        mainLayout = RoundScoringLayout;
        
        int roundNum = 0;
        //divide into rounds
        foreach(Round round in currentEvent.Rounds)
        {
            round.XTotal = 0;
            round.RoundTotal = 0;
            roundNum++;
            var roundVertical = new VerticalStackLayout();
            var roundLabel = new Label {Text=$"Round {roundNum}", FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };
            roundVertical.Children.Add(roundLabel);
            CollectionView endCollectionView = GenerateEndViews(round.Ends, round.Type, roundVertical); //create collection view items based on each end in the round.
            roundVertical.Children.Add(endCollectionView);

            //at bottom of each round display x totals, and total score.
            var horizontals = new HorizontalStackLayout();
            horizontals.BindingContext = round;
            var roundText = new Label { Text = "Round Total: " };
            horizontals.Children.Add(roundText);
            var roundTotalLabel = new Label { Text = $"{round.RoundTotal}" };
            roundTotalLabel.SetBinding(Label.TextProperty, "RoundTotal");
            horizontals.Children.Add(roundTotalLabel);
            var xText = new Label { Text = "Total X's: " };
            horizontals.Children.Add(xText);
            var roundXTotal = new Label { Text = $"{round.XTotal}" };
            roundXTotal.SetBinding(Label.TextProperty, "XTotal");

            
            horizontals.Children.Add(roundXTotal);


            roundVertical.Children.Add(horizontals);
            mainLayout.Children.Add(roundVertical);
        }
    }

    private CollectionView GenerateEndViews(List<End> ends, string type, VerticalStackLayout roundLayout)
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
                    new ColumnDefinition { Width = GridLength.Auto }, // Position, distance, target (for Flint)
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
                    positionLabel.SetBinding(Label.TextProperty, "Position");
                    flintStack.Children.Add(positionLabel);

                    // End Distance
                    var distanceLabel = new Label();
                    distanceLabel.SetBinding(Label.TextProperty, "Distance");
                    flintStack.Children.Add(distanceLabel);

                    // End Target
                    var targetLabel = new Label();
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
                var scoreLayout = new HorizontalStackLayout();
                for (int i = 0; i < ends.FirstOrDefault()?.ArrowCount; i++)
                {
                    var scoreEntry = new Entry { Placeholder = $"Arrow {i + 1}", StyleId = i.ToString() };
                    scoreEntry.SetBinding(Entry.TextProperty, new Binding($"Score[{i}]"));
                    scoreEntry.MaxLength = 1;
                    scoreEntry.TextChanged += ValidateEntry;
                    scoreEntry.TextChanged += OnScoreEntryChanged;
                    scoreEntry.Focused += EntryFocused;
                    scoreLayout.Children.Add(scoreEntry);
                }
                grid.Children.Add(scoreLayout); 
                Grid.SetColumn(scoreLayout, 2); 

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
    private async void ValidateEntry(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if(entry != null) 
        {
            string allowedInput;
            var end = entry.BindingContext as End;
            if(end.Target != null)
            {
                allowedInput = string.Join("", end.Target.ZoneValues) + "XMxm";
                string newText = e.NewTextValue;
                if (!string.IsNullOrEmpty(newText) && !newText.All(c => allowedInput.Contains(c)))
                {
                    entry.Text = e.OldTextValue;
                }
            }
            else
            {
                var round = currentEvent.Rounds.FirstOrDefault(r => r.Ends.Contains(end));
                allowedInput = string.Join("", round.Target.ZoneValues) + "XMxm";
                string newText = e.NewTextValue;
                if (!string.IsNullOrEmpty(newText) && !newText.All(c => allowedInput.Contains(c)))
                {
                    entry.Text = e.OldTextValue;
                }
            }
        }
        else
        {
            return;
        }
    }
    private void EntryFocused(object sender, FocusEventArgs e) => ScoreScrollView.ScrollToAsync((VisualElement)sender, ScrollToPosition.Start, true);
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
                    UpdateEndTotals(currentEnd, currentRound.Ends, currentRound.Target.ZoneValues);
                    currentRound.RoundTotal = 0;
                    currentRound.XTotal = 0;
                    int currentEndIndex = currentRound.Ends.IndexOf(currentEnd);
                    for(int i = currentEndIndex; i < currentRound.Ends.Count; i++)
                    {
                        End end = currentRound.Ends[i];
                        UpdateEndTotals(end, currentRound.Ends,currentRound.Target.ZoneValues);
                        
                        
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
                    totalScore += end.Target.ZoneValues[end.Target.ZoneValues.Count-1];
                }
                //if there is zoneValues param (standard round) then use the maximum value of target
                if(zoneValues != null)
                {
                    totalScore += zoneValues[zoneValues.Count - 1];
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
            //save to db here.

            //send to API if not guest
            
            await Navigation.PopAsync();
        }

    }
}


