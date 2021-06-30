using POSUNO.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace POSUNO.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<Customer> Customers { get; set; }

        private void EditTapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void DeleteTapped(object sender, TappedRoutedEventArgs e)
        {
        }
    }
}
