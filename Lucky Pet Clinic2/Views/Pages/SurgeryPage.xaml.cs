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
    /// Interaction logic for SurgeryPage.xaml
    /// </summary>
    public partial class SurgeryPage : INavigableView<SurgeryViewModel>
    {
        public SurgeryViewModel ViewModel { get; }

        public SurgeryPage(SurgeryViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
            ViewModel.PetID = NavigationData.PetId;

            this.Loaded += OnPageLoaded;

        }
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            // Refresh or update your ViewModel or UI here
            ViewModel.PetID = NavigationData.PetId;
            //ViewModel.SelectedPetType = null; // Reset pet type
            //ViewModel.SelectedGender = null; // Reset gender
            //ViewModel.PetName = null; // Reset pet name
            //ViewModel.DateOfBirth = null; // Reset birth date

            // Add any other refreshing logic here
        }
    }
}
