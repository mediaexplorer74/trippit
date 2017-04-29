﻿using DigiTransit10.Models;
using DigiTransit10.ViewModels.ControlViewModels;
using System;
using System.Collections.Generic;
using Template10.Mvvm;
using Windows.Devices.Geolocation;
using static DigiTransit10.ExtensionMethods.MapElementExtensions;
using static DigiTransit10.ViewModels.ControlViewModels.StopSearchContentViewModel;

namespace DigiTransit10.Helpers
{
    /// <summary>
    /// Classes used as containers for use with MVVM Light's Messenger service.
    /// </summary>
    public static class MessageTypes
    {
        /// <summary>
        /// Indicates that a TripPlan has been found. The plan itself is stored in the SuspensionState dict.
        /// </summary>
        public class PlanFoundMessage
        {
            public VisualStateType VisualState { get; }

            public PlanFoundMessage(VisualStateType visualState)
            {
                VisualState = visualState;
            }
        }

        public enum VisualStateType
        {
            Narrow,
            Normal,
            Wide
        }

        /// <summary>
        /// Indicates that the active map should center on its collection of displayed favorites.
        /// </summary>
        public class CenterAroundFavoritesOnMap { }

        /// <summary>
        /// Indicates that the active map should center on the given location.
        /// </summary>
        public class CenterMapOnGeoposition
        {
            public BasicGeoposition Position { get; private set; }

            public CenterMapOnGeoposition(BasicGeoposition position)
            {
                Position = position;
            }
        }

        /// <summary>
        /// Indicates that a TripPlanStrip has been tapped, and we should zoom in to display its details.
        /// </summary>
        public class ViewPlanDetails
        {
            public TripItinerary BackingModel { get; private set; }

            public ViewPlanDetails(TripItinerary model)
            {
                BackingModel = model;
            }
        }

        /// <summary>
        /// Indicates that we should back out of a detailed TripPlanStrip view.
        /// </summary>
        public class ViewPlanStrips { }

        /// <summary>
        /// Generic indicator that navigation has recently been canceled.
        /// </summary>
        public class NavigationCanceled { }

        /// <summary>
        /// Message to any listeners that the caller would like to view the details of the given stop.
        /// </summary>
        public class ViewStopDetails
        {
            public StopSearchElementViewModel StopSelected { get; private set; }

            public ViewStopDetails(StopSearchElementViewModel stop)
            {
                StopSelected = stop;
            }
        }

        public class ViewStateChanged
        {
            public StopSearchContentViewModel Sender { get; private set; }
            public StopSearchState ViewState { get; set; }

            public ViewStateChanged(StopSearchContentViewModel sender, StopSearchState newState)
            {
                Sender = sender;
                ViewState = newState;
            }
        }

        /// <summary>
        /// Message to request that the SearchPage change which places it shows on its map.
        /// </summary>
        public class SearchPageMapPlacesChangeRequested
        {
            public BindableBase Sender { get; private set; }
            public IEnumerable<IMapPoi> MapPlaces { get; set; }

            public SearchPageMapPlacesChangeRequested(BindableBase sender, IEnumerable<IMapPoi> places)
            {
                Sender = sender;
                MapPlaces = places;
            }
        }

        /// <summary>
        /// Informs any listeners that the selected Line in the Lines section on the
        /// Search Page has changed.
        /// </summary>
        public class SearchLineSelectionChanged { }

        /// <summary>
        /// Informs any listeners that the selected Nearby Stop on the Search Page has changed.
        /// </summary>
        public class NearbyListSelectionChanged
        {
            public TransitStop SelectedStop { get; set; }

            public NearbyListSelectionChanged(TransitStop stop)
            {
                SelectedStop = stop;
            }
        }

        /// <summary>
        /// Informs any listeners that the selected Stop on the Stops section of the 
        /// Search Page has changed.
        /// </summary>
        public class StopsListSelectionChanged
        {
            public TransitStop SelectedStop { get; set; }
            
            public StopsListSelectionChanged(TransitStop stop)
            {
                SelectedStop = stop;
            }
        }

        public class SetIconState
        {
            public Guid MapIconId { get; }
            public MapIconState NewState { get; }

            public SetIconState(Guid iconId, MapIconState newState)
            {
                MapIconId = iconId;
                NewState = newState;
            }
        }
    }
}
