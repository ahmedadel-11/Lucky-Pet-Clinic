using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Gallery.ViewModels.Pages.OpSystem;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class ExaminationViewModel : ObservableObject
    {
        private readonly IRepository<Examination> _examinationRepository;
        private readonly IRepository<Vets> _vetsRepository;
        private ObservableCollection<Vets> _vetsList;
        private readonly INavigationService _navigationService;

        private int? _petid;
        private string _temperature;
        private string _caseHistory;
        private string _vitalSigns;
        private string _diagnosis;
        private string _treatment;
        private string _weight;
        private DateTime? _nextVisit;
        private string _nextVisitType;
        private Vets _selectedVet;
        private DateTime? _examinationDate;

        public ICommand RefreshCommand { get; }
        public ICommand UpdateExaminationCommand { get; }

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

        private string _selectedNextVisitType;
        public string SelectedNextVisitType
        {
            get => _selectedNextVisitType;
            set => SetProperty(ref _selectedNextVisitType, value);
        }

        public ExaminationViewModel(IRepository<Examination> examinationRepository, IRepository<Vets> vetsRepository , INavigationService navigationService)
        {
            _examinationRepository = examinationRepository;
            _vetsRepository = vetsRepository;
            _navigationService = navigationService;
            UpdateExaminationCommand = new RelayCommand<Examination>(UpdateExamination);

            //FilePickerViewModel = new FilePickerViewModel();
            SaveCommand = new RelayCommand(async () => await SaveExamination(), CanSaveExamination);
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
        public string Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public string CaseHistory
        {
            get => _caseHistory;
            set
            {
                _caseHistory = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public string VitalSigns
        {
            get => _vitalSigns;
            set
            {
                _vitalSigns = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public string Diagnosis
        {
            get => _diagnosis;
            set
            {
                _diagnosis = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }
        public string Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }



        public string Treatment
        {
            get => _treatment;
            set
            {
                _treatment = value;
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

        public DateTime? ExaminationDate
        {
            get => _examinationDate;
            set
            {
                _examinationDate = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand { get; }

        private async Task SaveExamination()
        {
            if (MessageBox.Show("Do you want to save this examination?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var examination = new Examination
                    {
                        PetId = (int)PetID,
                        Temperature = Temperature,
                        Weight= Weight,
                        CaseHistory = CaseHistory,
                        VitalSigns = VitalSigns,
                        Diagnosis = Diagnosis,
                        Treatment = Treatment,
                        //XraysAndTests = string.Join(", ", FilePickerViewModel.SelectedFiles), // If you're saving file names/paths
                        XrayTests ="asdasd" , // If you're saving file names/paths
                        NextVisit = NextVisit.GetValueOrDefault(),
                        Vet = SelectedVet.Name,
                        Date = DateTime.Now,
                        NextVisitType = SelectedNextVisitType,
                    };

                    await _examinationRepository.AddAsync(examination);
                    MessageBox.Show("Examination saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private bool CanSaveExamination(object parameter)
        {
            // Enable save only when required fields are filled
            return !string.IsNullOrWhiteSpace(Temperature) && SelectedVet != null && NextVisit.HasValue && Weight!=null;
        }

        private void ClearForm()
        {
            PetID = null;
            Temperature = string.Empty;
            Weight = string.Empty;
            CaseHistory = string.Empty;
            VitalSigns = string.Empty;
            Diagnosis = string.Empty;
            Treatment = string.Empty;
            NextVisit = null;
            SelectedVet = null;
            SelectedNextVisitType = null;   
            //FilePickerViewModel.SelectedFiles.Clear();
        }

        private async Task LoadVets()
        {
            var vets = await _vetsRepository.GetAllAsync();
            VetsList = new ObservableCollection<Vets>(vets);
        }

        private async void UpdateExamination(Examination examination)
        {
            if (examination != null)
            {
                // Example update: Client data already bound to the view
                await _examinationRepository.UpdateAsync(examination); // Assuming you have an UpdateAsync method in the repository
                MessageBox.Show($"Examination {examination.Id} updated successfully!");

                
            }
            else
            {
                MessageBox.Show("No pet selected to update!");
            }
        }

    }
}
