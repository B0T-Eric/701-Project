using System.Collections.ObjectModel;
using ArcheryLibrary;

namespace ArcheryProjectApp;

public partial class RoundsPage : ContentPage
{
	public ObservableCollection<ScoreCardItem> Items { get; set; }
	public RoundsPage()
	{
		InitializeComponent();

		Items = new ObservableCollection<ScoreCardItem>
		{
            new ScoreCardItem("name",new DateOnly(2022,20,20),"target",100.00f,10.00f,"Indoor"),
		};
        var collectionView = new CollectionView
        {
            ItemsSource = Items,
            ItemTemplate = Application.Current.Resources["ItemTemplate"] as DataTemplate
        };
        Content = collectionView;
}
	public class ScoreCardItem
	{
		public string EventName { get; set; }
		public DateOnly Date { get; set; }
        public string Target { get; set; }
        public float TotalScore { get; set; }
        public float EndAverage { get; set; }
		public string Environment { get; set; }

        public ScoreCardItem(string eventName, DateOnly date, string target, float totalScore, float endAverage, string environment)
        {
            EventName = eventName;
            Date = date;
            Target = target;
            TotalScore = totalScore;
            EndAverage = endAverage;
            Environment = environment;
        }
    }
}

