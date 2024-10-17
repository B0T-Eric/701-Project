using ArcheryLibrary;

namespace ArcheryProjectApp;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
        if(ProfilePage.UserInstance.Events != null)
        {
            DisplayStats();
        }
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        DisplayStats();
    }
    private void DisplayStats()
	{
		//display weekly round average
		WeeklyAverageText.Text = GetWeeklyAverage();
		//display monthly round average
		MonthlyAverageText.Text = GetMonthlyAverage();
		//display yearly round average
		YearlyAverageText.Text = GetYearlyAverage();
		//display total scores
		TotalScoresText.Text = GetTotalScores();
		//display best round
		HighestRoundTotalText.Text = GetHighestRound();
		//display worst round
		LowestRoundTotalText.Text = GetLowestRound();
		//display best event
		HighestEventScoreText.Text = GetHighestEvent();
		//display worst event
		LowestEventScoreText.Text = GetLowestEvent();
	}

    private string GetLowestEvent()
    {
        List<int> lowest = new List<int>();
        if (ProfilePage.UserInstance.Events != null)
        {
            foreach (Event _event in ProfilePage.UserInstance.Events)
            {
                if (_event.Rounds != null && _event.Rounds.Count > 0)
                {
                    int low = _event.EventTotal;
                    lowest.Add(low);
                }
                else
                {
                    return "0";
                }

            }
        }
   
        if(lowest.Count > 0)
        {
            int lowScore = lowest.Min();
            return $"{lowScore}";
        }
        else
        {
            return "0";
        }
        
    }

    private string GetHighestRound()
    {
        int highScore = 0;
        List<int> highest = new List<int>();
        if (ProfilePage.UserInstance.Events != null)
        {
            foreach (Event _event in ProfilePage.UserInstance.Events)
            {
                if ( _event.Rounds != null && _event.Rounds.Count > 0)
                {
                    int high = _event.Rounds.Max(r => r.RoundTotal);
                    highest.Add(high);
                }
            }
        }
        if(highest.Count > 0)
        {
            highScore = highest.Max();
            return highScore.ToString();
        }
        else
        {
            return "0";
        }
    }

    private string GetHighestEvent()
    {
        List<int> highest = new List<int>();
        if (ProfilePage.UserInstance.Events != null)
        {
            foreach (Event e in ProfilePage.UserInstance.Events)
            {
                if ( e.Rounds != null && e.Rounds.Count > 0)
                {
                    int high = e.GetEventTotal();
                    highest.Add(high);
                }

            }
        }
        if(highest.Count > 0) 
        {
            int highScore = highest.Max();
            return highScore.ToString();
        }
        else
        {
            return "0";
        }
        
    }

    private string GetLowestRound()
    {
        int lowScore = 0;
        List<int> lowest = new List<int>();
        if (ProfilePage.UserInstance.Events != null)
        {
            foreach (Event _event in ProfilePage.UserInstance.Events)
            {
                if ( _event.Rounds != null && _event.Rounds.Count > 0)
                {
                    int low = _event.Rounds.Min(r => r.RoundTotal);
                    lowest.Add(low);
                }
                else
                {
                    return "0";
                }
            }
        }
        if(lowest.Count > 0)
        {
            lowScore = lowest.Min();
            return $"{lowScore}";
        }
        else
        {
            return "0";
        }
        
    }

    private string GetYearlyAverage()
    {
        int total = 0;
        int roundCount = 0;
        DateTime currentDate = DateTime.Now;
        DateTime weekPrior = currentDate.AddYears(-1);
        if (ProfilePage.UserInstance.Events != null)
        {
            foreach (Event _event in ProfilePage.UserInstance.Events)
            {
                if (_event.Rounds != null)
                {
                    if (_event.Date >= weekPrior && _event.Date <= currentDate)
                    {
                        foreach (Round round in _event.Rounds)
                        {
                            total += round.RoundTotal;
                        }
                        roundCount += _event.RoundCount;
                    }
                }
            }
        }
        if(total > 0 && roundCount > 0)
        {
            int average = total / roundCount;
            return $"{total} across {roundCount} rounds = {average}";
        }
        else
        {
            return "0";
        }
        
    }

    private string GetTotalScores()
    {
        int total = 0;
        int roundCount =0;
        if (ProfilePage.UserInstance.Events != null)
        {
            foreach (Event _event in ProfilePage.UserInstance.Events)
            {
                if (_event.RoundCount > 0)
                {
                    roundCount += _event.RoundCount;
                    if (_event.Rounds != null)
                    {
                        foreach (Round round in _event.Rounds)
                        {
                            total += round.RoundTotal;
                        }
                    }

                }

            }
        }
        if(total > 0 && roundCount > 0)
        {
            return $"{total} across {roundCount} rounds.";
        }
        else
        {
            return "0";
        }
        
    }

    private string GetMonthlyAverage()
    {
        int total = 0;
        int roundCount = 0;
        DateTime currentDate = DateTime.Now;
        DateTime weekPrior = currentDate.AddMonths(-1);
        if (ProfilePage.UserInstance.Events != null)
        {
            foreach (Event _event in ProfilePage.UserInstance.Events)
            {
                if (_event.Date >= weekPrior && _event.Date <= currentDate)
                {
                    if (_event.Rounds != null)
                    {
                        foreach (Round round in _event.Rounds)
                        {
                            total += round.RoundTotal;
                        }
                        roundCount += _event.RoundCount;
                    }
                }
            }
        }
        if (total > 0 && roundCount > 0)
        {
            int average = total / roundCount;
            return $"{total} across {roundCount} rounds = {average}";
        }
        else
        {
            return "0";
        }

    }

    private string GetWeeklyAverage()
    {
        int total = 0;
        int roundCount = 0;
        DateTime currentDate = DateTime.Now;
        DateTime weekPrior = currentDate.AddDays(-7);
        if (ProfilePage.UserInstance.Events != null)
        {
            foreach (Event _event in ProfilePage.UserInstance.Events)
            {
                if (_event.Date >= weekPrior && _event.Date <= currentDate)
                {
                    if (_event.Rounds != null)
                    {
                        foreach (Round round in _event.Rounds)
                        {
                            total += round.RoundTotal;
                        }
                        roundCount += _event.RoundCount;
                    }
                }
            }
        }
       
        if(total > 0 && roundCount > 0)
        {
            int average = total / roundCount;
            return $"{total} across {roundCount} rounds = {average}";
        }
        else
        {
            return $"0";
        }
    }
}