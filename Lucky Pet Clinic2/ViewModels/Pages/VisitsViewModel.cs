using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class VisitsViewModel : ObservableObject
    {
        private readonly IRepository<Examination> _examinationRepository;
        private readonly IRepository<Surgery> _surgeryRepository;
        private readonly IRepository<Vaccination> _vaccinationRepository;

        // Cache all data after loading from the database
        private List<Examination> _allExaminations;
        private List<Surgery> _allSurgeries;
        private List<Vaccination> _allVaccinations;


        public ObservableCollection<Examination> FilteredExaminations { get; set; }
        public ObservableCollection<Surgery> FilteredSurgeries { get; set; }
        public ObservableCollection<Vaccination> FilteredVaccinations { get; set; }


        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    FilterDataByDate(); // Filter data when the date changes
                }
            }
        }
        public ICommand RefreshCommand { get; }

        public VisitsViewModel(IRepository<Examination> examinationRepo, IRepository<Surgery> surgeryRepo, IRepository<Vaccination> vaccinationRepo)
        {
            _examinationRepository = examinationRepo;
            _surgeryRepository = surgeryRepo;
            _vaccinationRepository = vaccinationRepo;

            SelectedDate = DateTime.Today; // Initialize the SelectedDate
            RefreshCommand = new RelayCommand(async () => await LoadAllData());

            // Load all data from the database once
            LoadAllData();
        }

        // Load all data once and cache it in memory
        //private async Task LoadAllData()
        //{
        //    _allExaminations = (await _examinationRepository.GetAllExaminationAsync()).ToList();
        //    _allSurgeries = (await _surgeryRepository.GetAllSurgeryAsync()).ToList();
        //    _allVaccinations = (await _vaccinationRepository.GetAllVaccinationAsync()).ToList();



        //    // Filter data by the initially selected date
        //    FilterDataByDate();
        //}

        public async Task LoadAllData()
        {
            // Load all data once and keep in memory
            var examinations = await _examinationRepository.GetAllExaminationAsync();
            var surgeries = await _surgeryRepository.GetAllSurgeryAsync();
            var vaccinations = await _vaccinationRepository.GetAllVaccinationAsync();

            // Combine and filter data by NextVisitType for each category
            _allExaminations = examinations
                .Where(e => e.NextVisitType == "Examination")
                .Concat(surgeries
                    .Where(s => s.NextVisitType == "Examination")
                    .Select(s => new Examination
                    {
                        PetId = s.PetId,
                        Date = s.Date,
                        Vet = s.Vet,
                        NextVisitType = s.NextVisitType,
                        Pet = s.Pet,
                        NextVisit = s.NextVisit,

                        // Include any other necessary fields
                    }))
                .Concat(vaccinations
                    .Where(v => v.NextVisitType == "Examination")
                    .Select(v => new Examination
                    {
                        PetId = v.PetId,
                        Date = v.Date,
                        Vet = v.Vet,
                        NextVisitType = v.NextVisitType,
                        Pet = v.Pet,
                        NextVisit = v.NextVisit,
                        // Include any other necessary fields
                    })).ToList();

            _allSurgeries = surgeries
                .Where(s => s.NextVisitType == "Surgery")
                .Concat(examinations
                    .Where(e => e.NextVisitType == "Surgery")
                    .Select(e => new Surgery
                    {
                        PetId = e.PetId,
                        Date = e.Date,
                        Vet = e.Vet,
                        NextVisitType = e.NextVisitType,
                        Pet = e.Pet,
                        NextVisit = e.NextVisit,
                        // Include any other necessary fields
                    }))
                .Concat(vaccinations
                    .Where(v => v.NextVisitType == "Surgery")
                    .Select(v => new Surgery
                    {
                        PetId = v.PetId,
                        Date = v.Date,
                        Vet = v.Vet,
                        NextVisitType = v.NextVisitType,
                        Pet = v.Pet,
                        NextVisit = v.NextVisit,
                        // Include any other necessary fields
                    })).ToList();

            _allVaccinations = vaccinations
                .Where(v => v.NextVisitType == "Vaccination")
                .Concat(examinations
                    .Where(e => e.NextVisitType == "Vaccination")
                    .Select(e => new Vaccination
                    {
                        PetId = e.PetId,
                        Date = e.Date,
                        Vet = e.Vet,
                        NextVisitType = e.NextVisitType,
                            Pet = e.Pet,
                        NextVisit = e.NextVisit,
                        // Include any other necessary fields
                    }))
                .Concat(surgeries
                    .Where(s => s.NextVisitType == "Vaccination")
                    .Select(s => new Vaccination
                    {
                        PetId = s.PetId,
                        Date = s.Date,
                        Vet = s.Vet,
                        NextVisitType = s.NextVisitType,
                        Pet = s.Pet,
                        NextVisit = s.NextVisit,
                        // Include any other necessary fields
                    })).ToList();


            FilterDataByDate();

        }

        // Filter cached data based on the selected date
        private void FilterDataByDate()
        {
            if (_allExaminations != null && _allSurgeries != null && _allVaccinations != null)
            {
                FilteredExaminations = new ObservableCollection<Examination>(_allExaminations.Where(e => e.NextVisit.Date == SelectedDate.Date));
                FilteredSurgeries = new ObservableCollection<Surgery>(_allSurgeries.Where(s => s.NextVisit.Date == SelectedDate.Date));
                FilteredVaccinations = new ObservableCollection<Vaccination>(_allVaccinations.Where(v => v.NextVisit.Date == SelectedDate.Date));

                // Notify UI about the changes
                OnPropertyChanged(nameof(FilteredExaminations));
                OnPropertyChanged(nameof(FilteredSurgeries));
                OnPropertyChanged(nameof(FilteredVaccinations));
            }
        }

        // Open WhatsApp with the client's number
        public ICommand OpenWhatsAppCommand => new RelayCommand<string>(OpenWhatsApp);

        private void OpenWhatsApp(string mobileNumber)
        {
            string url = $"https://wa.me/+966{mobileNumber}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
