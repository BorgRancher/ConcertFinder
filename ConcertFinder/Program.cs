using System;
using System.Collections.Generic;

namespace StumpHub
{
    class Program
    {

        private static List<Customer> customers = new List<Customer> {
            new Customer { Name="Angel Lopez", City = "Los Angeles" },
            new Customer { Name = "Joe Soap", City = "Boston"},
            new Customer { Name = "Mark Crow", City = "New York"},
            new Customer { Name = "George Perez", City = "Washington"},
            new Customer { Name = "Guy Wise", City = "Chicago"},
            new Customer { Name = "Frank Saint", City = "San Francisco"}
        };


        public static void Main(string[] args)
        {
            ConcertFinder finder = new ConcertFinder();
            foreach (Customer customer in customers)
            {
                Console.WriteLine(customer.Name + " in " + customer.City);
                List<Event> myEvents = finder.NearestEvents(customer, 5);
                Console.WriteLine("Closest Shows:");
                foreach (Event show in myEvents)
                {
                    Console.WriteLine(show.Name + "\t" + show.City);
                }
                Console.WriteLine("------------");
            }

        }
    }

}



