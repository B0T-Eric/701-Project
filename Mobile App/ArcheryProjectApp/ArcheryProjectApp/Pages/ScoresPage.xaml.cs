
using ArcheryLibrary;
using System.Collections.ObjectModel;
namespace ArcheryProjectApp.Pages;

/*To Do: Copy pretty much exactly what I made for Java Application in 601.
	AKA, Create Collection Views for Rounds (many rounds) and collection view inside the rounds for ends.
	Logic for taking inputted data, logic for restricting inputs per end/round based on target face, logic for displaying round/end details
 */
public partial class ScoresPage : ContentPage
{
    public class EndModel
    {
        public int EndNum { get; set; }
        public List<string> Arrows { get; set; }
        public int Xs { get; set; }
        public int EndTotal { get; set; }
        public int RunningTotal { get; set; }
    }
    private VerticalStackLayout mainLayout;
    public ScoresPage(Event userEvent)
    {
        InitializeComponent();
        mainLayout = RoundScoringLayout;
        InitializeRoundDisplays(userEvent);
    }

    private ObservableCollection<EndModel> InitializeEndModels(int shotsPerEnd, int numberOfEnds, List<int>? distancePerEnd)
    {
        var endModels = new ObservableCollection<EndModel>();
        for (int i = 0; i < numberOfEnds; i++)
        {
            var arrows = new List<string>();
            for (int j = 0; j < shotsPerEnd; j++)
            {
                arrows.Add("");
            }
            endModels.Add(new EndModel {  EndNum = i + 1, Arrows = arrows, Xs = 0, EndTotal = 0, RunningTotal = 0 });
        }
        return endModels;
    }

    private void InitializeRoundDisplays(Event _event)
    {
        foreach (Round round in _event.ScoreCard.Rounds)
        {
            var endModels = InitializeEndModels(round.ShotsPerEnd, round.EndCount,null);
            GenerateRoundCollectionView(round, _event.ScoreCard.Rounds.IndexOf(round), endModels);
        }
    }

    private void GenerateRoundCollectionView(Round round, int roundIndex, ObservableCollection<EndModel> endModels)
    {
        var roundVertical = new VerticalStackLayout();
        var roundLabel = new Label { Text = $"Round: {roundIndex + 1}", FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };
        roundVertical.Children.Add(roundLabel);
        var endCollection = GenerateEndCollectionView(round, endModels);
        roundVertical.Add(endCollection);
        Device.BeginInvokeOnMainThread(() =>
        {
            mainLayout.Children.Add(roundVertical);
        });
    }
    //need parameters for flint and standard differences
    private CollectionView GenerateEndCollectionView(Round round, ObservableCollection<EndModel> endModels)
    {
        var endsCollectionView = new CollectionView
        {
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical),
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnSpacing = 5, // Adds some spacing between columns
                    RowSpacing = 5     // Adds some spacing between rows
                };

                // Define the column widths
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // End number label on the left
                for (int i = 0; i < round.ShotsPerEnd; i++) // Arrow editor columns (centered)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
                // Columns for X's, End Total, and Running Total on the far right
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                // Setup the grid children
                // End number label (aligned left)
                var endNumLabel = new Label { HorizontalTextAlignment = TextAlignment.Start };
                endNumLabel.SetBinding(Label.TextProperty, "EndNum");
                Grid.SetColumn(endNumLabel, 0);
                grid.Children.Add(endNumLabel);

                // Arrow Editors (centered)
                for (int i = 0; i < round.ShotsPerEnd; i++)
                {
                    var arrowEditor = new Editor { Placeholder = "arrow", HorizontalTextAlignment = TextAlignment.Center };
                    arrowEditor.SetBinding(Editor.TextProperty, $"Arrows[{i}]");
                    arrowEditor.TextChanged += OnArrowTextChanged;
                    Grid.SetColumn(arrowEditor, i + 1);
                    grid.Children.Add(arrowEditor);
                }

                // Xs Label (aligned right)
                var xCountLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                xCountLabel.SetBinding(Label.TextProperty, "Xs");
                Grid.SetColumn(xCountLabel, round.ShotsPerEnd + 1);
                grid.Children.Add(xCountLabel);

                // End Total Label (aligned right)
                var endTotalLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                endTotalLabel.SetBinding(Label.TextProperty, "EndTotal");
                Grid.SetColumn(endTotalLabel, round.ShotsPerEnd + 2);
                grid.Children.Add(endTotalLabel);

                // Running Total Label (aligned right)
                var runningTotalLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
                runningTotalLabel.SetBinding(Label.TextProperty, "RunningTotal");
                Grid.SetColumn(runningTotalLabel, round.ShotsPerEnd + 3);
                grid.Children.Add(runningTotalLabel);

                // Wrap the grid in a frame for better visual separation
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
            //context.RunningTotal = CalculateRunningTotal();
        }
    }

    //private int CalculateRunningTotal()
    //{
    //    int runningTotal = 0;
    //    foreach (var end in endModels)
    //    {
    //        runningTotal += end.EndTotal;
    //        end.RunningTotal = runningTotal;
    //    }
    //    return runningTotal;
    //}
}


