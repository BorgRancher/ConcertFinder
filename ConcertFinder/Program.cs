using System;
using System.Collections.Generic;

namespace StumpHub
{
    class Program
    {
        
        private static Customer customer = new Customer { Name = "Joe Fake", City = "Los Angeles" };

        static void Main(string[] args)
        {
            ConcertFinder finder = new ConcertFinder();
            List<Event> myEvents = finder.NearestEvents(customer, 5);
            Console.WriteLine(customer.Name);
            foreach (Event show in myEvents)
            {
                Console.WriteLine(show.Name +"\t" + show.City);
            }

        }
    }    

}



