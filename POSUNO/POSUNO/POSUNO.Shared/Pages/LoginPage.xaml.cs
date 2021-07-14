using POSUNO.Components;
using POSUNO.Helpers;
using POSUNO.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace POSUNO.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            EmailTextBox.Text = "ja@yopmail.com";
            PasswordBox.Password = "123456";
        }

        private async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog;
            bool isValid = await ValidForm();
            if (!isValid)
            {
                return;
            }
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            APIResponse response = await APIService.LoginAsync(new LoginRequest()
            {
                Email = EmailTextBox.Text,
                Password = PasswordBox.Password,
            });
            loader.Close();
            
            if (!response.IsSuccess)
            {
                messageDialog = new MessageDialog("Usuario y/o contraseña inválidos", "Error");
                await messageDialog.ShowAsync();
            }
            TokenResponse tokenResponse = (TokenResponse)response.Result;
            Frame.Navigate(typeof(MainPage), tokenResponse);
        }

        private async Task<bool> ValidForm()
        {
            MessageDialog messageDialog;

            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar tu email.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (!RegexUtilities.IsValidEmail(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar un email válido.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                messageDialog = new MessageDialog("Debes ingresar tu contraseña.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }
            return true;
        }
    }
}
