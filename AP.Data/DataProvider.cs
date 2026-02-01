using AP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AP.Data
{
    public class DataProvider
    {
        public IEnumerable<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price = 10.0m },
                new Product { Id = 2, Name = "Product B", Price = 20.0m },
                new Product { Id = 3, Name = "Product C", Price = 30.0m }
            };
        }
    }
}
