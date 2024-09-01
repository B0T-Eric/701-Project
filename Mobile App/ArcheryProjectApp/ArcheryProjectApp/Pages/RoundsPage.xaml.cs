using System.Collections.ObjectModel;
using ArcheryLibrary;
using ArcheryProjectApp.Pages;
using CommunityToolkit.Maui.Views;

namespace ArcheryProjectApp;

public partial class RoundsPage : ContentPage
{
    public ObservableCollection<EventItemModel> EventItems { get; set; }
	public RoundsPage()
	{

        InitializeComponent();
        EventItems = GetDisplayItems();
        ChangeTitle();
        RoundsCollectionView.ItemsSource = EventItems;
        BindingContext = this;
        
    }

    private void ChangeTitle()
    {
        if (EventItems.Count > 0)
        {
            Rounds.Text = "Saved Rounds";
        }
        else
        {
            Rounds.Text = "Create Rounds by clicking the plus to the top right!";
        }
    }

    private ObservableCollection<EventItemModel> GetDisplayItems()
    {
        ObservableCollection<EventItemModel> eventItemModels = new ObservableCollection<EventItemModel>();
        if(ProfilePage.UserInstance.Events != null)
        {
            foreach (Event e in ProfilePage.UserInstance.Events)
            {
                EventItemModel model = new EventItemModel(e.Name, e.Date, e.Type, e.ScoreCard.Environment, e);
                eventItemModels.Add(model);
            }
        }
        return eventItemModels;
    }
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        var createRoundPopup = new CreateRoundPopup();
        createRoundPopup.RoundCreated += OnRoundCreated;
        this.ShowPopup(createRoundPopup);
    }
    private void OnRoundCreated(Event newEvent)
    {
        var newEventItemModel = new EventItemModel(newEvent.Name, newEvent.Date, newEvent.Type, newEvent.ScoreCard.Environment, newEvent);
        EventItems.Add(newEventItemModel);
        ChangeTitle();
    }
    private async void OnItemTapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var tappedItem = frame.BindingContext as EventItemModel;
        if(tappedItem != null)
        {
            this.ShowPopup(new ScoreCardPopup(tappedItem.UserEvent, Navigation));
        }
        
    }
    
}
public class EventItemModel
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public string Type { get; set; }
    public string Environment { get; set; }
    public Event UserEvent { get; set; }
    public EventItemModel(string name, DateOnly date, string type, string environment, Event userEvent)
    {
        Name = name;
        Date = date;
        Type = type;
        Environment = environment;
        UserEvent = userEvent;
    }
}


