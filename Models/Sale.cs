using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace POS_Inventory.Models
{
    public class Sale
    {
        [Key]
        public int Sale_Id { get; set; }
        
        public double Amount { get; set; }
        public double Amount_Pay { get; set; }
        public double Grand_Total { get; set; }
        public double Left_Amount { get; set; }
        public double Remaining_Amount { get; set; }

        
        
        [DefaultValue(false)]
        public bool Is_Paid { get; set; }
        [DefaultValue(true)]
        public bool Is_Active { get; set; }
        [DefaultValue(false)]
        public bool Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Modified_Date { get; set; }
       
        public ICollection<Sale_FoodItemRelation> Sale_FoodItems { get; set; }
        public int Customer_Id { get; set; }

        [ForeignKey("Customer_Id")]
        public virtual Customer Customer { get; set; }
       
        
    }
}
