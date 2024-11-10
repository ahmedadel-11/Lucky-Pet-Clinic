// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace Lucky_Pet_Clinic2.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            ViewModel.PetId = NavigationData.PetId;
            ViewModel.ClientName = NavigationData.ClientName;
            ViewModel.LoadData();
            ViewModel.SearchPets();
            InitializeComponent();
            this.Loaded += OnPageLoaded;

        }
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            // Refresh or update your ViewModel or UI here
            ViewModel.PetId = NavigationData.PetId;
            ViewModel.ClientName = NavigationData.ClientName;

            if (ViewModel.SearchPetsCommand != null)
            {
                ViewModel.RefreshCommand.Execute(null);
                ViewModel.SearchPetsCommand.Execute(null);
            }
            else
            {
                System.Windows.MessageBox.Show("Commands are not initialized.");
            }
        }
    }
}
