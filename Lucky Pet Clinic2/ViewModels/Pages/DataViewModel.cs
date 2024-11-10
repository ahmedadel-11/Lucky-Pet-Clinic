using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using Lucky_Pet_Clinic2.Views.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Collections.Specialized.NameObjectCollectionBase;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class DataViewModel : ObservableObject
    {
        private readonly IRepository<Examination> _examinationRepository;
        private readonly IRepository<Surgery> _surgeryRepository;
        private readonly IRepository<Vaccination> _vaccinationRepository;

        public ObservableCollection<Examination> AllExaminations { get; private set; }
        public ObservableCollection<Surgery> AllSurgeries { get; private set; }
        public ObservableCollection<Vaccination> AllVaccinations { get; private set; }

        public ObservableCollection<Examination> FilteredExaminations { get; set; }
        public ObservableCollection<Surgery> FilteredSurgeries { get; set; }
        public ObservableCollection<Vaccination> FilteredVaccinations { get; set; }

        private int? _petid;
        public int? PetId
        {
            get => _petid;
            set
            {
                _petid = value;
                OnPropertyChanged();
            }
        }
        private string _clientName;
        public string ClientName
        {
            get => _clientName;
            set => SetProperty(ref _clientName, value);
        }
        public ICommand SearchPetsCommand { get; }
        public ICommand OpenExaminationWindowCommand { get; }
        public ICommand OpenSurgeryWindowCommand { get; }
        public ICommand OpenVaccinationWindowCommand { get; }
        public ICommand RefreshCommand { get; }

        public ICommand DeleteExaminationCommand { get; }
        public ICommand DeleteSurgeryCommand { get; }
        public ICommand DeleteVaccinationCommand { get; }

        public ICommand UpdateExaminationCommand { get; }

        public ICommand UpdateSurgeryCommand { get; }
        public ICommand UpdateVaccinationCommand { get; }

        private ObservableCollection<string> _nextVisitTypeCollection;
        public ObservableCollection<string> NextVisitTypeCollection
        {
            get => _nextVisitTypeCollection;
            set => SetProperty(ref _nextVisitTypeCollection, value);
        }
        public DataViewModel(IRepository<Examination> examinationRepo, IRepository<Surgery> surgeryRepo, IRepository<Vaccination> vaccinationRepo)
        {

            _examinationRepository = examinationRepo;
            _surgeryRepository = surgeryRepo;
            _vaccinationRepository = vaccinationRepo;
             LoadData();

            OpenExaminationWindowCommand = new RelayCommand<int>(OpenExaminationWindow);
            OpenSurgeryWindowCommand = new RelayCommand<int>(OpenSurgeryWindow);
            OpenVaccinationWindowCommand = new RelayCommand<int>(OpenVaccinationWindow);
            RefreshCommand = new RelayCommand(async () => await LoadData());
            UpdateSurgeryCommand = new RelayCommand<Surgery>(UpdateSurgery);
            UpdateExaminationCommand = new RelayCommand<Examination>(UpdateExamination);
            UpdateVaccinationCommand = new RelayCommand<Vaccination>(UpdateVaccination);
            // Initialize command for searching
            SearchPetsCommand = new RelayCommand(SearchPets);

            DeleteExaminationCommand = new RelayCommand<Examination>(DeleteExamination);
            DeleteSurgeryCommand = new RelayCommand<Surgery>(DeleteSurgery);
            DeleteVaccinationCommand = new RelayCommand<Vaccination>(DeleteVaccination);
            NextVisitTypeCollection = new ObservableCollection<string>
            {
                "Examination",
                "Surgery",
                "Vaccination",
            };

            // Load the data once
        }
        private Examination _selectedExaminationy;
        public Examination SelectedExamination
        {
            get => _selectedExaminationy;
            set
            {
                _selectedExaminationy = value;
                OnPropertyChanged();
            }
        }

        private Surgery _selectedSurgery;
        public Surgery SelectedSurgery
        {
            get => _selectedSurgery;
            set
            {
                _selectedSurgery = value;
                OnPropertyChanged();
            }
        }

        private Vaccination _selectedVaccination;
        public Vaccination SelectedVaccination
        {
            get => _selectedVaccination;
            set
            {
                _selectedVaccination = value;
                OnPropertyChanged();
            }
        }
        private async void OpenExaminationWindow(int examinationId)
        {
            var examination = await _examinationRepository.GetByIdAsync(examinationId); // Await the task to get the actual Examination object
            if (examination != null)
            {
                SelectedExamination = examination; // Set the selected examination object as a property in the DataViewModel
                var window = new ExaminationWindow
                {
                    DataContext = this // Set the DataContext to the Examination object
                };
                window.Show();
            }
            
        }
        private async void OpenSurgeryWindow(int surgeryId)
        {
            var surgery = await _surgeryRepository.GetByIdAsync(surgeryId); // Await the task to get the actual Surgery object
            if (surgery != null)
            {
                SelectedSurgery = surgery; // Set the selected surgery object as a property in the DataViewModel
                var window = new SurgeryWindow
                {
                    DataContext = this // Set the DataContext to DataViewModel
                };

                // Set the selected surgery object as a property in the DataViewModel

                window.Show();
            }
        }


        private async void OpenVaccinationWindow(int VaccinationId)
        {
            var vacctination = await _vaccinationRepository.GetByIdAsync(VaccinationId); // Await the task to get the actual Examination object
            if (vacctination != null)
            {
                SelectedVaccination = vacctination; // Set the selected examination object as a property in the DataViewModel
                var window = new VaccinationWindow
                {
                    DataContext = this
                };
                window.Show();
            }

        }

        public async Task LoadData()
        {
            // Load all data once and keep in memory
            var allExaminations = await _examinationRepository.GetAllExaminationAsync();
            var allSurgeries = await _surgeryRepository.GetAllSurgeryAsync();
            var allVaccinations = await _vaccinationRepository.GetAllVaccinationAsync();


            AllExaminations = new ObservableCollection<Examination>(allExaminations);
            AllSurgeries = new ObservableCollection<Surgery>(allSurgeries);
            AllVaccinations = new ObservableCollection<Vaccination>(allVaccinations);

            // Initial filtering to show all data
            FilteredExaminations = new ObservableCollection<Examination>(AllExaminations);
            FilteredSurgeries = new ObservableCollection<Surgery>(AllSurgeries);
            FilteredVaccinations = new ObservableCollection<Vaccination>(AllVaccinations);
        }

        //public async Task LoadData()
        //{
        //    // Load all data once and keep in memory
        //    var examinations = await _examinationRepository.GetAllExaminationAsync();
        //    var surgeries = await _surgeryRepository.GetAllSurgeryAsync();
        //    var vaccinations = await _vaccinationRepository.GetAllVaccinationAsync();

        //    // Combine and filter data by NextVisitType for each category
        //    var allExaminations = examinations
        //        .Where(e => e.NextVisitType == "Examination")
        //        .Concat(surgeries
        //            .Where(s => s.NextVisitType == "Examination")
        //            .Select(s => new Examination
        //            {
        //                PetId = s.PetId,
        //                Date = s.Date,
        //                Vet = s.Vet,
        //                NextVisitType = s.NextVisitType,
        //                // Include any other necessary fields
        //            }))
        //        .Concat(vaccinations
        //            .Where(v => v.NextVisitType == "Examination")
        //            .Select(v => new Examination
        //            {
        //                PetId = v.PetId,
        //                Date = v.Date,
        //                Vet = v.Vet,
        //                NextVisitType = v.NextVisitType,
        //                // Include any other necessary fields
        //            }));

        //    var allSurgeries = surgeries
        //        .Where(s => s.NextVisitType == "Surgery")
        //        .Concat(examinations
        //            .Where(e => e.NextVisitType == "Surgery")
        //            .Select(e => new Surgery
        //            {
        //                PetId = e.PetId,
        //                Date = e.Date,
        //                Vet = e.Vet,
        //                NextVisitType = e.NextVisitType,
        //                // Include any other necessary fields
        //            }))
        //        .Concat(vaccinations
        //            .Where(v => v.NextVisitType == "Surgery")
        //            .Select(v => new Surgery
        //            {
        //                PetId = v.PetId,
        //                Date = v.Date,
        //                Vet = v.Vet,
        //                NextVisitType = v.NextVisitType,
        //                // Include any other necessary fields
        //            }));

        //    var allVaccinations = vaccinations
        //        .Where(v => v.NextVisitType == "Vaccination")
        //        .Concat(examinations
        //            .Where(e => e.NextVisitType == "Vaccination")
        //            .Select(e => new Vaccination
        //            {
        //                PetId = e.PetId,
        //                Date = e.Date,
        //                Vet = e.Vet,
        //                NextVisitType = e.NextVisitType,
        //                // Include any other necessary fields
        //            }))
        //        .Concat(surgeries
        //            .Where(s => s.NextVisitType == "Vaccination")
        //            .Select(s => new Vaccination
        //            {
        //                PetId = s.PetId,
        //                Date = s.Date,
        //                Vet = s.Vet,
        //                NextVisitType = s.NextVisitType,
        //                // Include any other necessary fields
        //            }));

        //    // Populate observable collections
        //    AllExaminations = new ObservableCollection<Examination>(allExaminations);
        //    AllSurgeries = new ObservableCollection<Surgery>(allSurgeries);
        //    AllVaccinations = new ObservableCollection<Vaccination>(allVaccinations);

        //    // Initial filtering to show all data
        //    FilteredExaminations = new ObservableCollection<Examination>(AllExaminations);
        //    FilteredSurgeries = new ObservableCollection<Surgery>(AllSurgeries);
        //    FilteredVaccinations = new ObservableCollection<Vaccination>(AllVaccinations);
        //}


        // Filter the data based on the entered PetId
        public void SearchPets()
        {
            if (AllExaminations == null || AllSurgeries == null || AllVaccinations == null)
            {
                // Data not loaded yet, show error or handle accordingly
                MessageBox.Show("Data is still loading. Please try again in a moment.", "Data Not Loaded", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (PetId.HasValue)
            {
                FilteredExaminations = new ObservableCollection<Examination>(AllExaminations.Where(e => e.PetId == PetId).OrderByDescending(e => e.Date));
                FilteredSurgeries = new ObservableCollection<Surgery>(AllSurgeries.Where(s => s.PetId == PetId).OrderByDescending(e => e.Date));
                FilteredVaccinations = new ObservableCollection<Vaccination>(AllVaccinations.Where(v => v.PetId == PetId).OrderByDescending(e => e.Date));
            }
            else
            {
                // Reset to show all data if no PetId is entered
                FilteredExaminations = new ObservableCollection<Examination>(AllExaminations);
                FilteredSurgeries = new ObservableCollection<Surgery>(AllSurgeries);
                FilteredVaccinations = new ObservableCollection<Vaccination>(AllVaccinations);
            }

            // Notify UI of changes
            OnPropertyChanged(nameof(FilteredExaminations));
            OnPropertyChanged(nameof(FilteredSurgeries));
            OnPropertyChanged(nameof(FilteredVaccinations));
            NavigationData.PetId = null;
        }

        private async void DeleteExamination(Examination examination)
        {
            if (examination != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete this examination ?",
              "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _examinationRepository.RemoveAsync(examination);
                        AllExaminations.Remove(examination);
                        FilteredExaminations.Remove(examination);
                        FilteredExaminations = new ObservableCollection<Examination>(FilteredExaminations);
                        OnPropertyChanged(nameof(FilteredSurgeries));
                        MessageBox.Show($"Done.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting the examination: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
        }
        private async void DeleteSurgery(Surgery surgery)
        {
            if (surgery != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete this surgery ?",
                              "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _surgeryRepository.RemoveAsync(surgery);
                        AllSurgeries.Remove(surgery);
                        FilteredSurgeries.Remove(surgery);
                        FilteredSurgeries = new ObservableCollection<Surgery>(FilteredSurgeries);
                        OnPropertyChanged(nameof(FilteredSurgeries));
                        MessageBox.Show($"Done.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting the surgery: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void DeleteVaccination(Vaccination vaccination)
        {
            if (vaccination != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete this vaccination ?",
              "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _vaccinationRepository.RemoveAsync(vaccination);
                        AllVaccinations.Remove(vaccination);
                        FilteredVaccinations.Remove(vaccination);
                        FilteredVaccinations = new ObservableCollection<Vaccination>(FilteredVaccinations);
                        OnPropertyChanged(nameof(FilteredSurgeries));
                        MessageBox.Show($"Done.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting the vaccination: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
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

        private async void UpdateVaccination(Vaccination vaccination)
        {
            if (vaccination != null)
            {
                // Example update: Client data already bound to the view
                await _vaccinationRepository.UpdateAsync(vaccination); // Assuming you have an UpdateAsync method in the repository
                MessageBox.Show($"Vaccination {vaccination.VaccinationID} updated successfully!");
            }
            else
            {
                MessageBox.Show("No pet selected to update!");
            }
        }


    }
}
