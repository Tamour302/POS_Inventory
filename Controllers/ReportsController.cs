using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using POS_Inventory.Models;

namespace POS_Inventory.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context=new ApplicationDbContext();
     
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCustomerWiseSale(int id)
        {
            var customer = _context.Customers.Where(m => m.Customer_Id == id).FirstOrDefault();
            var sales= _context.Sale.Where(m => m.Customer_Id == customer.Customer_Id && m.Created_Date.Date == DateTime.UtcNow.Date).ToList();
            var data= _context.Sale_FoodItems.ToList().Where(x => sales.Any(m=>m.Sale_Id==x.Sale_Id.Value)).ToList();

            var results= data.GroupBy(l => l.Food_Item_Id.Value).ToList();
            var a = data.GroupBy(i => _context.FoodItems.Find(i.Food_Item_Id.Value).Name)
              .Select(g => new InventoryViewModel
              {
                  ProductName = g.Key,
                  Type = g.Select(i=>i.Food_Item.FoodCategory.ToString()).First(),
                  Quantity = g.Sum(i => i.Quantity).ToString(),
                  Price = g.Sum(i => i.Amount).ToString(),
              });

            //List<InventoryViewModel> result = data.GroupBy(l => l.Food_Item_Id.Value).Select(cl => new InventoryViewModel
            //{
            //    Id = cl.First().Food_Item.FoodItem_Id,
            //    ProductName = cl.First().Amount.ToString(),
            //    Quantity = cl.Sum(c => c.Quantity).ToString(),
            //    Price = cl.Sum(c => c.Amount).ToString(),
            //}).ToList();


            return View(a);
        }
        public ActionResult GetOverallSale()
        {
            var sales = _context.Sale.Where(m => m.Created_Date.Date == DateTime.UtcNow.Date).ToList();
            var data = _context.Sale_FoodItems.ToList().Where(x => sales.Any(m => m.Sale_Id == x.Sale_Id.Value)).ToList();

            var results = data.GroupBy(l => l.Food_Item_Id.Value).ToList();
            var a = data.GroupBy(i => _context.FoodItems.Find(i.Food_Item_Id.Value).Name)
              .Select(g => new InventoryViewModel
              {
                  ProductName = g.Key,
                  Type = g.Select(i => i.Food_Item.FoodCategory.ToString()).First(),
                  Quantity = g.Sum(i => i.Quantity).ToString(),
                  Price = g.Sum(i => i.Amount).ToString(),
              });

            //List<InventoryViewModel> result = data.GroupBy(l => l.Food_Item_Id.Value).Select(cl => new InventoryViewModel
            //{
            //    Id = cl.First().Food_Item.FoodItem_Id,
            //    ProductName = cl.First().Amount.ToString(),
            //    Quantity = cl.Sum(c => c.Quantity).ToString(),
            //    Price = cl.Sum(c => c.Amount).ToString(),
            //}).ToList();


            return View(a);
        }

    }
}