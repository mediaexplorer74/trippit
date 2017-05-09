﻿using DigiTransit10.ExtensionMethods;
using DigiTransit10.Helpers;
using DigiTransit10.Localization.Strings;
using DigiTransit10.Models;
using DigiTransit10.Models.Geocoding;
using DigiTransit10.Services;
using DigiTransit10.Services.SettingsServices;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static DigiTransit10.Models.ModelEnums;

namespace DigiTransit10.Controls
{
    public sealed partial class DigiTransitSearchBox : UserControl, INotifyPropertyChanged
    {
        private static readonly IPlaceComparer _placeComparer = new IPlaceComparer();
        private static readonly SettingsService _settingsService;
        private static readonly INetworkService _networkService;
        private static readonly IFavoritesService _favoritesService;

        private CancellationTokenSource _currentToken = new CancellationTokenSource();
        private bool _favoritesInserted = false;

        private readonly GroupedPlaceList _stopList = new GroupedPlaceList(ModelEnums.PlaceType.Stop,
            AppResources.SuggestBoxHeader_TransitStops);

        private readonly GroupedPlaceList _addressList = new GroupedPlaceList(ModelEnums.PlaceType.Address,
            AppResources.SuggestBoxHeader_Addresses);

        private readonly GroupedPlaceList _userCurrentLocationList = new GroupedPlaceList(ModelEnums.PlaceType.UserCurrentLocation,
            AppResources.SuggestBoxHeader_MyLocation);

        private readonly GroupedPlaceList _favoritePlacesList = new GroupedPlaceList(ModelEnums.PlaceType.FavoritePlace,
            AppResources.SuggestBoxHeader_FavoritePlaces);

