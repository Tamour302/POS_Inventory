using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace POS_Inventory.Models
{
    public class CreateSaleViewModel
    {

        public int Sale_Id { get; set; }

        public double Amount { get; set; }
        public double Amount_Pay { get; set; }
        public double Grand_Total { get; set; }
        public double Left_Amount { get; set; }
        public double Remaining_Amount { get; set; }
        public DateTime Created_Date { get; set; }
        public string Is_Paid { get; set; }
        public List<Sale_FoodItem> Sale_FoodItems { get; set; }
        public int Customer_Id { get; set; }

      

    } 
    public class Sale_FoodItem {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public double Rate { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }

    }

    public class EditSaleViewModel
    {
        public string CustomerName { get; set; }
        public int Sale_Id { get; set; }

        public double Amount { get; set; }
        public double Amount_Pay { get; set; }
        public double Grand_Total { get; set; }
        public double Left_Amount { get; set; }
        public double Remaining_Amount { get; set; }
        public DateTime Created_Date { get; set; }
        public string Is_Paid { get; set; }
        public List<Sale_FoodItem> Sale_FoodItems { get; set; }
        public int Customer_Id { get; set; }


    }
}