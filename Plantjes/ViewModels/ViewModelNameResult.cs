﻿using System.Collections.ObjectModel;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.Input;
using Plantjes.Models.Db;
using Plantjes.ViewModels.Interfaces;

namespace Plantjes.ViewModels; 

public class ViewModelNameResult : ViewModelBase {
    //private ServiceProvider _serviceProvider;
    private readonly ISearchService _searchService = (ISearchService)App.Current.Services.GetService(typeof(ISearchService));

    public ViewModelNameResult(ISearchService searchService) {
        _searchService = searchService;
        //_searchService = new SearchService();

        //Observable Collections 
        ////Obserbable collections to fill with the necessary objects to show in the comboboxes
        cmbTypes = new ObservableCollection<TfgsvType>();
        cmbFamilies = new ObservableCollection<TfgsvFamilie>();
        cmbGeslacht = new ObservableCollection<TfgsvGeslacht>();
        cmbSoort = new ObservableCollection<TfgsvSoort>();
        cmbVariant = new ObservableCollection<TfgsvVariant>();
        cmbRatioBladBloei = new ObservableCollection<Fenotype>();

        ////Observable Collections to bind to listboxes
        filteredPlantResults = new ObservableCollection<Plant>();
        detailsSelectedPlant = new ObservableCollection<string>();

        //ICommands
        ////These will be used to bind our buttons in the xaml and to give them functionality
        SearchCommand = new RelayCommand(ApplyFilterClick);
        ResetCommand = new RelayCommand(ResetClick);

        //These comboboxes will already be filled with data on startup
        fillComboboxes();
    }


    //Observable collections
    ////Bind to comboboxes
    public ObservableCollection<TfgsvType> cmbTypes { get; set; }
    public ObservableCollection<TfgsvFamilie> cmbFamilies { get; set; }
    public ObservableCollection<TfgsvGeslacht> cmbGeslacht { get; set; }
    public ObservableCollection<TfgsvSoort> cmbSoort { get; set; }
    public ObservableCollection<TfgsvVariant> cmbVariant { get; set; }
    public ObservableCollection<Fenotype> cmbRatioBladBloei { get; set; }

    ////Bind to ListBoxes
    public ObservableCollection<Plant> filteredPlantResults { get; set; }

    public ObservableCollection<string> detailsSelectedPlant { get; set; }

    //geschreven door owen
    public void FillAllImages() {
        ImageBlad = _searchService.GetImageLocation("blad", SelectedPlantInResult);
        ImageBloei = _searchService.GetImageLocation("bloei", SelectedPlantInResult);
        ImageHabitus = _searchService.GetImageLocation("habitus", SelectedPlantInResult);
    }

    //written by kenny (region)

    #region tussenFunctie voor knoppen met parameters

    public void fillComboboxes() {
        _searchService.fillComboBoxType(cmbTypes);
        _searchService.fillComboBoxFamilie(SelectedType, cmbFamilies);
        _searchService.fillComboBoxGeslacht(SelectedFamilie, cmbGeslacht);
        _searchService.fillComboBoxSoort(SelectedGeslacht, cmbSoort);
        _searchService.fillComboBoxVariant(cmbVariant);
        _searchService.fillComboBoxRatioBloeiBlad(cmbRatioBladBloei);
    }

    public void ResetClick() {
        filteredPlantResults.Clear();

        cmbTypes.Clear();
        cmbFamilies.Clear();
        cmbGeslacht.Clear();
        cmbSoort.Clear();
        cmbVariant.Clear();
        cmbRatioBladBloei.Clear();
        SelectedNederlandseNaam = null;

        fillComboboxes();
    }


    public void ApplyFilterClick() {
        filteredPlantResults.Clear();
        var listPlants = _searchService.ApplyFilter(SelectedType, SelectedFamilie, SelectedGeslacht, SelectedSoort, SelectedVariant, SelectedNederlandseNaam, SelectedRatioBloeiBlad);
        foreach (var item in listPlants) filteredPlantResults.Add(item);
    }

    #endregion

    ////

    #region RelayCommands

    //RelayCommands
    public RelayCommand SearchCommand { get; set; }
    public RelayCommand ResetCommand { get; set; }

    #endregion

    //geschreven door owen en robin

    #region Selected Item variables for each combobox

    private TfgsvType _selectedType;

    public TfgsvType SelectedType {
        get => _selectedType;
        set {
            _selectedType = value;

            cmbFamilies.Clear();
            cmbGeslacht.Clear();
            cmbSoort.Clear();
            cmbVariant.Clear();

            _searchService.fillComboBoxFamilie(SelectedType, cmbFamilies);
            OnPropertyChanged();
        }
    }

    private TfgsvFamilie _selectedFamilie;

    public TfgsvFamilie SelectedFamilie {
        get => _selectedFamilie;
        set {
            _selectedFamilie = value;


            cmbGeslacht.Clear();
            cmbSoort.Clear();
            cmbVariant.Clear();

            _searchService.fillComboBoxGeslacht(SelectedFamilie, cmbGeslacht);
            OnPropertyChanged();
        }
    }

    private TfgsvGeslacht _selectedGeslacht;

    public TfgsvGeslacht SelectedGeslacht {
        get => _selectedGeslacht;
        set {
            _selectedGeslacht = value;


            cmbSoort.Clear();
            cmbVariant.Clear();

            _searchService.fillComboBoxSoort(SelectedGeslacht, cmbSoort);
            OnPropertyChanged();
        }
    }

    private TfgsvSoort _selectedSoort;

    public TfgsvSoort SelectedSoort {
        get => _selectedSoort;
        set {
            _selectedSoort = value;

            cmbVariant.Clear();

            OnPropertyChanged();
        }
    }

    private TfgsvVariant _selectedVariant;

    public TfgsvVariant SelectedVariant {
        get => _selectedVariant;
        set {
            _selectedVariant = value;
            OnPropertyChanged();
        }
    }

    private string _selectedRatioBloeiBlad;

    public string SelectedRatioBloeiBlad {
        get => _selectedRatioBloeiBlad;
        set {
            _selectedRatioBloeiBlad = value;
            OnPropertyChanged();
        }
    }

    private string _selectedNederlandseNaam;

    public string SelectedNederlandseNaam {
        get => _selectedNederlandseNaam;
        set {
            if (SelectedNederlandseNaam == "")
                _selectedNederlandseNaam = null;
            else
                _selectedNederlandseNaam = value;

            OnPropertyChanged();
        }
    }

    //This will update the selected plant in the result listbox
    //This will be used to show the selected plant details
    private Plant _selectedPlantInResult;

    public Plant SelectedPlantInResult {
        get => _selectedPlantInResult;
        set {
            _selectedPlantInResult = value;
            FillAllImages();
            OnPropertyChanged();
            _searchService.FillDetailPlantResult(detailsSelectedPlant, SelectedPlantInResult);

            //Make the currently selected plant in the Result list available in the SearchService
        }
    }

    #endregion


    //geschreven door owen

    #region binding images

    private ImageSource _imageBloei;

    public ImageSource ImageBloei {
        get => _imageBloei;
        set {
            _imageBloei = value;
            OnPropertyChanged();
        }
    }

    private ImageSource _imageHabitus;

    public ImageSource ImageHabitus {
        get => _imageHabitus;
        set {
            _imageHabitus = value;
            OnPropertyChanged();
        }
    }

    private ImageSource _imageBlad;

    public ImageSource ImageBlad {
        get => _imageBlad;
        set {
            _imageBlad = value;
            OnPropertyChanged();
        }
    }

    #endregion
}