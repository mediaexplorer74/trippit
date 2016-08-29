﻿using System;
using System.Linq;
using DigiTransit10.Models.ApiModels;
using System.Collections.Generic;

namespace DigiTransit10.Models
{
    public class TripItinerary
    {        
        public string StartingPlaceName { get; set; }
        public string EndingPlaceName { get; set; }
        public List<TripLeg> ItineraryLegs { get; set; }

        public TripItinerary(ApiItinerary apiItinerary, string startingPlaceName, string endingPlaceName)
        {
            StartingPlaceName = startingPlaceName;
            EndingPlaceName = endingPlaceName;

            ItineraryLegs = apiItinerary.Legs.Select(x =>
                {
                    bool isStart = x == apiItinerary.Legs.First();
                    bool isEnd = x == apiItinerary.Legs.Last();
                    string startingName = isStart ? StartingPlaceName : x.From.Name;
                    string endingName = isEnd ? EndingPlaceName : x.To.Name;
                    return new TripLeg(x, isStart, isEnd, startingName, endingName);
                }).ToList();            
        }
    }
}
