using System;
using System.Collections.Generic;

namespace StubHub
{
    class Program
    {
       

        private static List<Customer> customers = new List<Customer> {
            new Customer { Name = "Angel Lopez", City = "Los Angeles" },
            new Customer { Name = "Joe Soap", City = "Boston"},
            new Customer { Name = "Mark Crow", City = "New York"},
            new Customer { Name = "George Perez", City = "Washington DC"},
            new Customer { Name = "Guy Wise", City = "Chicago"},
            new Customer { Name = "Frank Saint", City = "San Francisco"}
        };


        public static void Main(string[] args)
        {
            ConcertFinder finder = new ConcertFinder();
            foreach (Customer customer in customers)
            {
                //Console.WriteLine( "<< {0} in {1} >>",customer.Name, customer.City);
                //List<Event> myEvents = finder.NearestEvents(customer, 5);
                //Console.WriteLine("Closest Shows:");
                //foreach (Event show in myEvents)
                //{
                //    Console.WriteLine("{0}\t\t{1}", show.Name, show.City);
                //}
                //Console.WriteLine("------------");
                finder.PrepareCustomerEmail(customer);
            }

        }

    }

}



