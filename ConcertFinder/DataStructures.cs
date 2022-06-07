using System;
namespace StumpHub
{
	
        public struct Event
        {
            public String City;
            public String Name;
        }


        public struct City
        {
            public String Name;
            public Coordinate Coordinate;

        }

        public class Coordinate
        {
            public Double Latitude;
            public Double Longitude;
            const Double nearest = 5.0;

            public Double NearestLatitude()
            {
                return Math.Round(this.Latitude / nearest) * (int) nearest;
            }

            public Double NearestLongtitude()
            {
                return Math.Round(this.Longitude / nearest) * (int) nearest;
            }

        }

    

        public struct Customer
        {
            public String Name;
            public String City;
        }
    
}

