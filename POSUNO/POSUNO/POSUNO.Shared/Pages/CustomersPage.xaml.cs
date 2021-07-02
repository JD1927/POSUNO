﻿using POSUNO.Components;
using POSUNO.Helpers;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public sealed partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<Customer> Customers { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GetCustomersAsync();
        }

        private async void GetCustomersAsync()
        {
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            APIResponse response = await APIService.GetListAsync<Customer>("customers");
            loader.Close();
            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            List<Customer> customers = (List<Customer>)response.Result;
            Customers = new ObservableCollection<Customer>(customers);
            RefreshList();
        }

        private void RefreshList()
        {
            CustomersListView.ItemsSource = null;
            CustomersListView.Items.Clear();
            CustomersListView.ItemsSource = Customers;
        }

        private async void AddCustomerClick(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer();
            CustomerDialog customerDialog = new CustomerDialog(customer);
            await customerDialog.ShowAsync();
            if (!customer.WasSaved)
            {
                return;
            }
            customer.User = MainPage.GetInstance().User;
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            APIResponse response = await APIService.PostAsync("customers", customer);
            loader.Close();
            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            Customer newCustomer = (Customer)response.Result;
            Customers.Add(newCustomer);
            RefreshList();
        }
        private async void EditTapped(object sender, TappedRoutedEventArgs e)
        {
            Customer customer = Customers[CustomersListView.SelectedIndex];
            customer.IsEdit = true;
            CustomerDialog customerDialog = new CustomerDialog(customer);
            await customerDialog.ShowAsync();

            if (!customer.WasSaved)
            {
                return;
            }
            customer.User = MainPage.GetInstance().User;
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            APIResponse response = await APIService.PutAsync("customers", customer, customer.Id);
            loader.Close();
            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            Customer newCustomer = (Customer)response.Result;
            Customer oldCustomer = Customers.FirstOrDefault(c => c.Id == newCustomer.Id);
            oldCustomer = newCustomer;
            RefreshList();
        }
        
        private void DeleteTapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