        private ObservableCollection<GroupedPlaceList> _suggestedPlaces = new ObservableCollection<GroupedPlaceList>();
        public ObservableCollection<GroupedPlaceList> SuggestedPlaces
        {
            get { return _suggestedPlaces; }
            set
            {
                if (_suggestedPlaces != value)
                {
                    _suggestedPlaces = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isWaiting = false;
        public bool IsWaiting
        {
            get { return _isWaiting; }
            set
            {
                if (_isWaiting != value)
                {
                    _isWaiting = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsFavoriteButtonEnabled
        {
            get
            {
                if(SelectedPlace == null)
                {
                    return false;
                }
                if(SelectedPlace.Type == ModelEnums.PlaceType.NameOnly
                    || SelectedPlace.Type == ModelEnums.PlaceType.UserCurrentLocation                    
                    || SelectedPlace.Lat == default(float)
                    || SelectedPlace.Lon == default(float))
                {
                    return false;
                }
                return true;
            }
        }

        private string _favoriteButtonGlyph = FontIconGlyphs.HollowStar;
        public string FavoriteButtonGlyph
        {
            get { return _favoriteButtonGlyph; }
            set
            {
                if(_favoriteButtonGlyph != value)
                {
                    _favoriteButtonGlyph = value;
                    RaisePropertyChanged();
                }
            }
        }

        static DigiTransitSearchBox()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }
            _networkService = ServiceLocator.Current.GetInstance<INetworkService>();
            _settingsService = ServiceLocator.Current.GetInstance<SettingsService>();
            _favoritesService = ServiceLocator.Current.GetInstance<IFavoritesService>();
        }

        public DigiTransitSearchBox()
        {
            this.InitializeComponent();
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            _userCurrentLocationList.Add(new Place { Name = AppResources.SuggestBoxHeader_MyLocation, Type = ModelEnums.PlaceType.UserCurrentLocation });
            SuggestedPlaces.Add(_userCurrentLocationList);
            //Load favorites in the Loaded handler           
            SuggestedPlaces.Add(_stopList);
            SuggestedPlaces.Add(_addressList);
            PlacesCollection.Source = SuggestedPlaces;

            this.SearchBox.GotFocus += SearchBox_GotFocus;
            this.Loaded += DigiTransitSearchBox_Loaded;
            this.Unloaded += DigiTransitSearchBox_Unloaded;
        }

        private async void DigiTransitSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            _favoritesService.FavoritesChanged += FavoritesChanged;

            if (!_favoritesInserted)
            {
                _favoritesInserted = true;
                foreach (var favorite in (await _favoritesService.GetFavoritesAsync())
                    .Where(x => x is FavoritePlace))
                {
                    _favoritePlacesList.AddSorted(favorite as FavoritePlace);
                }
                int insertIndex = IsUserCurrentLocationListed ? 1 : 0;
                SuggestedPlaces.Insert(insertIndex, _favoritePlacesList);
            }
        }

        private void DigiTransitSearchBox_Unloaded(object sender, RoutedEventArgs e)
        {
            _favoritesService.FavoritesChanged -= FavoritesChanged;
        }

        public static readonly DependencyProperty SelectedPlaceProperty =
            DependencyProperty.Register("SelectedPlace", typeof(IPlace), typeof(DigiTransitSearchBox), new PropertyMetadata(null,
                async (obj, args) =>
                {
                    DigiTransitSearchBox box = obj as DigiTransitSearchBox;
                    if (box == null)
                    {
                        return;
                    }

                    IPlace newPlace = args.NewValue as IPlace;
                    if (newPlace != null)
                    {
                        box.SearchText = newPlace.Name;
                    }
                    else
                    {
                        box.SearchText = "";
                    }

                    box.RaisePropertyChanged(nameof(IsFavoriteButtonEnabled));

                    if (box.SelectedPlace == null)
                    {
                        box.FavoriteButtonGlyph = FontIconGlyphs.HollowStar;
                        return;
                    }
                    if (box.SelectedPlace.Type == ModelEnums.PlaceType.NameOnly
                        || box.SelectedPlace.Type == ModelEnums.PlaceType.UserCurrentLocation
                        || box.SelectedPlace.Lat == default(float)
                        || box.SelectedPlace.Lon == default(float))
                    {
                        box.FavoriteButtonGlyph = FontIconGlyphs.HollowStar;
                        return;
                    }
                    if (box.SelectedPlace.Type == ModelEnums.PlaceType.FavoritePlace
                        || (await _favoritesService.GetFavoritesAsync()).Where(x => x is FavoritePlace)
                            .Any(x => ((FavoritePlace)x).Lat == box.SelectedPlace.Lat
                                && ((FavoritePlace)x).Lon == box.SelectedPlace.Lon))
                    {
                        box.FavoriteButtonGlyph = FontIconGlyphs.FilledStar;
                        return;
                    }
                    else
                    {
                        box.FavoriteButtonGlyph = FontIconGlyphs.HollowStar;
                    }
                }));
        public IPlace SelectedPlace
        {
            get { return (IPlace)GetValue(SelectedPlaceProperty); }
            set { SetValue(SelectedPlaceProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(DigiTransitSearchBox), new PropertyMetadata(""));
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DigiTransitSearchBox), new PropertyMetadata(null));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty FavoriteTappedCommandProperty =
            DependencyProperty.Register("FavoriteTappedCommand", typeof(ICommand), typeof(DigiTransitSearchBox), new PropertyMetadata(null));
        public ICommand FavoriteTappedCommand
        {
            get { return (ICommand)GetValue(FavoriteTappedCommandProperty); }
            set { SetValue(FavoriteTappedCommandProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(DigiTransitSearchBox), new PropertyMetadata(null));
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty TextBoxHeightProperty =
            DependencyProperty.Register("TextBoxHeight", typeof(double), typeof(DigiTransitSearchBox), new PropertyMetadata(48.0, OnTextBoxHeightChanged));
        private static void OnTextBoxHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as DigiTransitSearchBox;
            if (_this == null)
            {
                return;
            }

            if (!(e.NewValue is double))
            {
                return;
            }

            double availableSeachBoxSpace = (double)e.NewValue;
            _this.SearchBox.Height = availableSeachBoxSpace;
        }
        public double TextBoxHeight
        {
            get { return (double)GetValue(TextBoxHeightProperty); }
            set { SetValue(TextBoxHeightProperty, value); }
        }

        public static readonly new DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(DigiTransitSearchBox), new PropertyMetadata(20, OnFontSizeChanged));
        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as DigiTransitSearchBox;
            if (_this == null)
            {
                return;
            }

            if (!(e.NewValue is double))
            {
                return;
            }

            double newSize = (double)e.NewValue;

            _this.SearchBox.FontSize = newSize;
        }
        public new double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty ShowFavoritesButtonProperty =
            DependencyProperty.Register("ShowFavoritesButton", typeof(bool), typeof(DigiTransitSearchBox), new PropertyMetadata(true));       
        public bool ShowFavoritesButton
        {
            get { return (bool)GetValue(ShowFavoritesButtonProperty); }
            set { SetValue(ShowFavoritesButtonProperty, value); }
        }

        public static readonly DependencyProperty IsUserCurrentLocationListedProperty =
            DependencyProperty.Register(nameof(IsUserCurrentLocationListed), typeof(bool), typeof(DigiTransitSearchBox), new PropertyMetadata(true,
                OnIsUserCurrentLocationListedChanged));

        private static void OnIsUserCurrentLocationListedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as DigiTransitSearchBox;
            if(_this == null)
            {
                return;
            }

            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;
            if(newValue == oldValue)
            {
                return;
            }

            if (newValue == true)
            {
                _this.SuggestedPlaces.Insert(0, _this._userCurrentLocationList);
            }
            else
            {
                _this.SuggestedPlaces.Remove(_this._userCurrentLocationList);
            }
        }

        public bool IsUserCurrentLocationListed
        {
            get { return (bool)GetValue(IsUserCurrentLocationListedProperty); }
            set { SetValue(IsUserCurrentLocationListedProperty, value); }
        }

        public static readonly DependencyProperty OpenOnFocusProperty =
            DependencyProperty.Register(nameof(OpenOnFocus), typeof(bool), typeof(DigiTransitSearchBox), new PropertyMetadata(true));
        /// <summary>
        /// Determines whether or not the suggestion list opens automatically when the searchbox gets focus.
        /// </summary>
        public bool OpenOnFocus
        {
            get { return (bool)GetValue(OpenOnFocusProperty); }
            set { SetValue(OpenOnFocusProperty, value); }
        }
               
        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (String.IsNullOrWhiteSpace(SearchBox.Text))
            {
                _stopList.Clear();
                _addressList.Clear();

                // this has to happen after the list clearing. clearing _stopList seems to force a SuggestionChosen(), which grabs the first item in the still-filled _addressList.
                SearchText = "";
                SelectedPlace = null;
                return;
            }

            if (!args.CheckCurrent())
            {
                SearchText = SearchBox.Text;
            }

            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                SelectedPlace = new Place
                {
                    Name = SearchText,
                    Type = ModelEnums.PlaceType.NameOnly
                };
                await TriggerSearch(SearchText);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //cancel any outstanding Searches, set text field to the Place chosen
            IsWaiting = false;
            _currentToken?.Cancel();

            SelectedPlace = (IPlace)args.SelectedItem;
            SearchText = SelectedPlace.Name;
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //todo: should this also be an actual event, as well as the Command?
            IsWaiting = false;
            _currentToken?.Cancel();

            if (args.ChosenSuggestion != null)
            {
                SelectedPlace = (IPlace)args.ChosenSuggestion;
                SearchText = SelectedPlace.Name;

                if (Command != null && Command.CanExecute(SelectedPlace))
                {
                    Command.Execute(SelectedPlace);
                }
            }
            else
            {
                SelectedPlace = new Place
                {
                    Confidence = null,
                    StringId = null,
                    Lat = 0,
                    Lon = 0,
                    Name = args.QueryText,
                    Type = ModelEnums.PlaceType.NameOnly
                };
                SearchText = args.QueryText;

                if(SearchBox.IsSuggestionListOpen)
                {
                    SearchBox.IsSuggestionListOpen = false;
                }

                if (Command != null && Command.CanExecute(args.QueryText))
                {
                    Command.Execute(args.QueryText);
                }
            }
        }

