// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace Lucky_Pet_Clinic2.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Lucky Pet Clinic";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            //new NavigationViewItem()
            //{
            //    Content = "Home",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            //    TargetPageType = typeof(Views.Pages.DashboardPage)
            //},
            new NavigationViewItem()
            {
                Content = "Data",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DataPage),
                Visibility = Visibility.Collapsed // Hides the item from the menu

            },
            new NavigationViewItem()
            {
                Content = "Clients",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ContactCard24 },
                TargetPageType = typeof(Views.Pages.ClientsPage)
            },
            new NavigationViewItem()
            {
                Content = "Visits",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.VisitsPage)
            },
            new NavigationViewItem()
            {
                Content = "Add Client",
                Icon = new SymbolIcon { Symbol = SymbolRegular.PersonAdd24 },
                TargetPageType = typeof(Views.Pages.AddClientPage)
            },
            new NavigationViewItem()
            {
                Content = "Add Pet",
                Icon = new SymbolIcon { Symbol = SymbolRegular.AnimalCat24 },
                TargetPageType = typeof(Views.Pages.AddPetPage),
                Visibility = Visibility.Collapsed // Hides the item from the menu
            },
            new NavigationViewItem()
            {
                Content = "Vets",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                TargetPageType = typeof(Views.Pages.AddVetPage)
            },

            new NavigationViewItem()
            {
                Content = "Examination",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Stethoscope24 },
                TargetPageType = typeof(Views.Pages.ExaminationPage),
                Visibility = Visibility.Collapsed // Hides the item from the menu

            },
            new NavigationViewItem()
            {
                Content = "Surgery",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Doctor24 },
                TargetPageType = typeof(Views.Pages.SurgeryPage),
                Visibility = Visibility.Collapsed // Hides the item from the menu
            },
            new NavigationViewItem()
            {
                Content = "Vaccination",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Syringe24 },
                TargetPageType = typeof(Views.Pages.VaccinationPage),
                Visibility = Visibility.Collapsed // Hides the item from the menu
            },
            new NavigationViewItem()
            {
                Content = "Pets",
                Icon = new SymbolIcon { Symbol = SymbolRegular.AnimalCat24 },
                TargetPageType = typeof(Views.Pages.PetsPage),
                Visibility = Visibility.Collapsed // Hides the item from the menu



            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
