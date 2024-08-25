using System.Collections.ObjectModel;
using ArcheryLibrary;
using ArcheryProjectApp.Pages;
using CommunityToolkit.Maui.Views;

namespace ArcheryProjectApp;

public partial class RoundsPage : ContentPage
{
	public ObservableCollection<ScoreCardItem> Items { get; set; }
	public RoundsPage()
	{

        InitializeComponent();

        Items = new ObservableCollection<ScoreCardItem>
        {
            new ScoreCardItem("Round One",new DateOnly(2022,12,20),"Target One",100,10,"Indoor"),
            new ScoreCardItem("Round Two",new DateOnly(2022,12,21),"Target Two",10,1,"Indoor"),
            new ScoreCardItem("Round Three",new DateOnly(2022,12,22),"Target Three",300,30,"Indoor"),
            new ScoreCardItem("Round Four",new DateOnly(2022,12,23),"Target Four",89,22,"Indoor"),
        };
        BindingContext = this;

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

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new CreateRoundPopup());
    }
}

