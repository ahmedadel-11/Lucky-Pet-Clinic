// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Lucky_Pet_Clinic2.Models;
using Lucky_Pet_Clinic2.Repository;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Lucky_Pet_Clinic2.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _counter = 0;

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
        }

        private readonly IRepository<Clients> _clientRepository;

        public ObservableCollection<Clients> ClientsCollection { get; } = new ObservableCollection<Clients>();

        public ICommand LoadClientsCommand { get; }
        public ObservableCollection<Contact> ContactsCollection { get; set; }

        public DashboardViewModel(IRepository<Clients> clientRepository)
        {
            ContactsCollection = new ObservableCollection<Contact>
        {
            new Contact { Name = "Ahmed", MobileNumber = "201275775845" },
            new Contact { Name = "Ahmedadel", MobileNumber = "201012722350" },
            new Contact { Name = "Taher", MobileNumber = "966581432962" }
        };

            _clientRepository = clientRepository;
            LoadClientsCommand = new RelayCommand(async () => await LoadClientsAsync());
        }

        public ICommand OpenWhatsAppCommand => new RelayCommand<string>(OpenWhatsApp);

        private void OpenWhatsApp(string mobileNumber)
        {
            string url = $"https://wa.me/+966{mobileNumber}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private async Task LoadClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            ClientsCollection.Clear();
            foreach (var client in clients)
            {
                ClientsCollection.Add(client);
            }
        }
    }
}
