﻿using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Plantjes.ViewModels;
using Plantjes.ViewModels.Interfaces;
using Plantjes.ViewModels.Services;
// using ServiceProvider = Plantjes.ViewModels.HelpClasses.ServiceProvider;

namespace Plantjes;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; }

    public App() {
        Services = ConfigureServices();
        this.InitializeComponent();
        //SearchService.CreateInstance();

        // VMprovider toevoegen als "static resource" in MvvM zodat die kan worden gebruikt in de Views om
        // de ViewModels te koppelen aan de DataContext
        // instantie die over de hele applicatie kan worden gebruikt in de Views met onderstaande binding
        // <Window: ...
        // DataContext="{Binding Source={ StaticResource VMProvider }, Path=MainWindowViewModel }" 
        // ... 
        // >

        //var iocc = SimpleIoc.Default;

        //ViewModelRepo.CreateInstance();


        // Resources.Add("VMProvider", new ViewModelProvider());

        // de viewmodellen kunnen ook worden toegekend aan de 
        // datacontext van de view met GetInstance methode van de IoC Container
        // in de code behind van de view (yyy.xaml.cs)
        //vb. DataContext = GalaSoft.MvvmLight.Ioc.SimpleIoc.Default.GetInstance<MainWindowViewModel>
    }

    private static IServiceProvider ConfigureServices() {
        var services = new ServiceCollection();
        
        //xander - services
        services.AddSingleton<IloginUserService, LoginUserService>();
        services.AddSingleton<ISearchService, SearchService>();
        services.AddSingleton<IDetailService, DetailService>();

        //xander - viewmodels
        services.AddTransient<ViewModelAppearance>();
        services.AddTransient<ViewModelBloom>();
        services.AddTransient<ViewModelGrooming>();
        services.AddTransient<ViewModelGrow>();
        services.AddTransient<ViewModelHabitat>();
        services.AddTransient<ViewModelLogin>();
        services.AddTransient<ViewModelMain>();
        services.AddTransient<ViewModelNameResult>();
        services.AddTransient<ViewModelRegister>();
        services.AddTransient<ViewModelRepo>();
        
        //xander - viewmodel factories
        var loginService = (LoginUserService)services.BuildServiceProvider().GetService(typeof(LoginUserService));
        var searchService = (SearchService)services.BuildServiceProvider().GetService(typeof(SearchService));
        var detailService = (DetailService)services.BuildServiceProvider().GetService(typeof(DetailService));
        
        services.AddSingleton(() => new ViewModelLogin(loginService));
        services.AddSingleton(() => new ViewModelRegister(loginService));
        services.AddSingleton(() => new ViewModelBloom(detailService));
        services.AddSingleton(() => new ViewModelGrooming(detailService));
        services.AddSingleton(() => new ViewModelGrow(detailService));
        services.AddSingleton(() => new ViewModelHabitat(detailService));
        services.AddSingleton(() => new ViewModelAppearance(detailService));
        services.AddSingleton(() => new ViewModelNameResult(searchService));
        services.AddSingleton(() => new ViewModelBase());
        services.AddSingleton(() => new ViewModelMain(loginService, searchService));
        services.AddSingleton(() => new ViewModelRepo());

        return services.BuildServiceProvider();
    }
}