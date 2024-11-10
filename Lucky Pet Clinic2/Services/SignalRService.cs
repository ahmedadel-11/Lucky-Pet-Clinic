//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.SignalR.Client;

//namespace Lucky_Pet_Clinic2.Services
//{
//    public class SignalRService
//    {
//        private readonly HubConnection _hubConnection;
//        public event Action OnUpdateReceived;

//        public SignalRService()
//        {
//            _hubConnection = new HubConnectionBuilder()
//                .WithUrl("http://localhost:5252/clientHub") // Ensure this URL is correct
//                .Build();
//            _hubConnection.On("ReceiveUpdate", () =>
//            {
//                // Raise event to notify subscribers
//                OnUpdateReceived?.Invoke();
//            });
//        }

//        public async Task StartConnectionAsync()
//        {
//            try
//            {
//                await _hubConnection.StartAsync();
//                Console.WriteLine("Connected to SignalR server.");
//                MessageBox.Show($"Connected to SignalR server.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"An error occurred while connecting the server: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        public async Task SendUpdateAsync()
//        {
//            try
//            {
//                await _hubConnection.InvokeAsync("SendUpdate");
//                Console.WriteLine("Update sent to SignalR server.");
//                MessageBox.Show($"Update sent to SignalR server.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error sending update to SignalR server: {ex.Message}");
//                MessageBox.Show($"An error occurred while sending update to SignalR server:: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

//            }
//        }
//    }
//}