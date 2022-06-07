using System;
using System.Collections.Generic;
using System.Linq;

namespace StumpHub
{
    public class ConcertFinder
    {
        private static List<Event> events = new List<Event>{
            new Event{ Name = "Phantom of the Opera", City =  "New York" },
            new Event{ Name = "Metallica", City = "Los Angeles"},
            new Event{ Name = "Metallica", City = "New York"},
            new Event{ Name = "Metallica", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "New York"},
            new Event{ Name = "LadyGaGa", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "Chicago"},
            new Event{ Name = "LadyGaGa", City = "San Francisco"},
            new Event{ Name = "LadyGaGa", City = "Washington"}
        };

        private static List<City> cities = new List<City>{
            new City{Name = "New York", Coordinate = new Coordinate{ Latitude = 40.730610, Longitude = -73.935242 } },
            new City{Name = "Boston", Coordinate = new Coordinate{ Latitude = 42.361145, Longitude = -71.057083 } },
            new City{Name = "Chicago", Coordinate = new Coordinate{ Latitude = 41.510395, Longitude = -87.644287} },
            new City{Name = "Washington", Coordinate = new Coordinate{ Latitude = 47.751076, Longitude = -120.740135 } },
            new City{Name = "Los Angeles", Coordinate = new Coordinate{ Latitude = 34.052235, Longitude = -118.243683 } },
            new City{Name = "San Francisco", Coordinate = new Coordinate{ Latitude = 37.773972, Longitude = -122.431297 } },
        };

        public List<Event> NearestEvents(Customer customer, int numberOfEvents)
        {
            List<Event> eventsFound = new List<Event>();
            Dictionary<Double, String> closestCities = ClosestCites(customer);

            foreach (KeyValuePair<Double, String> entry in closestCities)
            {
                List<Event> cityEvents = events.Where(e => e.City == entry.Value).ToList();
                eventsFound.AddRange(cityEvents);

            }

            return eventsFound.Take(numberOfEvents).ToList();


        }

        
        private Dictionary<Double, String> ClosestCites(Customer customer)
        {
            City start = cities.Where(c => c.Name == customer.City).First();
            List<City> destinations = cities.Where(c => c.Name != customer.City &&
                Math.Abs(start.Coordinate.NearestLatitude() - c.Coordinate.NearestLatitude()) <= 5 &&
                Math.Abs(start.Coordinate.NearestLongtitude() - c.Coordinate.NearestLongtitude()) <= 5)
                .ToList();

            Dictionary<Double, String> drivingDistances = new Dictionary<Double, String>();
            drivingDistances.Add(0.0, start.Name);

            foreach (City city in destinations)
            {
                Double distance = Haversine(start.Coordinate, city.Coordinate, DistanceUnit.Miles);
                Console.WriteLine(city.Name + "\t" + distance);
                drivingDistances.Add(distance, city.Name);

            }
            return drivingDistances;
        }

        public enum DistanceUnit
        {
            Miles,
            Kilometers
        }

        private double Haversine(Coordinate pos1, Coordinate pos2, DistanceUnit unit = DistanceUnit.Miles)
        {
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = (pos2.Latitude - pos1.Latitude).ToRadians();
            var lng = (pos2.Longitude - pos1.Longitude).ToRadians();
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(pos1.Latitude.ToRadians()) * Math.Cos(pos2.Latitude.ToRadians()) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }
    }
}

