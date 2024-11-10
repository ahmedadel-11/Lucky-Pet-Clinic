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
    /// Interaction logic for VisitsPage.xaml
    /// </summary>
    public partial class VisitsPage : INavigableView<VisitsViewModel>
    {
        public VisitsViewModel ViewModel { get; }

        public VisitsPage(VisitsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }
    }
}
