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
            new Customer { Name = "Matthew Luke", City="Los Angeles"},
            new Customer { Name = "Guy Wise", City = "Chicago"},
            new Customer { Name = "Frank Saint", City = "San Francisco"},
            new Customer { Name = "Oliver Rock", City = "Boston"}
        };


        public static void Main(string[] args)
        {
            ConcertFinder finder = new ConcertFinder();
            foreach (Customer customer in customers)
            {
                // Question 1
                // finder.allEventsInCustomerCity(customer);

                //Question 2
                finder.PrepareCustomerEmail(customer);

            }

        }

    }

}



