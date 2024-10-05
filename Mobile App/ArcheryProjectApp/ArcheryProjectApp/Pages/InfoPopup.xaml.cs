using CommunityToolkit.Maui.Views;

namespace ArcheryProjectApp.Pages;

public partial class InfoPopup : Popup
{
	public InfoPopup()
	{
		InitializeComponent();
	}
	private async void CloseButtonClicked(object sender, EventArgs e)
	{
		Close();
	}
}