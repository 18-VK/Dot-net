using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePractice.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = ""; // The property starts as an empty string instead of null.
        public decimal Price { get; set; }
    }
}
