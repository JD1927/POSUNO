using POSUNO.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace POSUNO.Components
{
    public sealed partial class ProductsDialog : ContentDialog
    {
        public ProductsDialog(Product product)
        {
            InitializeComponent();
            Product = product;
            if (Product.IsEdit)
            {
                TitleTextBlock.Text = $"Editar el producto: {Product.Name}";
                PriceString = $"{product.Price}";
                StockString = $"{product.Stock}";
            } else
            {
                TitleTextBlock.Text = $"Nuevo cliente: {Product.Name}";
            }
        }

        public Product Product { get; set; }
        public string PriceString { get; set; }
        public string StockString { get; set; }

        private void CloseTapped(object sender, TappedRoutedEventArgs e)
        {
            Product.WasSaved = false;
            Hide();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Product.WasSaved = false;
            Hide();
        }

        private async void SaveClick(object sender, RoutedEventArgs e)
        {
            bool isValid = await ValidateFormAsync();
            if (!isValid)
            {
                return;
            }
            Product.WasSaved = true;
            Hide();
        }

        private async Task<bool> ValidateFormAsync()
        {
            MessageDialog messageDialog;

            if (string.IsNullOrEmpty(Product.Name))
            {
                messageDialog = new MessageDialog("Debes ingresar el nombre del producto", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(Product.Description))
            {
                messageDialog = new MessageDialog("Debes ingresar una descripción del producto.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }
            Product.Price = decimal.Parse(string.IsNullOrEmpty(PriceString) ? "0" : PriceString);
            if (Product.Price <= 0)
            {
                messageDialog = new MessageDialog("Debes ingresar precio válido para el producto.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }
            Product.Stock = float.Parse(string.IsNullOrEmpty(StockString) ? "0" : StockString);
            if (Product.Stock <= 0)
            {
                messageDialog = new MessageDialog("Debes ingresar stock válido para el producto.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }
            return true;
        }
    }
}
