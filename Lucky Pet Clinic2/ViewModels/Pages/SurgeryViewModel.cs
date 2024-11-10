using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class SurgeryViewModel : ObservableObject
    {
        private readonly IRepository<Surgery> _surgeryRepository;
        private readonly IRepository<Vets> _vetsRepository;
        private ObservableCollection<Vets> _vetsList;
        private readonly INavigationService _navigationService;

        private int? _petid;
        private string _surgeryType;
        private DateTime? _date;
        private DateTime? _nextVisit;
        private Vets _selectedVet;
        private string _nextVisitType;

        private Surgery _selectedSurgery;
        public Surgery SelectedSurgery
        {
            get { return _selectedSurgery; }
            set
            {
                _selectedSurgery = value;
                OnPropertyChanged(nameof(SelectedSurgery));
            }
        }
        public ICommand RefreshCommand { get; }
        public ICommand UpdateSurgeryCommand { get; }

        public ObservableCollection<Vets> VetsList
        {
            get => _vetsList;
            set
            {
                _vetsList = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<string> _nextVisitTypeCollection;
        public ObservableCollection<string> NextVisitTypeCollection
        {
            get => _nextVisitTypeCollection;
            set => SetProperty(ref _nextVisitTypeCollection, value);
        }

        public SurgeryViewModel(IRepository<Surgery> surgeryRepository, IRepository<Vets> vetsRepository , INavigationService navigationService)
        {
            _surgeryRepository = surgeryRepository;
            _vetsRepository = vetsRepository;
            _navigationService = navigationService;
            UpdateSurgeryCommand = new RelayCommand<Surgery>(UpdateSurgery);

            SaveCommand = new RelayCommand(async () => await SaveSurgery(), CanSaveSurgery);
            RefreshCommand = new RelayCommand(async () => await LoadVets());
            LoadVets();

            NextVisitTypeCollection = new ObservableCollection<string>
            {
                "Examination",
                "Surgery",
                "Vaccination",
            };

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

        public string SurgeryType
        {
            get => _surgeryType;
            set
            {
                _surgeryType = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public DateTime? NextVisit
        {
            get => _nextVisit;
            set
            {
                _nextVisit = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        private string _selectedNextVisitType;
        public string SelectedNextVisitType
        {
            get => _selectedNextVisitType;
            set => SetProperty(ref _selectedNextVisitType, value);
        }
        public ICommand SaveCommand { get; }

        private async Task SaveSurgery()
        {
            if (MessageBox.Show("Do you want to save this surgery?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var surgery = new Surgery
                    {
                        PetId = PetID.Value,
                        SurgeryType = SurgeryType,
                        Vet = SelectedVet.Name,
                        Date = DateTime.Now,
                        NextVisit = NextVisit.GetValueOrDefault(),
                        NextVisitType = SelectedNextVisitType
                    };

                    await _surgeryRepository.AddAsync(surgery);
                    MessageBox.Show("Surgery saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationData.PetId = null;
                    ClearForm();
                    _navigationService.GoBack();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving the surgery: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanSaveSurgery(object parameter)
        {
            return PetID.HasValue && !string.IsNullOrWhiteSpace(SurgeryType)  && NextVisit.HasValue && SelectedVet != null;
        }

        private void ClearForm()
        {
            PetID = null;
            SurgeryType = string.Empty;
            SelectedVet = null;
            SelectedNextVisitType = null;

            NextVisit = null;
        }

        private async Task LoadVets()
        {
            var vets = await _vetsRepository.GetAllAsync();
            VetsList = new ObservableCollection<Vets>(vets);
        }

        private async void UpdateSurgery(Surgery surgery)
        {
            if (surgery != null)
            {
                // Example update: Client data already bound to the view
                await _surgeryRepository.UpdateAsync(surgery); // Assuming you have an UpdateAsync method in the repository
                MessageBox.Show($"Surgery {surgery.SurgeryID} updated successfully!");


            }
            else
            {
                MessageBox.Show("No pet selected to update!");
            }
        }

    }
}
