using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using System;
using System.Windows.Input;
using System.Windows;
using Lucky_Pet_Clinic2.Helpers;
using System.Collections.ObjectModel;
using Wpf.Ui;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class VaccinationViewModel : ObservableObject
    {
        private readonly IRepository<Vaccination> _vaccinationRepository;
        private readonly IRepository<Vets> _vetsRepository;
        private ObservableCollection<Vets> _vetsList;
        private readonly INavigationService _navigationService;

        public VaccinationViewModel(IRepository<Vaccination> vaccinationRepository, IRepository<Vets> vetsRepository, INavigationService navigationService)
        {
            _vaccinationRepository = vaccinationRepository;
            _vetsRepository = vetsRepository;
            _navigationService = navigationService;

            SaveCommand = new RelayCommand(async () => await SaveVaccination(), CanSaveVaccination);
            RefreshCommand = new RelayCommand(async () => await LoadVets());

            LoadVets();

            NextVisitTypeCollection = new ObservableCollection<string>
            {
                "Examination",
                "Surgery",
                "Vaccination",
            };

        }
        public ICommand RefreshCommand { get; }

        private int? _petid;
        private string _vaccinationType;
        private DateTime? _vaccinationDate;
        private DateTime? _nextVisitDate;
        private Vets _selectedVet;


        public ObservableCollection<Vets> VetsList
        {
            get => _vetsList;
            set
            {
                _vetsList = value;
                OnPropertyChanged();
            }
        }
        public Vets SelectedVet
        {
            get => _selectedVet;
            set
            {
                _selectedVet = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public int? PetID
        {
            get => _petid;
            set
            {
                _petid = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public string VaccinationType
        {
            get => _vaccinationType;
            set
            {
                _vaccinationType = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public DateTime? VaccinationDate
        {
            get => _vaccinationDate;
            set
            {
                _vaccinationDate = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public DateTime? NextVisitDate
        {
            get => _nextVisitDate;
            set
            {
                _nextVisitDate = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<string> _nextVisitTypeCollection;
        public ObservableCollection<string> NextVisitTypeCollection
        {
            get => _nextVisitTypeCollection;
            set => SetProperty(ref _nextVisitTypeCollection, value);
        }

        private string _selectedNextVisitType;
        public string SelectedNextVisitType
        {
            get => _selectedNextVisitType;
            set => SetProperty(ref _selectedNextVisitType, value);
        }
        public ICommand SaveCommand { get; }

        private async Task SaveVaccination()
        {
            if (MessageBox.Show("Do you want to save this vaccination?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var vaccination = new Vaccination
                    {
                        PetId = PetID.GetValueOrDefault(),
                        VaccinationType = VaccinationType,
                        Vet = SelectedVet.Name,
                        Date = DateTime.Now,
                        NextVisit = NextVisitDate.GetValueOrDefault(),
                        NextVisitType = SelectedNextVisitType,
                    };

                    await _vaccinationRepository.AddAsync(vaccination);
                    MessageBox.Show("Vaccination saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationData.PetId = null;

                    ClearForm();
                    _navigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanSaveVaccination(object parameter)
        {
            // Enable save only when required fields are filled
            return PetID != null && !string.IsNullOrWhiteSpace(VaccinationType)  && NextVisitDate != default && SelectedVet != null;
        }

        private void ClearForm()
        {
            PetID = null;
            VaccinationType = string.Empty;
            SelectedVet = null;

            NextVisitDate = null;
            SelectedNextVisitType = null;
        }

        private async Task LoadVets()
        {
            var vets = await _vetsRepository.GetAllAsync();
            VetsList = new ObservableCollection<Vets>(vets);
        }
    }
}
