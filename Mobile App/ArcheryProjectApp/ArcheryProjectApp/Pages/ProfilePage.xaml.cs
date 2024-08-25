using ArcheryLibrary;

namespace ArcheryProjectApp;

public partial class ProfilePage : ContentPage
{
    public static User UserInstance;
    public ProfilePage(User login)
    {
        InitializeComponent();

        NavigationPage.SetHasBackButton(this, false);



        UserInstance = login;
        if (UserInstance.isGuest)
        {
            ModifyButtonText();
        }
    }

    private void ModifyButtonText()
    {
        ProfileButton.Text = "Sign-Up";
    }
    private async void ProfileButtonClicked(object sender, EventArgs e)
    {
        if (UserInstance.isGuest)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}