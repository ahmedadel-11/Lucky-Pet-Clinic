using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using Lucky_Pet_Clinic2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui;
using static System.Collections.Specialized.NameObjectCollectionBase;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public class AddVetViewModel : ObservableObject , INotifyPropertyChanged
    {
        private readonly IRepository<Vets> _vetsRepository;
        private string _vetName;
        private ObservableCollection<Vets> _vetsCollection;
        public ICommand RefreshCommand { get; }

        public ICommand SaveCommand { get; }
        public ICommand DeleteVetCommand { get; }

        public AddVetViewModel(IRepository<Vets> vetRepository)
        {


            _vetsRepository = vetRepository;
            SaveCommand = new RelayCommand(async () => await SaveClient(), CanSaveVet);
            RefreshCommand = new RelayCommand(async () => await LoadVets());
            DeleteVetCommand = new RelayCommand<Vets>(DeleteVet);

            LoadVets();
        }




        public string VetName
        {
            get => _vetName;
            set
            {
                _vetName = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();

            }
        }
        public ObservableCollection<Vets> VetsCollection
        {
            get => _vetsCollection;
            set => SetProperty(ref _vetsCollection, value);
        }

        private async Task SaveClient()
        {
            if (MessageBox.Show("Do you want to save this Vet?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    var vet = new Vets
                    {
                        Name = VetName
                    };
                    await _vetsRepository.AddAsync(vet); // Save asynchronously
                    VetsCollection.Add(vet);
                    MessageBox.Show($"Client data saved successfully with ID: {vet.Id}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnPropertyChanged(nameof(VetsCollection));

                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }
        private void ClearForm()
        {
            VetName = string.Empty;
            OnPropertyChanged(nameof(VetName)); // Notify the UI explicitly

        }

        private bool CanSaveVet(object parameter)
        {
            return !string.IsNullOrWhiteSpace(VetName);
        }

        public async Task LoadVets()
        {

            var vets = await _vetsRepository.GetAllAsync();
            VetsCollection = new ObservableCollection<Vets>(vets);
            OnPropertyChanged(nameof(VetsCollection));

        }

        private async void DeleteVet(Vets vet)
        {
            if (vet != null)
            {
                await _vetsRepository.RemoveAsync(vet);
                VetsCollection.Remove(vet);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
