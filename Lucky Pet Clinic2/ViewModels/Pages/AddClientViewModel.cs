using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Lucky_Pet_Clinic2.Views.Pages;
using Wpf.Ui;
using System.Windows.Navigation;
using Lucky_Pet_Clinic2.Services;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class AddClientViewModel : ObservableObject, INotifyDataErrorInfo
    {
        //private readonly SignalRService _signalRService;


        private readonly IRepository<Clients> _clientRepository;
        private string _clientName;
        private string _number;
        private string _address;
        private readonly INavigationService _navigationService;

        public ICommand NavigateToAddPetPageCommand { get; }
        public ICommand SaveCommand { get; }
        public AddClientViewModel(IRepository<Clients> clientRepository, INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToAddPetPageCommand = new RelayCommand(ExecuteNavigateToAddPetPage);

            _clientRepository = clientRepository;
            SaveCommand = new RelayCommand(async () => await SaveClient(), CanSaveClient);
            //_signalRService = signalRService;
        }
        private void ExecuteNavigateToAddPetPage()
        {

            _navigationService.Navigate(typeof(AddPetPage));
        }

        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }






        private async Task SaveClient()
        {
            if (MessageBox.Show("Do you want to save this client?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    var client = new Clients
                    {
                        Name = ClientName,
                        MobileNumber = Number,
                        Address = Address
                    };

                    await _clientRepository.AddAsync(client); // Save asynchronously
                    MessageBox.Show($"Client data saved successfully with ID: {client.Id}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    //await _signalRService.SendUpdateAsync();

                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private bool CanSaveClient(object parameter)
        {
            return !string.IsNullOrWhiteSpace(ClientName) &&
                   !string.IsNullOrWhiteSpace(Number) &&
                   !string.IsNullOrWhiteSpace(Address) &&
                   !HasErrors;
        }

        private void ClearForm()
        {
            ClientName = string.Empty;
            Number = string.Empty;
            Address = string.Empty;
        }

        // Validation logic
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        public bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                yield return _errors[propertyName];
            }
        }


        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
