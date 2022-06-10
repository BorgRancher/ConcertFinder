using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace StubHub
{
    public class ConcertFinder
    {

        private Dictionary<string, int> journeyDistance = new Dictionary<string, int>();
        private List<Event> pricedEvents = new List<Event>();

        private static List<Event> events = new List<Event>{
            new Event{ Name = "Phantom of the Opera", City =  "New York"},
            new Event{ Name = "Metallica", City = "Los Angeles"},
            new Event{ Name = "Metallica", City = "New York"},
            new Event{ Name = "Metallica", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "New York"},
            new Event{ Name = "LadyGaGa", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "Chicago"},
            new Event{ Name = "LadyGaGa", City = "San Francisco"},
            new Event{ Name = "LadyGaGa", City = "Washington DC"}
        };

        private static List<City> cities = new List<City>{
            new City{Name = "New York", Coordinate = new Coordinate{ Latitude = 40.730610, Longitude = -73.935242 } },
            new City{Name = "Boston", Coordinate = new Coordinate{ Latitude = 42.361145, Longitude = -71.057083 } },
            new City{Name = "Chicago", Coordinate = new Coordinate{ Latitude = 41.510395, Longitude = -87.644287} },
            new City{Name = "Washington DC", Coordinate = new Coordinate{ Latitude = 38.900497, Longitude = -77.007507 } },
            new City{Name = "Los Angeles", Coordinate = new Coordinate{ Latitude = 34.052235, Longitude = -118.243683 } },
            new City{Name = "San Francisco", Coordinate = new Coordinate{ Latitude = 37.773972, Longitude = -122.431297 } },
        };


        public void closestEvents(Customer customer, int noOfEvents)
        {
            // Generate ticket prices once
            if (pricedEvents.Count == 0)
            {
                pricedEvents = events.Select(e => new Event { Name = e.Name, City = e.City, Price = getPrice(e).Price }).ToList();
            }

            // Get n nearest shows to customer
            var shows = pricedEvents.Select(e => new { Distance = getCachedDistance(customer.City, e.City), Event = e, Price = e.Price })
                .OrderBy(e => e.Distance).ThenBy(e => e.Price).Take(noOfEvents).ToList();

            Console.WriteLine("Shows For: {0} in {1}", customer.Name, customer.City);
            Console.WriteLine("---------------------------------");
            shows.ForEach(s =>
                addToEmail(customer, s.Event));
        }


        public void PrepareCustomerEmail(Customer customer)
        {
            // collect all events in  customer city
            Console.WriteLine("Composing Email For: {0} in {1}", customer.Name, customer.City);
            Console.WriteLine("---------------------------------");
            closestEvents(customer, 5);

        }
        public void allEventsInCustomerCity(Customer customer)
        {
            List<Event> customerEvents = events.Where(e => e.City == customer.City).ToList();
            customerEvents.ForEach(e => addToEmail(customer, e));

        }

        public void addToEmail(Customer customer, Event @event)
        {
            // This left blank
            Console.WriteLine("{0} - {1} ${2}", @event.City, @event.Name, @event.Price);
        }

        /**
         * Distance is only calculated on new queries, otherwise
         * the cached distance is returned
         */
        private int getCachedDistance(String fromCity, String toCity)
        {
            int maxRetries = 5;
            int retries = 0;
            string key = fromCity + toCity;
            int distance = int.MinValue;

            Stopwatch lookupTimer = new Stopwatch();
            lookupTimer.Start();
            if (fromCity == toCity)
            {
                distance = 0;
            } else
            {
                if (!journeyDistance.TryGetValue(key, out distance))
                {
                    do
                    {
                        Console.WriteLine("\tUsing Distance Service");
                        try
                        {
                            distance = getDistance(fromCity, toCity);
                        }
                        catch (IOException ioex)
                        {
                            Console.WriteLine("\tDistance lookup failed: {0}", ioex.Message);
                            retries++;
                            distance = int.MaxValue;
                            Console.WriteLine("\tWaiting {0} seconds...", retries);
                            Thread.Sleep(1000 * retries); // Linear backoff function
                        }
                    } while (retries < maxRetries && distance == int.MaxValue);


                    if (distance < int.MaxValue) // Lookup did not throw an exception
                    {
                        journeyDistance.Add(key, distance);
                    }

                }
                else
                {
                    Console.WriteLine("\tCached Distance");
                }
            }
            lookupTimer.Stop();

            Console.WriteLine("\t{0} to {1} is {2} miles", fromCity, toCity, distance);
            Console.WriteLine("\tOperation took {0} ms", lookupTimer.ElapsedMilliseconds);


            return distance;
        }
           
       

        /**
         * Haversine is used as a stand-in for the expensive distance call.
         * 
         * The haversine formula determines the great-circle distance between two points on 
         * a sphere given their longitudes and latitudes. Important in navigation, it is a 
         * special case of a more general formula in spherical trigonometry, the law of haversines, 
         * that relates the sides and angles of spherical triangles.
         */

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
            Thread.Sleep(1000); // simulate network call
            return R * h2;
        }

        private int getDistance(String fromCity, String toCity)
        {

            var coordinates =
                cities.Where(c => new List<String> { fromCity, toCity }.Contains(c.Name))
                .Select(c => c.Coordinate).ToArray<Coordinate>();

            Random rnd = new Random();
            int errorOdds = rnd.Next(1, 100);
            if (errorOdds >= 80)
            {
                throw new IOException("Random Exception");
            }


            double distance = getActualDistance(journey: new Journey { start = coordinates[0], end = coordinates[1] });

            return (int)distance;

        }

        private double getActualDistance(Journey journey)
        {
            return Haversine(journey.start, journey.end, DistanceUnit.Miles);
        }

        private Event getPrice(Event @event)
        {
            Random rnd = new Random();
            int priceFactor = rnd.Next(1, 5);
            double price = 50.0 * (double)priceFactor;
            writeDiagnostic(String.Format("Price for {0} in {1} is ${2}",@event.Name, @event.City, price));
            @event.Price = price;
            return @event;
        }

        private void writeDiagnostic(string message)
        {
            Console.WriteLine("\t" + message);
        }

    }

}