        private async Task TriggerSearch(string searchString)
        {
            if (_currentToken != null && !_currentToken.IsCancellationRequested)
            {
                _currentToken.Cancel();
            }

            _currentToken = new CancellationTokenSource();

            Task addressTask = SearchAddresses(searchString, _currentToken.Token);
            Task stopTask = SearchStops(searchString, _currentToken.Token);

            IsWaiting = true;
            try
            {
                await Task.WhenAll(addressTask, stopTask);
            }
            catch (OperationCanceledException)
            {
                IsWaiting = false;
                return;
            }
            IsWaiting = false;
        }

        private async Task SearchAddresses(string searchString, CancellationToken token)
        {
            //not sending the token into the network request, because they get called super frequently, and cancelling a network request is a HUGE perf hit on phones
            var response = await _networkService.SearchAddressAsync(searchString);
            if (token.IsCancellationRequested || response.IsFailure)
            {
                return;
            }

            GeocodingResponse result = response.Result;

            //Remove entries in old list not in new response
            List<string> responseIds = result.Features.Select(x => x.Properties.Id).ToList();
            List<IPlace> stalePlaces = _addressList.Where(x => !responseIds.Contains(x.StringId)).ToList();
            foreach (var stale in stalePlaces)
            {
                _addressList.Remove(stale);
            }

            foreach (var place in result.Features)
            {
                if (_addressList.Any(x => x.StringId == place.Properties.Id))
                {
                    IPlace address = _addressList.First(x => x.StringId == place.Properties.Id);
                    if (address.Confidence != place.Properties.Confidence)
                    {
                        address.Confidence = place.Properties.Confidence;
                    }
                    continue;
                }
                string name = place.Properties.Name;
                Place foundPlace = new Place
                {
                    StringId = place.Properties.Id,
                    Name = name,
                    Lat = (float)place.Geometry.Coordinates[1],
                    Lon = (float)place.Geometry.Coordinates[0],
                    Type = ModelEnums.PlaceType.Address,
                    Confidence = place.Properties.Confidence
                };
                _addressList.AddSorted(foundPlace, _placeComparer);
            }
            _addressList.SortInPlace(x => x, _placeComparer);
        }

