using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace Lucky_Pet_Clinic2.Views.Pages
{
    /// <summary>
    /// Interaction logic for PetsPage.xaml
    /// </summary>
    public partial class PetsPage : INavigableView<PetsViewModel>
    {
        public PetsViewModel ViewModel { get; }

        public PetsPage(PetsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            ViewModel.ClientId = NavigationData.ClientId.ToString();
            ViewModel.ClientName = NavigationData.ClientName;
            this.Loaded += OnPageLoaded;

        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {



            ViewModel.ClientId = NavigationData.ClientId.ToString();
            ViewModel.ClientName = NavigationData.ClientName;
            if ( ViewModel.SearchPetsCommand != null)
            {
                ViewModel.LoadPetsCommand.Execute(null);
                ViewModel.SearchPetsCommand.Execute(null);
            }
            else
            {
                System.Windows.MessageBox.Show("Commands are not initialized.");
            }

        }
    }


}
