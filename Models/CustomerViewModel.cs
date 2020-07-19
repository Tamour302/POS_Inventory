using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace POS_Inventory.Models
{
    public class CustomerViewModel
    {

        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }       
        public string Phone_Number { get; set; }
        public string Shop_Number { get; set; }
       public double Remaining_Amount { get; set; }


    }
}
