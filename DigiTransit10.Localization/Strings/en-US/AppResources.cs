//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------------------
// <auto-generatedInfo>
// 	This code was generated by ResW File Code Generator (http://reswcodegen.codeplex.com)
// 	ResW File Code Generator was written by Christian Resma Helle
// 	and is under GNU General Public License version 2 (GPLv2)
// 
// 	This code contains a helper class exposing property representations
// 	of the string resources defined in the specified .ResW file
// 
// 	Generated: 07/26/2016 01:40:40
// </auto-generatedInfo>
// --------------------------------------------------------------------------------------------------
namespace DigiTransit10.Localization.Strings
{
    using Windows.ApplicationModel.Resources;
    
    
    public partial class AppResources
    {
        
        private static ResourceLoader resourceLoader;
        
        static AppResources()
        {
            string executingAssemblyName;
            executingAssemblyName = Windows.UI.Xaml.Application.Current.GetType().AssemblyQualifiedName;
            string[] executingAssemblySplit;
            executingAssemblySplit = executingAssemblyName.Split(',');
            executingAssemblyName = executingAssemblySplit[1];
            string currentAssemblyName;
            currentAssemblyName = typeof(AppResources).AssemblyQualifiedName;
            string[] currentAssemblySplit;
            currentAssemblySplit = currentAssemblyName.Split(',');
            currentAssemblyName = currentAssemblySplit[1];
            if (executingAssemblyName.Equals(currentAssemblyName))
            {
                resourceLoader = ResourceLoader.GetForCurrentView("AppResources");
            }
            else
            {
                resourceLoader = ResourceLoader.GetForCurrentView(currentAssemblyName + "/AppResources");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "You haven't saved any favorites yet"
        /// </summary>
        public static string Favorites_EmptyList
        {
            get
            {
                return resourceLoader.GetString("Favorites_EmptyList");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Favorites"
        /// </summary>
        public static string Favorites_FavoritesHeader
        {
            get
            {
                return resourceLoader.GetString("Favorites_FavoritesHeader");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Date"
        /// </summary>
        public static string LiteralDate
        {
            get
            {
                return resourceLoader.GetString("LiteralDate");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "From"
        /// </summary>
        public static string MainPage_FromHeader
        {
            get
            {
                return resourceLoader.GetString("MainPage_FromHeader");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "TRIP PLANNING"
        /// </summary>
        public static string MainPage_Header
        {
            get
            {
                return resourceLoader.GetString("MainPage_Header");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "To"
        /// </summary>
        public static string MainPage_ToHeader
        {
            get
            {
                return resourceLoader.GetString("MainPage_ToHeader");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Addresses"
        /// </summary>
        public static string SuggestBoxHeader_Addresses
        {
            get
            {
                return resourceLoader.GetString("SuggestBoxHeader_Addresses");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Transit Stops"
        /// </summary>
        public static string SuggestBoxHeader_TransitStops
        {
            get
            {
                return resourceLoader.GetString("SuggestBoxHeader_TransitStops");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Arrival"
        /// </summary>
        public static string TimeType_Arrival
        {
            get
            {
                return resourceLoader.GetString("TimeType_Arrival");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Departure"
        /// </summary>
        public static string TimeType_Departure
        {
            get
            {
                return resourceLoader.GetString("TimeType_Departure");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Arrival or departure time?"
        /// </summary>
        public static string TimeType_PlaceholderText
        {
            get
            {
                return resourceLoader.GetString("TimeType_PlaceholderText");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Plan a trip"
        /// </summary>
        public static string TripForm_PlanATripHeader
        {
            get
            {
                return resourceLoader.GetString("TripForm_PlanATripHeader");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Planning..."
        /// </summary>
        public static string TripForm_PlanningTrip
        {
            get
            {
                return resourceLoader.GetString("TripForm_PlanningTrip");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Plan"
        /// </summary>
        public static string TripForm_PlanTripButton
        {
            get
            {
                return resourceLoader.GetString("TripForm_PlanTripButton");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Resolving locations..."
        /// </summary>
        public static string TripFrom_GettingsAddresses
        {
            get
            {
                return resourceLoader.GetString("TripFrom_GettingsAddresses");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "TRIP PLANS"
        /// </summary>
        public static string TripResults_TripPlansHeader
        {
            get
            {
                return resourceLoader.GetString("TripResults_TripPlansHeader");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Fewer options"
        /// </summary>
        public static string TripForm_FewerOptions
        {
            get
            {
                return resourceLoader.GetString("TripForm_FewerOptions");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "More options"
        /// </summary>
        public static string TripForm_MoreOptions
        {
            get
            {
                return resourceLoader.GetString("TripForm_MoreOptions");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Time"
        /// </summary>
        public static string LiteralTime
        {
            get
            {
                return resourceLoader.GetString("LiteralTime");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Transit types"
        /// </summary>
        public static string TripForm_TransitTypeHeader
        {
            get
            {
                return resourceLoader.GetString("TripForm_TransitTypeHeader");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "My location"
        /// </summary>
        public static string SuggestBoxHeader_MyLocation
        {
            get
            {
                return resourceLoader.GetString("SuggestBoxHeader_MyLocation");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Now"
        /// </summary>
        public static string LiteralNow
        {
            get
            {
                return resourceLoader.GetString("LiteralNow");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Set to today"
        /// </summary>
        public static string TripForm_SetDateToNow
        {
            get
            {
                return resourceLoader.GetString("TripForm_SetDateToNow");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Set to now"
        /// </summary>
        public static string TripForm_SetTimeToNow
        {
            get
            {
                return resourceLoader.GetString("TripForm_SetTimeToNow");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Origin"
        /// </summary>
        public static string TripPlanStrip_StartingPlaceDefault
        {
            get
            {
                return resourceLoader.GetString("TripPlanStrip_StartingPlaceDefault");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Loading..."
        /// </summary>
        public static string LoadingLiteral
        {
            get
            {
                return resourceLoader.GetString("LoadingLiteral");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Destination"
        /// </summary>
        public static string TripPlanStrip_EndPlaceDefault
        {
            get
            {
                return resourceLoader.GetString("TripPlanStrip_EndPlaceDefault");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Delete"
        /// </summary>
        public static string LiteralDelete
        {
            get
            {
                return resourceLoader.GetString("LiteralDelete");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Favorites"
        /// </summary>
        public static string SuggestBoxHeader_FavoritePlaces
        {
            get
            {
                return resourceLoader.GetString("SuggestBoxHeader_FavoritePlaces");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Now"
        /// </summary>
        public static string CurrentTimePicker_CurrentTime
        {
            get
            {
                return resourceLoader.GetString("CurrentTimePicker_CurrentTime");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Pinned favorites"
        /// </summary>
        public static string TripForm_PinnedFavoritesHeader
        {
            get
            {
                return resourceLoader.GetString("TripForm_PinnedFavoritesHeader");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Remove"
        /// </summary>
        public static string FavoritesList_RemoveItem
        {
            get
            {
                return resourceLoader.GetString("FavoritesList_RemoveItem");
            }
        }
    }
}
