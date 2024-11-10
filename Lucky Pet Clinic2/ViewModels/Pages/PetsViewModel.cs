using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using Lucky_Pet_Clinic2.Views.Pages;
using Lucky_Pet_Clinic2.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Wpf.Ui;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class PetsViewModel : ObservableObject
    {
            
        private Pets _selectedpet;
        private ObservableCollection<Pets> _petsCollection;
        private Pets _selectedPet;

        private readonly INavigationService _navigationService;

        private readonly IRepository<Pets> _petsRepository;
        private string _petId;
        public string PetId
        {
            get => _petId;
            set => SetProperty(ref _petId, value);
        }

        private string _clientId;
        public string ClientId
        {
            get => _clientId;
            set => SetProperty(ref _clientId, value);
        }
        private string _clientName;
        public string ClientName
        {
            get => _clientName;
            set => SetProperty(ref _clientName, value);
        }

        public ObservableCollection<Pets> PetsCollection
        {
            get => _petsCollection;
            set => SetProperty(ref _petsCollection, value);
        }
        public Pets SelectedPet
        {
            get { return _selectedPet; }
            set
            {
                _selectedPet = value;
                OnPropertyChanged(nameof(SelectedPet));
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand OpenPetWindowCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand UpdateClientCommand { get; }

        public PetsViewModel(IRepository<Pets> petsRepository, INavigationService navigationService)
        {
            _petsRepository = petsRepository;
            LoadPets();
            Pets = new ObservableCollection<Pets>();
            SearchPetsCommand = new RelayCommand(() => SearchPets()); // No need for async/await here
            _navigationService = navigationService;
            NavigateToSurgeryPageCommand = new RelayCommand(ExecuteNavigateToSurgeryPage);
            NavigateToVaccinationPageCommand = new RelayCommand(ExecuteNavigateToVaccinationPage);
            NavigateToExaminationPageCommand = new RelayCommand(ExecuteNavigateToExaminationPage);
            NavigateToDataPageCommand = new RelayCommand(ExecuteNavigateToDataPage);
            DeleteClientCommand = new RelayCommand<Pets>(DeleteClient);
            UpdateClientCommand = new RelayCommand<Pets>(UpdateClient);

            LoadPetsCommand = new RelayCommand(async () => await LoadPets());
            SearchPetsCommand = new RelayCommand(FilterPetsByClient);

            RefreshCommand = new RelayCommand(async () => await LoadPets());

            OpenPetWindowCommand = new RelayCommand<int>(OpenPetWindow);





        }


        private void ExecuteNavigateToSurgeryPage()
        {

            if (SelectedPet != null)
            {
                // Pass the selected client or client ID when navigating to AddPetPage
                NavigationData.PetId = SelectedPet.Id;

                MessageBox.Show($"Navigating with PetId: {NavigationData.PetId}"); // Debugging


                //_navigationService.Navigate(typeof(AddPetPage), viewModel);

                _navigationService.Navigate(typeof(SurgeryPage));
            }
            else
            {
                // Handle case where no client is selected (optional)
                MessageBox.Show("Please select a client first.");
            }
        }
        private void ExecuteNavigateToVaccinationPage()
        {

            if (SelectedPet != null)
            {
                // Pass the selected client or client ID when navigating to AddPetPage
                NavigationData.PetId = SelectedPet.Id;

                MessageBox.Show($"Navigating with PetId: {NavigationData.PetId}"); // Debugging


                //_navigationService.Navigate(typeof(AddPetPage), viewModel);

                _navigationService.Navigate(typeof(VaccinationPage));
            }
            else
            {
                // Handle case where no client is selected (optional)
                MessageBox.Show("Please select a client first.");
            }
        }
        private void ExecuteNavigateToExaminationPage()
        {

            if (SelectedPet != null)
            {
                // Pass the selected client or client ID when navigating to AddPetPage
                NavigationData.PetId = SelectedPet.Id;

                MessageBox.Show($"Navigating with PetId: {NavigationData.PetId}"); // Debugging


                //_navigationService.Navigate(typeof(AddPetPage), viewModel);

                _navigationService.Navigate(typeof(ExaminationPage));
            }
            else
            {
                // Handle case where no client is selected (optional)
                MessageBox.Show("Please select a client first.");
            }
        }
        private void ExecuteNavigateToDataPage()
        {

            if (SelectedPet != null)
            {
                // Pass the selected client or client ID when navigating to AddPetPage
                NavigationData.PetId = SelectedPet.Id;

                MessageBox.Show($"Navigating with PetId: {NavigationData.PetId}"); // Debugging


                //_navigationService.Navigate(typeof(AddPetPage), viewModel);

                _navigationService.Navigate(typeof(DataPage));
            }
            else
            {
                // Handle case where no client is selected (optional)
                MessageBox.Show("Please select a client first.");
            }
        }

        private async void OpenPetWindow(int clientId)
        {

            var selectedPet = PetsCollection.Where(e => e.Id == clientId).FirstOrDefault();
            if (selectedPet != null)
            {
                SelectedPet = selectedPet; // Set the selected client
                var window = new PetWindow
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


        public ObservableCollection<Pets> Pets { get; private set; }

        public ICommand LoadPetsCommand { get; }
        public ICommand SearchPetsCommand { get; }
        public ICommand NavigateToSurgeryPageCommand { get; }
        public ICommand NavigateToVaccinationPageCommand { get; }
        public ICommand NavigateToExaminationPageCommand { get; }
        public ICommand NavigateToDataPageCommand { get; }

        private IEnumerable<Pets> _petsList; // Store the loaded pets

        public async Task LoadPets()
        {

            //var petsList = await _petsRepository.GetPetsWithClientsAsync(); // Load pets with clients
            _petsList = await _petsRepository.GetPetsWithClientsAsync(); // Load pets with clients
            PetsCollection = new ObservableCollection<Pets>(_petsList);
            if (_petsList == null || PetsCollection ==null)
            {
                // Data not loaded yet, show error or handle accordingly
                MessageBox.Show("Data is still loading. Please try again in a moment.", "Data Not Loaded", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Pets.Clear();
            //foreach (var pet in petsList)
            foreach (var pet in _petsList)
            {
                pet.CalculateAge(); // Calculate age for each pet

                Pets.Add(pet);
            }
            FilterPetsByClient();
        }


        public void SearchPets()
        {
            IEnumerable<Pets> filteredPets;

            if (!string.IsNullOrEmpty(PetId) && !string.IsNullOrEmpty(ClientId))
            {
                // Search by both PetId and ClientId
                filteredPets = _petsList.Where(p => p.Id.ToString() == PetId && p.ClientId.ToString() == ClientId);
            }
            else if (!string.IsNullOrEmpty(PetId))
            {
                // Search by PetId only
                filteredPets = _petsList.Where(p => p.Id.ToString() == PetId);
            }
            else if (!string.IsNullOrEmpty(ClientId))
            {

                // Search by ClientId only
                filteredPets = _petsList.Where(p => p.ClientId.ToString() == ClientId);
            }
            else
            {
                // No search criteria, return all pets
                filteredPets = _petsList;
            }

            Pets.Clear();
            foreach (var pet in filteredPets)
            {

                Pets.Add(pet);
            }
            NavigationData.ClientId = null;
        }
        public void FilterPetsByClient()
        {
            if ( NavigationData.ClientId>0 )
            {
                Debug.WriteLine($"Filtering pets for ClientId: {NavigationData.ClientId}");

                // Filter the pets based on the selected ClientId
                if (_petsList == null || PetsCollection == null)
                {
                    // Data not loaded yet, show error or handle accordingly
                    MessageBox.Show("Data is still loading. Please try again in a moment.", "Data Not Loaded", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var filteredPets = _petsList.Where(pet => pet.ClientId == NavigationData.ClientId).ToList();

                // Clear the Pets collection and add the filtered pets
                Pets.Clear();
                foreach (var pet in filteredPets)
                {
                    Pets.Add(pet);
                }
            }
            else
            {
                // If no ClientId is selected, show all pets
                Pets.Clear();
                foreach (var pet in _petsList)
                {
                    Pets.Add(pet);
                }
            }
        }

        private async void DeleteClient(Pets pet)
        {
            if (pet != null)
            {
                // Confirm delete operation
                var result = MessageBox.Show($"Are you sure you want to delete pet {pet.Name}?",
                                              "Confirm Delete", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    await _petsRepository.RemoveByIdAsync(pet.Id);
                    PetsCollection.Remove(pet); // Remove from the collection
                    await LoadPets();

                }
            }
            else
            {
                MessageBox.Show("No pet selected!");
            }
        }

        private async void UpdateClient(Pets pet)
        {
            if (pet != null)
            {
                // Example update: Client data already bound to the view
                await _petsRepository.UpdateAsync(pet); // Assuming you have an UpdateAsync method in the repository
                MessageBox.Show($"Pet {pet.Name} updated successfully!");

;
            }
            else
            {
                MessageBox.Show("No pet selected to update!");
            }
        }




    }
}
