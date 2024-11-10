using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using Lucky_Pet_Clinic2.Services;
using Lucky_Pet_Clinic2.Views.Pages;
using Lucky_Pet_Clinic2.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Wpf.Ui;
using static System.Collections.Specialized.NameObjectCollectionBase;
namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class ClientsViewModel : ObservableObject
    {
        private readonly IRepository<Clients> _clientRepository;
        private ObservableCollection<Clients> _clientsCollection;
        private Clients _selectedClient;
        private string _searchQuery;
        private ICollectionView _filteredClientsCollection;
        //private readonly SignalRService _signalRService;
        private bool _isLoading;
        private readonly INavigationService _navigationService;


        public ICommand NavigateToAddPetPageCommand { get; }
        public ICommand NavigateToPetsPageCommand { get; }
        public ICommand OpenClientsWindowCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand UpdateClientCommand { get; }

        private void ExecuteNavigateToAddPetPage()
        {

            if (SelectedClient != null)
            {
                // Pass the selected client or client ID when navigating to AddPetPage
                NavigationData.ClientId = SelectedClient.Id;
                NavigationData.ClientName = SelectedClient.Name;

                MessageBox.Show($"Navigating with ClientId: {NavigationData.ClientId}"); // Debugging


                //_navigationService.Navigate(typeof(AddPetPage), viewModel);

                _navigationService.Navigate(typeof(AddPetPage));
            }
            else
            {
                // Handle case where no client is selected (optional)
                MessageBox.Show("Please select a client first.");
            }
        }

        private void ExecuteNavigateToPetsPage()
        {
            if (SelectedClient != null)
            {
                // Pass the selected client ID when navigating to PetsPage
                NavigationData.ClientId = SelectedClient.Id;
                NavigationData.ClientName = SelectedClient.Name;

                MessageBox.Show($"Navigating to PetsPage with ClientId: {NavigationData.ClientId}"); // Debugging
                _navigationService.Navigate(typeof(PetsPage));

            }
            else
            {
                // Handle case where no client is selected (optional)
                MessageBox.Show("Please select a client first.");
            }
        }


        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        public ObservableCollection<Clients> ClientsCollection
        {
            get => _clientsCollection;
            set => SetProperty(ref _clientsCollection, value);
        }



        public ICollectionView FilteredClientsCollection
        {
            get => _filteredClientsCollection;
            private set => SetProperty(ref _filteredClientsCollection, value);
        }

        //public Clients SelectedClient
        //{
        //    get => _selectedClient;
        //    set => SetProperty(ref _selectedClient, value);
        //}
        public Clients SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }

        private DispatcherTimer _debounceTimer;

        private readonly IRepository<Vets> _vetsRepository;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SetProperty(ref _searchQuery, value);
                if (_debounceTimer != null)
                {
                    _debounceTimer.Stop();
                }
                _debounceTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(500)
                };
                _debounceTimer.Tick += (s, e) =>
                {
                    FilteredClientsCollection.Refresh();
                    _debounceTimer.Stop();
                };
                _debounceTimer.Start();
            }
        }

        public ClientsViewModel(IRepository<Clients> clientRepository, INavigationService navigationService, IRepository<Vets> vetRepository)
        {
            _navigationService = navigationService;
            NavigateToAddPetPageCommand = new RelayCommand(ExecuteNavigateToAddPetPage);
            NavigateToPetsPageCommand = new RelayCommand(ExecuteNavigateToPetsPage); // Add PetsPage navigation command
            OpenClientsWindowCommand = new RelayCommand<int>(OpenClinetsWindow);
            DeleteClientCommand = new RelayCommand<Clients>(DeleteClient);
            UpdateClientCommand = new RelayCommand<Clients>(UpdateClient);
            _clientRepository = clientRepository;
            _vetsRepository = vetRepository;

            //_signalRService = signalRService;
            //_signalRService.OnUpdateReceived += HandleUpdateReceived;

            RefreshCommand = new RelayCommand(async () => await LoadClients());

            LoadClients();





        }

        private async void OpenClinetsWindow(int clientId)
        {

            var selectedClient = ClientsCollection.Where(e => e.Id == clientId).FirstOrDefault();
            if (selectedClient != null)
            {
                SelectedClient = selectedClient; // Set the selected client
                var window = new ClientsWindow
                {
                    DataContext = this // Set the ViewModel as DataContext
                };
                window.Show();
            }
            else
            {
                System.Windows.MessageBox.Show("Client not found!");
            }

        }



        public async Task LoadClients()
        {
            IsLoading = true;  // Start loading
            var clients = await _clientRepository.GetAllAsync();
            ClientsCollection = new ObservableCollection<Clients>(clients);
            InitializeFilteredCollection();
            FilteredClientsCollection.Refresh();
            IsLoading = false;  // Stop loading


        }
        private async void HandleUpdateReceived()
        {
            // Reload clients when an update is received
            await LoadClients();
        }


        private void InitializeFilteredCollection()
        {
            _filteredClientsCollection = CollectionViewSource.GetDefaultView(ClientsCollection);
            _filteredClientsCollection.Filter = FilterClients;
        }

        private bool FilterClients(object obj)
        {
            if (obj is Clients client)
            {
                if (string.IsNullOrWhiteSpace(SearchQuery))
                    return true;

                // Apply the filter based on ID, Name, or Mobile Number
                return client.Id.ToString().Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                       client.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                       client.MobileNumber.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public ICommand OpenWhatsAppCommand => new RelayCommand<string>(OpenWhatsApp);

        private void OpenWhatsApp(string mobileNumber)
        {
            string url = $"https://wa.me/+966{mobileNumber}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private ClientsWindow _clientsWindow; // Reference to the window


        private async void DeleteClient(Clients client)
        {
            if (client != null)
            {
                // Confirm delete operation
                var result = MessageBox.Show($"Are you sure you want to delete client {client.Name}?",
                                              "Confirm Delete", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    await _clientRepository.RemoveByIdAsync(client.Id);
                    ClientsCollection.Remove(client); // Remove from the collection

                }
            }
            else
            {
                MessageBox.Show("No client selected!");
            }
        }

        private async void UpdateClient(Clients client)
        {
            if (client != null)
            {
                // Example update: Client data already bound to the view
                await _clientRepository.UpdateAsync(client); // Assuming you have an UpdateAsync method in the repository
                MessageBox.Show($"Client {client.Name} updated successfully!");

                // Refresh the list after updating (optional)
                await LoadClients();
            }
            else
            {
                MessageBox.Show("No client selected to update!");
            }
        }


    }
}
