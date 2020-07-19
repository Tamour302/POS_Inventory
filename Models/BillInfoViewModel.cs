using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_Inventory.Models
{
    public class BillInfoViewModel
    {
        public double Amount { get; set; }
        public double Amount_Pay { get; set; }
        public double Grand_Total { get; set; }
        public double Left_Amount { get; set; }
        public double Remaining_Amount { get; set; }
        public List<Sale_Product> Sale_Products{get; set;}
        public Customer Customer { get; set; }
        public DateTime Created_Date { get; set; }
        public string Is_Paid { get; set; }
    }
    public class Sale_Product { 
     public string Name { get; set; }
     public string Category { get; set; }
    public double Amount { get; set; }
    public double Quantity { get; set; } 
     public double Rate { get; set; }
    public DateTime Created_Date { get; set; }

}
}
