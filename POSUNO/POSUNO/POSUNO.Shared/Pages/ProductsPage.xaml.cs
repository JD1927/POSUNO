using POSUNO.Components;
using POSUNO.Helpers;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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

        private async void EditTapped(object sender, TappedRoutedEventArgs e)
        {
            Product product = Products[ProductsListView.SelectedIndex];
            product.IsEdit = true;
            ProductsDialog productDialog = new ProductsDialog(product);
            await productDialog.ShowAsync();

            if (!product.WasSaved)
            {
                return;
            }
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            APIResponse response = await APIService.PutAsync("products", product, product.Id, MainPage.GetInstance().TokenResponse.Token);
            loader.Close();
            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            Product newProduct = (Product)response.Result;
            Product oldProduct = Products.FirstOrDefault(p => p.Id == newProduct.Id);
            oldProduct = newProduct;
            RefreshList();
        }

        private async void DeleteTapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialogResult result = await ConfirmDeleteAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            Loader loader = new Loader("Por favor espere...");
            loader.Show();

            Product currentProduct = Products[ProductsListView.SelectedIndex];
            APIResponse response = await APIService.DeleteAsync("products", currentProduct.Id, MainPage.GetInstance().TokenResponse.Token);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }
            List<Product> products = Products.Where(p => p.Id != currentProduct.Id).ToList();
            Products = new ObservableCollection<Product>(products);
            RefreshList();
        }

        private async Task<ContentDialogResult> ConfirmDeleteAsync()
        {
            ContentDialog confirmDialog = new ContentDialog()
            {
                Title = "Confirmación",
                Content = "¿Estás seguro de que deseas eliminar el registro?",
                PrimaryButtonText = "Sí",
                CloseButtonText = "No"
            };

            return await confirmDialog.ShowAsync();
        }

        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            ProductsDialog productDialog = new ProductsDialog(product);
            await productDialog.ShowAsync();
            if (!product.WasSaved)
            {
                return;
            }
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            APIResponse response = await APIService.PostAsync("products", product, MainPage.GetInstance().TokenResponse.Token);
            loader.Close();
            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            Product newProduct = (Product)response.Result;
            Products.Add(newProduct);
            RefreshList();
        }
    }
}