        private async Task SearchStops(string searchString, CancellationToken token)
        {
            //not sending the token into the network request, because they get called super frequently, and cancelling a network request is a HUGE perf hit on phones
            var response = await _networkService.GetStopsAsync(searchString);
            if (token.IsCancellationRequested || response.IsFailure)
            {
                return;
            }

            var result = response.Result;

            //Remove entries in old list not in new response
            List<string> responseIds = result.Select(x => x.GtfsId).ToList();
            List<IPlace> stalePlaces = _stopList.Where(x => !responseIds.Contains(x.StringId)).ToList();
            foreach (var stale in stalePlaces)
            {
                _stopList.Remove(stale);
            }

            foreach (var stop in result)
            {
                if (_stopList.Any(x => x.StringId == stop.GtfsId))
                {
                    continue;
                }
                Place foundPlace = new Place
                {
                    StringId = stop.GtfsId,
                    Name = stop.Name,
                    Lat = stop.Coords.Latitude,
                    Lon = stop.Coords.Longitude,
                    Type = ModelEnums.PlaceType.Stop
                };
                if (!String.IsNullOrWhiteSpace(stop.Code))
                {
                    foundPlace.Name += $", {stop.Code}";
                }
                _stopList.AddSorted(foundPlace, _placeComparer);
            }
            _stopList.SortInPlace(x => x, _placeComparer);
        }                

        private async void AddToFavoriteButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = true;

            FavoritePlace existingFavorite = (await _favoritesService.GetFavoritesAsync()).FirstOrDefault(x => x == SelectedPlace) as FavoritePlace;
            if (existingFavorite != null)
            {                
                _favoritesService.RemoveFavorite(existingFavorite);
                SelectedPlace = new Place
                {
                    Confidence = SelectedPlace.Confidence,
                    Lat = SelectedPlace.Lat,
                    Lon = SelectedPlace.Lon,
                    Id = SelectedPlace.Id,
                    Name = SelectedPlace.Name,
                    StringId = SelectedPlace.StringId,
                    Type = PlaceType.Address
                };
                return;
            }

            var newFavoritePlace = new FavoritePlace
            {
                FontIconGlyph = FontIconGlyphs.FilledStar,
                Id = Guid.NewGuid(),
                IconFontFace = ((FontFamily)App.Current.Resources[Constants.SymbolThemeFontResource]).Source,
                IconFontSize = Constants.SymbolFontSize,
                Lat = SelectedPlace.Lat,
                Lon = SelectedPlace.Lon,
                Name = SelectedPlace.Name,
                Type = PlaceType.FavoritePlace,
                UserChosenName = SelectedPlace.Name
            };
            _favoritesService.AddFavorite(newFavoritePlace);
            SelectedPlace = newFavoritePlace;
        }

        private void FavoritesChanged(object sender, FavoritesChangedEventArgs args)
        {
            if (args.AddedFavorites?.Count > 0)
            {
                foreach (IPlace added in args.AddedFavorites.OfType<IPlace>())
                {
                    _favoritePlacesList.AddSorted(added);
                }
            }
            if (args.RemovedFavorites?.Count > 0)
            {
                foreach (IPlace removed in args.RemovedFavorites.OfType<IPlace>())
                {
                    _favoritePlacesList.Remove(removed);
                }
            }

            RaisePropertyChanged(nameof(IsFavoriteButtonEnabled));
            RaisePropertyChanged(nameof(FavoriteButtonGlyph));
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Don't open the search box if the user is just adding a favorite or clearing the box
            if (!SearchBox.IsSuggestionListOpen
                && e.OriginalSource as Button == null
                && OpenOnFocus)
            {
                SearchBox.IsSuggestionListOpen = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
