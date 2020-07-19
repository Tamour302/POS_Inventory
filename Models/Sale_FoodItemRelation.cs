using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace POS_Inventory.Models
{
    public class Sale_FoodItemRelation
    {
        [Key]
        public int Sale_FoodItem_Id { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }

        public DateTime Created_Date { get; set; }
        public DateTime Modified_Date { get; set; }

        public int? Sale_Id { get; set; }

        [ForeignKey("Sale_Id")]
        public virtual Sale Sale { get; set; }

        public int? Food_Item_Id { get; set; }

        [ForeignKey("Food_Item_Id")]
        public virtual FoodItem Food_Item { get; set; }



    }
}
