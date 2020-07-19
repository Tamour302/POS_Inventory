using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace POS_Inventory.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodItem_Id { get; set; }
        
        public string Name { get; set; }
        [DefaultValue(true)]
        public bool Is_Active { get; set; }
        [DefaultValue(false)]
        public bool Is_Deleted { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Modified_Date { get; set; }
      
        public FoodItemCategories FoodCategory { get; set; }
       
        public ICollection<Sale_FoodItemRelation> Sale_FoodItems { get; set; }

    }
    public enum FoodItemCategories{
        سبزی,
        پھل

    }
}
