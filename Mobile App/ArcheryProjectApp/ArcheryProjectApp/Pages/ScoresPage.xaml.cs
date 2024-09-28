
using ArcheryLibrary;
using System.Collections.ObjectModel;
namespace ArcheryProjectApp.Pages;

/*To Do: Copy pretty much exactly what I made for Java Application in 601.
	AKA, Create Collection Views for Rounds (many rounds) and collection view inside the rounds for ends.
	Logic for taking inputted data, logic for restricting inputs per end/round based on target face, logic for displaying round/end details
 */
public partial class ScoresPage : ContentPage
{
    ObservableCollection<EndModel> endModels;
    public class EndModel
    {
        public int EndNum { get; set; }
        public List<string> Arrows { get; set; }
        public int Xs { get; set; }
        public int EndTotal { get; set; }
        public int RunningTotal { get; set; }
        public string? Distance { get; set; }
        public Target? Target { get; set; }
        public string Type { get; set; }
    }
    private VerticalStackLayout mainLayout;
    public ScoresPage(Event userEvent)
    {
        InitializeComponent();
        mainLayout = RoundScoringLayout;
        InitializeRoundDisplays(userEvent);
    }

    private ObservableCollection<EndModel> InitializeEndModels(int shotsPerEnd, int numberOfEnds, List<End> ends)
    {
        var endModels = new ObservableCollection<EndModel>();
        for (int i = 0; i < numberOfEnds; i++)
        {
            var arrows = new List<string>();
            for (int j = 0; j < shotsPerEnd; j++)
            {
                arrows.Add("");
            }
            if (ends[i].Position == ShootingPosition.WalkBack)
            {
                endModels.Add(new EndModel { EndNum = i + 1, Type = "Walk Back", Arrows = arrows, Xs = 0, EndTotal = 0, RunningTotal = 0, Distance = ends[i].Distance, Target = ends[i].Target });
            }
            else if (ends[i].Position == ShootingPosition.WalkUp)
            {
                endModels.Add(new EndModel { EndNum = i + 1, Type = "Walk Up", Arrows = arrows, Xs = 0, EndTotal = 0, RunningTotal = 0, Distance = ends[i].Distance, Target = ends[i].Target });
            }
            else
            {
                endModels.Add(new EndModel { EndNum = i + 1, Type = "Stationary", Arrows = arrows, Xs = 0, EndTotal = 0, RunningTotal = 0, Distance = null, Target = null });
            }
        }
        return endModels;
    }

    private void InitializeRoundDisplays(Event _event)
    {
        foreach (Round round in _event.Rounds)
        {
            endModels = InitializeEndModels(round.ShotsPerEnd, round.EndCount,round.Ends);
            GenerateRoundCollectionView(round, _event.Rounds.IndexOf(round), endModels);
        }
    }

    private void GenerateRoundCollectionView(Round round, int roundIndex, ObservableCollection<EndModel> endModels)
    {
        var roundVertical = new VerticalStackLayout();
        var roundLabel = new Label { Text = $"Round: {roundIndex + 1}", FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };
        roundVertical.Children.Add(roundLabel);
        if(round.Type == "Standard")
        {
            var roundDetailLabel = new Label { Text = $"Distance: {round.Distance + 1} - Target: {round.Target.ToString()}", FontSize = 12, HorizontalTextAlignment = TextAlignment.Center };
            roundVertical.Children.Add(roundDetailLabel);
            var endCollection = GenerateStandardEndCollectionView(round, endModels);
            roundVertical.Add(endCollection);
        }
        else
        {
            var endCollection = GenerateFlintEndCollectionView(round, endModels);
            roundVertical.Add(endCollection);
        }
        
        Device.BeginInvokeOnMainThread(() =>
        {
            mainLayout.Children.Add(roundVertical);
        });
    }
    //need parameters for flint and standard differences
    private CollectionView GenerateFlintEndCollectionView(Round round, ObservableCollection<EndModel> endModels)
    {
        var endsCollectionView = new CollectionView
        {
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical),
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnSpacing = 5,
                    RowSpacing = 5     
                };

