using POSUNO.Models;
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
            Hide();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
