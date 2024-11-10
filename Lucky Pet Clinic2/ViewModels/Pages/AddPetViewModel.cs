using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using Lucky_Pet_Clinic2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class AddPetViewModel : ObservableObject
    {


        private readonly IRepository<Pets> _petRepository;
        //private readonly SignalRService _signalRService;
        private readonly INavigationService _navigationService;

        public ICommand SaveCommand { get; }

        private ObservableCollection<string> _petTypes;
        public ObservableCollection<string> PetTypes
        {
            get => _petTypes;
            set => SetProperty(ref _petTypes, value);
        }

        private string _selectedPetType;
        public string SelectedPetType
        {
            get => _selectedPetType;
            set => SetProperty(ref _selectedPetType, value);
        }



        private ObservableCollection<string> _genders;
        public ObservableCollection<string> Genders
        {
            get => _genders;
            set => SetProperty(ref _genders, value);
        }

        private string _selectedGender;
        public string SelectedGender
        {
            get => _selectedGender;
            set => SetProperty(ref _selectedGender, value);
        }


        private int? _clientId;
        public int? ClientId
        {
            get => _clientId;
            set => SetProperty(ref _clientId, value);
        }

        private DateTime? _dateOfBirth;
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (SetProperty(ref _dateOfBirth, value))
                {
                    CalculateAge();
                }
            }
        }

        private int _ageYears;
        public int AgeYears
        {
            get => _ageYears;
            set => SetProperty(ref _ageYears, value);
        }

        private int _ageMonths;
        public int AgeMonths
        {
            get => _ageMonths;
            set => SetProperty(ref _ageMonths, value);
        }

        public string _petName;
        public string PetName
        {
            get => _petName;
            set => SetProperty(ref _petName, value);
        }        
        
        public string _species;
        public string Species
        {
            get => _species;
            set => SetProperty(ref _species, value);
        }


        public bool _isSterilized;
        public bool IsSterilized
        {
            get => _isSterilized;
            set => SetProperty(ref _isSterilized, value);
        }
        private int _ageDays;
        public int AgeDays
        {
            get => _ageDays;
            set => SetProperty(ref _ageDays, value);
        }
        public AddPetViewModel(INavigationService navigationService, IRepository<Pets> petRepository)
        {
            //ClientId = NavigationData.ClientId;
            //MessageBox.Show($"AddPetViewModel initialized with ClientId: {ClientId}"); // Debugging
            _petRepository = petRepository;
            //_signalRService = signalRService;
            SaveCommand = new RelayCommand(async () => await SavePet(),CanSavePet);
            _navigationService = navigationService;

            // Populate the PetTypes collection with sample data
            PetTypes = new ObservableCollection<string>
            {
                "Dog",
                "Cat",
                "Bird",
                "Fish",
                "Reptile"
            };

            Genders = new ObservableCollection<string>
            {
                "Male",
                "Neutered Male",
                "Female",
                "Spayed Female"
            };

            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(PetName) ||
                    e.PropertyName == nameof(SelectedPetType) ||
                    e.PropertyName == nameof(SelectedGender) ||
                    e.PropertyName == nameof(ClientId) ||
                    e.PropertyName == nameof(DateOfBirth))
                {
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            };
        }

        private void CalculateAge()
        {
            if (_dateOfBirth.HasValue)
            {
                var today = DateTime.Today;
                var ageYears = today.Year - _dateOfBirth.Value.Year;
                var ageMonths = today.Month - _dateOfBirth.Value.Month;
                var ageDays = today.Day - _dateOfBirth.Value.Day;

                if (ageDays < 0)
                {
                    ageMonths--;
                    // Add days in the previous month to adjust the days
                    ageDays += DateTime.DaysInMonth(today.Year, today.Month - 1);
                }

                if (ageMonths < 0)
                {
                    ageYears--;
                    ageMonths += 12;
                }

                AgeYears = ageYears;
                AgeMonths = ageMonths;
                AgeDays = ageDays;
            }
            else
            {
                AgeYears = 0;
                AgeMonths = 0;
                AgeDays = 0;
            }
        }


        private async Task SavePet()
        {
            if (MessageBox.Show("Do you want to save this pet?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var pet = new Pets
                    {
                        Name = PetName,
                        Type = SelectedPetType,
                        Gender = SelectedGender,
                        DateOfBirth = DateOfBirth.GetValueOrDefault(),
                        Species = Species,
                        ClientId = (int)ClientId
                    };

                    await _petRepository.AddAsync(pet); // Save asynchronously
                    MessageBox.Show($"Pet data saved successfully with ID: {pet.Id}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    //await _signalRService.SendUpdateAsync();

                    ClearForm();
                    NavigationData.ClientId = null;
                    _navigationService.GoBack();

                }
                catch (DbUpdateException dbEx)
                {
                    MessageBox.Show($"Database error: {dbEx.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Log the error (to a file or log management system)
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Log the error
                }

            }
        }
        private bool CanSavePet(object parameter)
        {
            return !string.IsNullOrWhiteSpace(PetName) &&
                   !string.IsNullOrWhiteSpace(SelectedPetType) &&
                   ClientId > 0 &&
                   !string.IsNullOrWhiteSpace(SelectedGender) &&
                   DateOfBirth != null;
        }


        private void ClearForm()
        {
            PetName = string.Empty;
            SelectedPetType = null;
            SelectedGender = null;
            DateOfBirth = null;
            Species = string.Empty;
            AgeYears = 0;
            AgeMonths = 0;
            AgeDays = 0;
            ClientId = null;
            IsSterilized = false;
        }


    }
}
