using System.Collections.ObjectModel;
using System.Text;
using ArcheryLibrary;
using ArcheryProjectApp.Pages;
using CommunityToolkit.Maui.Views;

namespace ArcheryProjectApp;

public partial class RoundsPage : ContentPage
{
    public ObservableCollection<CompletedEventItemModel> CompletedEvents {  get; set; }
    public ObservableCollection<EventItemModel> EventItems { get; set; }
    public static RoundsPage instance;
    private int tapCount;
    private const int DoubleTapTime = 300;
    private bool isDoubleTapHandled = false;
	public RoundsPage()
	{

        InitializeComponent();
        DisplayItems();
        BindingContext = this;
        instance = this;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        DisplayItems();
    }

    public void DisplayItems()
    {
        EventItems = GetDisplayItems();
        CompletedEvents = GetCompletedItems();
        RoundsCollectionView.ItemsSource = EventItems;
        CompleteCollectionView.ItemsSource = CompletedEvents;
    }

    private ObservableCollection<CompletedEventItemModel>? GetCompletedItems()
    {
        ObservableCollection<CompletedEventItemModel> completeEvents = new ObservableCollection<CompletedEventItemModel>();
        foreach(Event _event in  ProfilePage.UserInstance.Events) 
        {
            if(_event.Rounds != null)
            {
                int roundTotals = 0;
                Target roundTarget = null;
                foreach (Round round in _event.Rounds)
                {
                    if(round.RoundTotal != 0)
                    {
                        roundTotals += round.RoundTotal;
                        if (round.Target != null && round.Type == "Standard")
                        {
                            roundTarget = round.Target;
                        }
                        else if (round.Type == "Flint")
                        {
                            roundTarget = round.Ends[0].Target;
                        }
                    }
                }
                float eventAverage = 0;
                if (roundTotals > 0 && _event.Rounds.Count > 0)
                {
                    eventAverage = (float)roundTotals / _event.Rounds.Count;
                }
                if(roundTarget == null)
                {
                    completeEvents.Add(new CompletedEventItemModel(_event.Name, _event.Date, _event.Type, _event.Environment, _event, _event.RoundCount, eventAverage, null));
                }
                else
                {
                    completeEvents.Add(new CompletedEventItemModel(_event.Name, _event.Date, _event.Type, _event.Environment, _event, _event.RoundCount, eventAverage, roundTarget));
                }
                
                
            }
        }
        return completeEvents;
    }
    private ObservableCollection<EventItemModel> GetDisplayItems()
    {
        ObservableCollection<EventItemModel> eventItemModels = new ObservableCollection<EventItemModel>();
        if(ProfilePage.UserInstance.Events != null)
        {
            foreach (Event e in ProfilePage.UserInstance.Events)
            {
                if(e.Rounds == null)
                {
                    EventItemModel model = new EventItemModel(e.Name, e.Date, e.Type, e.Environment, e);
                    eventItemModels.Add(model);
                }
            }
        }
        return eventItemModels;
    }
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        var createRoundPopup = new CreateEventPopup();
        createRoundPopup.RoundCreated += OnRoundCreated;
        this.ShowPopup(createRoundPopup);
    }
    private void OnRoundCreated(Event newEvent)
    {
        var newEventItemModel = new EventItemModel(newEvent.Name, newEvent.Date, newEvent.Type, newEvent.Environment, newEvent);
        EventItems.Add(newEventItemModel);
    }
    private async void OnItemTapped(object sender, EventArgs e)
    {
        tapCount++;
        if(isDoubleTapHandled)
        {
            return;
        }
        await Task.Delay(DoubleTapTime);
        if(tapCount == 1)
        {
            SingleTapStandardItem(sender);
        }
        else if(tapCount == 2) 
        {
            isDoubleTapHandled = true;
            DoubleTapItem(sender);
        }
        tapCount = 0;  
        isDoubleTapHandled = false;
    }
    private async void SingleTapStandardItem(object sender)
    {
        var frame = (Frame)sender;
        var tappedItem = frame.BindingContext as EventItemModel;
        if (tappedItem != null && tappedItem.UserEvent != null)
        {
            var popup = new RoundPopup(tappedItem.UserEvent, this.Navigation);
            await this.ShowPopupAsync(popup);
        }
    }
    private async void SingleTapCompleteItem(object sender)
    {
        var frame = (Frame)sender;
        var tappedItem = frame?.BindingContext as EventItemModel;
        if (tappedItem != null)
        {
            //if rounds have been set and created before open the scores page with this event.
            await Navigation.PushAsync(new ScoresPage(tappedItem.UserEvent));
        }
    }
    private async void DoubleTapItem(object sender)
    {
        var frame = (Frame)sender;
        var tappedItem = frame.BindingContext as EventItemModel;
        if (tappedItem != null && tappedItem.UserEvent != null)
        {
            var popup = new EventViewPopup(tappedItem.UserEvent, instance);
            await this.ShowPopupAsync(popup);
        }
    }
    private async void OnCompletedItemTapped(object sender, EventArgs e)
    {
        tapCount++;
        if (isDoubleTapHandled)
        {
            return;
        }
        await Task.Delay(DoubleTapTime);
        if (tapCount == 1)
        {
            SingleTapCompleteItem(sender);
        }
        else if(tapCount == 2)
        {
            isDoubleTapHandled = true;
            DoubleTapItem(sender);
        }
        tapCount = 0;
        isDoubleTapHandled = false;
    }
    private async void ToolbarHelp_Clicked(object sender, EventArgs e)
    {
        var popup = new InfoPopup();
        await this.ShowPopupAsync(popup);
    }
}
public class EventItemModel
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public string Type { get; set; }
    public string? Environment { get; set; }
    public Event UserEvent { get; set; }
    public EventItemModel(string name, DateTime date, string type, string environment, Event userEvent)
    {
        Name = name;
        Date = DateOnly.FromDateTime(date);
        Type = type;
        Environment = environment;
        UserEvent = userEvent;
    }
    public EventItemModel(string name, DateTime date, string type, Event userEvent) 
    {
        Name =name;
        Date = DateOnly.FromDateTime(date);
        Type = type;
        UserEvent=userEvent;
    }
}
public class CompletedEventItemModel : EventItemModel
{
    public int RoundCount { get; set; }
    public float RoundAverage { get; set; }
    public ImageSource? RoundTargetImage { get; set; }
    public CompletedEventItemModel(string name, DateTime date, string type, string environment, Event userEvent, int roundCount, float roundAverage, Target target) : base(name, date, type, environment, userEvent)
    {
        RoundCount = roundCount;
        RoundAverage = roundAverage;
        if(target != null) 
        {
            RoundTargetImage = target.FaceImage.Source; 
        }

       
    }
}


