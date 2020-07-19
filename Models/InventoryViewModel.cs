using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_Inventory.Models
{
    public class InventoryViewModel

    { 
        public string Type { get; set; }

        public string Quantity { get; set; }

        public string Price { get; set; }
        public string ProductName { get; set; }
        public int Id { get; set; }
    }
}
