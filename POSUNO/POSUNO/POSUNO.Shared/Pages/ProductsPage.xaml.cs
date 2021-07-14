using POSUNO.Components;
using POSUNO.Helpers;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace POSUNO.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<Product> Products { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GetProductsAsync();
        }

        private async void GetProductsAsync()
        {
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            APIResponse response = await APIService.GetListAsync<Product>("products", MainPage.GetInstance().TokenResponse.Token);
            loader.Close();
            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            List<Product> products = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(products);
            RefreshList();
        }

        private void RefreshList()
        {
            ProductsListView.ItemsSource = null;
            ProductsListView.Items.Clear();
            ProductsListView.ItemsSource = Products;
        }

        private void EditTapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void DeleteTapped(object sender, TappedRoutedEventArgs e)
        {
        }
    }
}