                // Define the column widths
                //end label
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                //Shooting Position label
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                //flint distance label
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                //flint target label
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                //arrows editors
                for (int i = 0; i < round.ShotsPerEnd; i++) 
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
                // Columns for X's, End Total, and Running Total  towards the right
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });


                //fill in grid
                //end label
                var endNumLabel = new Label { HorizontalTextAlignment = TextAlignment.Start };
                endNumLabel.SetBinding(Label.TextProperty, "EndNum");
                Grid.SetColumn(endNumLabel, 0);
                grid.Children.Add(endNumLabel);
                //end type
                var endTypeLabel = new Label { HorizontalTextAlignment = TextAlignment.Start };
                endNumLabel.SetBinding(Label.TextProperty, "Type");
                Grid.SetColumn(endTypeLabel, 1);
                grid.Children.Add(endTypeLabel);
                //end distance
                var endDistLabel = new Label { HorizontalTextAlignment = TextAlignment.Start };
                endNumLabel.SetBinding(Label.TextProperty, "Distance");
                Grid.SetColumn(endDistLabel, 2);
                grid.Children.Add(endDistLabel);
                //end target
                var endTargetLabel = new Label { HorizontalTextAlignment = TextAlignment.Start };
                endNumLabel.SetBinding(Label.TextProperty, "Target");
                Grid.SetColumn(endTargetLabel, 3);
                grid.Children.Add(endTargetLabel);

                // Arrow Editors
                for (int i = 0; i < round.ShotsPerEnd; i++)
                {
                    var arrowEditor = new Editor { Placeholder = "arrow", HorizontalTextAlignment = TextAlignment.Center };
                    arrowEditor.SetBinding(Editor.TextProperty, $"Arrows[{i}]");
                    arrowEditor.TextChanged += OnArrowTextChanged;
                    Grid.SetColumn(arrowEditor, i + 4);
                    grid.Children.Add(arrowEditor);
                }

                // Xs Label (aligned right)
                var xCountLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                xCountLabel.SetBinding(Label.TextProperty, "Xs");
                Grid.SetColumn(xCountLabel, round.ShotsPerEnd + 4);
                grid.Children.Add(xCountLabel);

                // End Total Label (aligned right)
                var endTotalLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                endTotalLabel.SetBinding(Label.TextProperty, "EndTotal");
                Grid.SetColumn(endTotalLabel, round.ShotsPerEnd + 5);
                grid.Children.Add(endTotalLabel);

                // Running Total Label (aligned right)
                var runningTotalLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                runningTotalLabel.SetBinding(Label.TextProperty, "RunningTotal");
                Grid.SetColumn(runningTotalLabel, round.ShotsPerEnd + 6);
                grid.Children.Add(runningTotalLabel);
                var endFrame = new Frame { Content = grid, Padding = 5, Margin = 5 };
                return endFrame;
            }),
            ItemsSource = endModels
        };

        return endsCollectionView;
    }
    private CollectionView GenerateStandardEndCollectionView(Round round, ObservableCollection<EndModel> endModels)
    {
        var endsCollectionView = new CollectionView
        {
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical),
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnSpacing = 5,
                    RowSpacing = 5
                };

                // Define the column widths
                //end label
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                //Shooting Position label
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                //arrows editors
                for (int i = 0; i < round.ShotsPerEnd; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
                // Columns for X's, End Total, and Running Total  towards the right
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });


                //fill in grid
                //end label
                var endNumLabel = new Label { HorizontalTextAlignment = TextAlignment.Start };
                endNumLabel.SetBinding(Label.TextProperty, "EndNum");
                Grid.SetColumn(endNumLabel, 0);
                grid.Children.Add(endNumLabel);
                //end type
                var endTypeLabel = new Label { HorizontalTextAlignment = TextAlignment.Start };
                endNumLabel.SetBinding(Label.TextProperty, "Type");
                Grid.SetColumn(endTypeLabel, 1);
                grid.Children.Add(endTypeLabel);
                // Arrow Editors
                for (int i = 0; i < round.ShotsPerEnd; i++)
                {
                    var arrowEditor = new Editor { Placeholder = "arrow", HorizontalTextAlignment = TextAlignment.Center };
                    arrowEditor.SetBinding(Editor.TextProperty, $"Arrows[{i}]");
                    arrowEditor.TextChanged += OnArrowTextChanged;
                    Grid.SetColumn(arrowEditor, i + 4);
                    grid.Children.Add(arrowEditor);
                }

                // Xs Label (aligned right)
                var xCountLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                xCountLabel.SetBinding(Label.TextProperty, "Xs");
                Grid.SetColumn(xCountLabel, round.ShotsPerEnd + 4);
                grid.Children.Add(xCountLabel);

                // End Total Label (aligned right)
                var endTotalLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                endTotalLabel.SetBinding(Label.TextProperty, "EndTotal");
                Grid.SetColumn(endTotalLabel, round.ShotsPerEnd + 5);
                grid.Children.Add(endTotalLabel);

                // Running Total Label (aligned right)
                var runningTotalLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                runningTotalLabel.SetBinding(Label.TextProperty, "RunningTotal");
                Grid.SetColumn(runningTotalLabel, round.ShotsPerEnd + 6);
                grid.Children.Add(runningTotalLabel);
                var endFrame = new Frame { Content = grid, Padding = 5, Margin = 5 };
                return endFrame;
            }),
            ItemsSource = endModels
        };

        return endsCollectionView;
    }


    private void OnArrowTextChanged(object? sender, TextChangedEventArgs e)
    {
        var editor = sender as Editor;
        var context = editor.BindingContext as EndModel;
        if (context != null)
        {
            context.Xs = context.Arrows.Count(a => a == "X");
            context.EndTotal = context.Arrows.Where(a => int.TryParse(a, out _)).Sum(a => int.Parse(a));
            context.RunningTotal = CalculateRunningTotal();
        }
    }

    private int CalculateRunningTotal()
    {
        int runningTotal = 0;
        foreach (var end in endModels)
        {
            runningTotal += end.EndTotal;
            end.RunningTotal = runningTotal;
        }
        return runningTotal;
    }
}


