// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Lucky_Pet_Clinic2.Services;
using Lucky_Pet_Clinic2.ViewModels.Pages;
using Lucky_Pet_Clinic2.ViewModels.Windows;
using Lucky_Pet_Clinic2.Views.Pages;
using Lucky_Pet_Clinic2.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;

using Wpf.Ui;
using System.Configuration;
using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Repository;
using Lucky_Pet_Clinic2.Models;

namespace Lucky_Pet_Clinic2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            //.ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {

                string connectionString = ConfigurationManager.ConnectionStrings["RemoteCS"]?.ConnectionString;
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
                services.AddHostedService<ApplicationHostService>();
                //services.AddSingleton<SignalRService>(); // Add this line





                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<DataPage>();
                services.AddSingleton<DataViewModel>();

                services.AddSingleton<VisitsPage>();
                services.AddSingleton<VisitsViewModel>();

                services.AddSingleton<AddClientPage>();
                services.AddSingleton<AddClientViewModel>();

                services.AddSingleton<AddPetPage>();
                services.AddSingleton<AddPetViewModel>();

                services.AddSingleton<AddVetPage>();
                services.AddSingleton<AddVetViewModel>();

                services.AddSingleton<ClientsPage>();
                services.AddSingleton<ClientsViewModel>();

                services.AddSingleton<ExaminationPage>();
                services.AddSingleton<ExaminationViewModel>();

                services.AddSingleton<SurgeryPage>();
                services.AddSingleton<SurgeryViewModel>();

                services.AddSingleton<VaccinationPage>();
                services.AddSingleton<VaccinationViewModel>();


                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
                
                services.AddSingleton<PetsPage>();
                services.AddSingleton<PetsViewModel>();


                services.AddScoped<IRepository<Pets>, Repository<Pets>>();


                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {

                await _host.StartAsync();

                // Uncomment the SignalR initialization code if needed
                // var signalRService = _host.Services.GetService<SignalRService>();
                // if (signalRService != null)
                // {
                //     try
                //     {
                //         await signalRService.StartConnectionAsync();
                //         Console.WriteLine("SignalR connection started successfully.");
                //     }
                //     catch (Exception ex)
                //     {
                //         Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
                //         MessageBox.Show($"Error starting SignalR connection: {ex.Message}", "SignalR Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //     }
                // }
            }
            catch (Exception ex)
            {
                // Display error message in case of any startup failures
                MessageBox.Show($"An error occurred during application startup: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //await _host.StartAsync();

        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        //private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        //{
        //    // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        //}

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An unhandled exception occurred: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine($"An unhandled exception occurred: {e.Exception.Message}");
            e.Handled = true; // Prevents the application from terminating
        }

    }
}
