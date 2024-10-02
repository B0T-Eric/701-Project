
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
            roundNum++;
            var roundVertical = new VerticalStackLayout();
            var roundLabel = new Label {Text=$"Round {roundNum}", FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };
            roundVertical.Children.Add(roundLabel);
            CollectionView endCollectionView = GenerateEndViews(round.Ends, round.Type, roundVertical);
            roundVertical.Children.Add(endCollectionView);
            mainLayout.Children.Add(roundVertical);
        }
    }

    private CollectionView GenerateEndViews(List<End> ends, string type, VerticalStackLayout roundLayout)
    {
        var endCollectionView = new CollectionView
        {
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical),
            ItemsSource = ends,
            ItemTemplate = new DataTemplate(() =>
            {
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
                    scoreEntry.TextChanged += OnScoreEntryChanged;
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
                    UpdateEndTotals(currentEnd,currentRound.Ends);
                }
                
            }
        }
    }

    private void UpdateEndTotals(End end, List<End> roundEnds)
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
                totalScore += 10; //placeholder value for target max value.
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
    private void ToolbarItem_Clicked(object sender, EventArgs e)
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

    }
}


