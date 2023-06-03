# Transport for Finland

# Transport for Finland - Public APIs

- Transport for: Finland

- Category: Transportation

## Introduction to Digitransit API

[DigiTransit](https://digitransit.fi/en/) is a Finnish development project that aims to provide accessible and sustainable public 
transport solutions. To achieve this goal, they've developed an open API for developers to use.

The Digitransit API provides a wealth of information related to public transportation, such as real-time vehicle locations and schedules, trip planning, and information on nearby stations and stops.

## Getting Started

To access the Digitransit API, developers must register for an API key. Once you have your key, you can authenticate requests by including it in the header of your API calls.

Authorization: <YOUR_API_KEY>
Example API Requests
Get Routes
To get all available routes for a given area, use the following API call:

const baseUrl = 'https://api.digitransit.fi/routing/v1/routers/hsl/';
const endPoint = 'index/graphql';

const query = `{
    routes {
        gtfsId
        shortName
        longName
    }
}`;

const options = {
    method: 'POST',
    headers: {
        'Content-Type': 'application/graphql',
        'Authorization': `Bearer ${YOUR_API_KEY_HERE}`,
    },
    body: query,
};

fetch(baseUrl + endPoint, options)
    .then(response => response.json())
    .then(data => console.log(data.data.routes));
Get Real-time Stops
To get real-time information for stops near a given location, use the following API call:

const baseUrl = 'https://api.digitransit.fi/routing/v1/routers/hsl/';
const endPoint = 'index/graphql';

const query = `{
    stopsByRadius(lat: LATITUDE, lon: LONGITUDE, radius: RADIUS) {
        edges {
            node {
                stop {
                    gtfsId
                    name
                    lat
                    lon
                    stoptimesWithoutPatterns {
                        scheduledArrival
                        realtimeArrival
                        headsign
                        trip {
                            route {
                                shortName
                                longName
                                mode
                            }
                        }
                    }
                }
            }
        }
    }
}`;

const options = {
    method: 'POST',
    headers: {
        'Content-Type': 'application/graphql',
        'Authorization': `Bearer ${YOUR_API_KEY_HERE}`,
    },
    body: query,
};

fetch(baseUrl + endPoint, options)
    .then(response => response.json())
    .then(data => console.log(data.data.stopsByRadius.edges));
Get Trip Plan
To plan a trip between two locations, use the following API call:

const baseUrl = 'https://api.digitransit.fi/routing/v1/routers/hsl/';
const endPoint = 'plan';

const options = {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${YOUR_API_KEY_HERE}`,
    },
    body: JSON.stringify({
        from: { lat: FROM_LATITUDE, lon: FROM_LONGITUDE },
        to: { lat: TO_LATITUDE, lon: TO_LONGITUDE },
        date: '2022-12-31',
        time: '08:30:00',
        arriveBy: false,
        modes: 'BUS,RAIL',
    }),
};

fetch(baseUrl + endPoint, options)
    .then(response => response.json())
    .then(data => console.log(data.plan.itineraries));
Conclusion
The Digitransit API provides a powerful tool for developers looking to build applications related to public transportation. With its comprehensive documentation and easy-to-use API, it's never been easier to build new and innovative solutions to help people move around cities.

Visit to [Transport for Finland website](https://digitransit.fi/en/developers/).

## Additional Resources

https://portal-api.digitransit.fi/