using POSUNO.Helpers;
using POSUNO.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace POSUNO.Components
{
    public sealed partial class CustomerDialog : ContentDialog
    {
        public CustomerDialog(Customer customer)
        {
            InitializeComponent();
            Customer = customer;
            TitleTextBlock.Text = Customer.IsEdit ? $"Editar el cliente: {Customer.FullName}" : $"Nuevo cliente: {Customer.FullName}";
        }

        public Customer Customer { get; set; }

        
        private void CloseTapped(object sender, TappedRoutedEventArgs e)
        {
            Customer.WasSaved = false;
            Hide();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Customer.WasSaved = false;
            Hide();
        }

        private async void SaveClick(object sender, RoutedEventArgs e)
        {
            bool isValid = await ValidateFormAsync();
            if (!isValid)
            {
                return;
            }
            Customer.WasSaved = true;
            Hide();
        }

        private async Task<bool> ValidateFormAsync()
        {
            MessageDialog messageDialog;

            if (string.IsNullOrEmpty(Customer.FirstName))
            {
                messageDialog = new MessageDialog("Debes ingresar nombres del cliente.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(Customer.LastName))
            {
                messageDialog = new MessageDialog("Debes ingresar apellidos del cliente.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(Customer.PhoneNumber))
            {
                messageDialog = new MessageDialog("Debes ingresar teléfono del cliente.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(Customer.Address))
            {
                messageDialog = new MessageDialog("Debes ingresar dirección del cliente.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(Customer.Email))
            {
                messageDialog = new MessageDialog("Debes ingresar email del cliente.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (!RegexUtilities.IsValidEmail(Customer.Email))
            {
                messageDialog = new MessageDialog("Debes ingresar un email válido.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }
            return true;
        }
    }
}
