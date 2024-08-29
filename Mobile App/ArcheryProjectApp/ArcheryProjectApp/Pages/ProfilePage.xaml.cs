using ArcheryLibrary;
using static ArcheryProjectApp.SigninPage;

namespace ArcheryProjectApp;

[QueryProperty(nameof(LoginType),"loginType")]
public partial class ProfilePage : ContentPage
{ 
    public static User UserInstance;

    private string _loginType;

    public string LoginType
    {
        get => _loginType;
        set 
        { 
            _loginType = value; 
            OnPropertyChanged();
            if(Enum.TryParse(_loginType, out LoginType loginTypeEnum))
            {
                HandleLoginType(loginTypeEnum);
            }
        }
    }

    private void HandleLoginType(LoginType loginTypeEnum)
    {
        if(loginTypeEnum == SigninPage.LoginType.RegisteredUser)
        {
            //do registered user data handling?????
        }
        else
        {
            UserInstance = new User();
        }
    }

    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = this;
        if (UserInstance != null)
        {
            if(UserInstance.isGuest)
            {
                ModifyButtonText();
            }
            
        }
    }

    private void ModifyButtonText()
    {
        ProfileButton.Text = "Sign-Up";
    }
    private async void ProfileButtonClicked(object sender, EventArgs e)
    {
        if (UserInstance.isGuest && UserInstance != null)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}