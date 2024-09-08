
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

    private ObservableCollection<EndModel> InitializeEndModels(int shotsPerEnd, int numberOfEnds)
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
            var endModels = InitializeEndModels(round.ShotsPerEnd, round.EndCount);
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
    private CollectionView GenerateEndCollectionView(Round round,ObservableCollection<EndModel> endModels)
    {
        var endsCollectionView = new CollectionView
        {

            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical),
            ItemTemplate = new DataTemplate(() =>
            {
                var endFrame = new Frame();
                var endHorizontal = new HorizontalStackLayout();
                var endNumLabel = new Label {Margin = 4 , FontAutoScalingEnabled = true };
                endNumLabel.SetBinding(Label.TextProperty, "EndNum");
                endHorizontal.Children.Add(endNumLabel);
                var targetDistContainer = new VerticalStackLayout { };
                var endTargetNameLabel = new Label { Text = $"{round.Target}" , FontAutoScalingEnabled = true,  FontSize = 8};
                var endDistanceLabel = new Label { Text = $"{round.Distance}", FontAutoScalingEnabled = true, FontSize = 8 };
                targetDistContainer.Children.Add(endDistanceLabel);
                targetDistContainer.Children.Add(endTargetNameLabel);
                endHorizontal.Children.Add(targetDistContainer);


                for (int i = 0; i < round.ShotsPerEnd; i++)
                {
                    Console.WriteLine($"Added Arrow Editor number {i}");
                    var arrowEditor = new Editor { Margin = 4 };
                    arrowEditor.SetBinding(Editor.TextProperty, $"Arrow[{i}]");
                    arrowEditor.Placeholder = "arrow";
                    //arrowEditor.TextChanged += OnArrowTextChanged;
                    endHorizontal.Children.Add(arrowEditor);
                }

                var endXCountLabel = new Label {Margin = 4, HorizontalTextAlignment = TextAlignment.End, FontAutoScalingEnabled = true };
                endXCountLabel.SetBinding(Label.TextProperty, "xCount");
                endHorizontal.Children.Add(endXCountLabel);

                var endTotalLabel = new Label { Margin = 4, HorizontalTextAlignment = TextAlignment.End, FontAutoScalingEnabled = true };
                endTotalLabel.SetBinding(Label.TextProperty, "EndTotal");
                endHorizontal.Children.Add(endTotalLabel);

                var runningTotalLabel = new Label { Margin=4, HorizontalTextAlignment = TextAlignment.End, FontAutoScalingEnabled = true };
                runningTotalLabel.SetBinding(Label.TextProperty, "RunningTotal");
                endHorizontal.Children.Add(runningTotalLabel);

                endFrame.Content = endHorizontal;
                return endFrame;

            }),
            ItemsSource = endModels
        };
        
        return endsCollectionView;
    }

    //private void OnArrowTextChanged(object? sender, TextChangedEventArgs e)
    //{
    //    var editor = sender as Editor;
    //    var context = editor.BindingContext as EndModel;
    //    if (context != null)
    //    {
    //        context.Xs = context.Arrows.Count(a => a == "X");
    //        context.EndTotal = context.Arrows.Where(a => int.TryParse(a, out _)).Sum(a => int.Parse(a));
    //        context.RunningTotal = CalculateRunningTotal();
    //    }
    //}

    //private int CalculateRunningTotal()
    //{
    //    int runningTotal = 0;
    //    foreach(var end in endModels)
    //    {
    //        runningTotal += end.EndTotal;
    //        end.RunningTotal = runningTotal;
    //    }
    //    return runningTotal;
    //}
}


