using POS_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace POS_Inventory.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context=new ApplicationDbContext();

      
        // GET: Sales
        public async Task<ActionResult> Index()
        {
            return View(await _context.Sale.ToListAsync());
        }

        public async Task<ActionResult> GetBillList()
        {
            return View();
        }

        // GET: Sales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var sale = await _context.Sale
                .FirstOrDefaultAsync(m => m.Sale_Id == id);
            if (sale == null)
            {
                return View();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create(int? id = 0)
        {
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.FoodItems = _context.FoodItems.ToList();

            CreateSaleViewModel sale = new CreateSaleViewModel();
            sale.Sale_FoodItems = new List<Sale_FoodItem>();
            if (id != 0)
            {
                Customer customer = _context.Customers.Find(id);

                sale.Customer_Id = id.Value;
                sale.Remaining_Amount = customer.Remaining_Amount;
            }
            //sale.Remaining_Amount = _context.Sale.Where(m => m.Customer_Id == id).Sum(c => c.Remaining_Amount);
            return View(sale);
        }

        public ActionResult EditSale(int? id = 0)
        {
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.FoodItems = _context.FoodItems.ToList();
            ViewBag.Food_Item = _context.FoodItems.ToList();
            Sale sale = _context.Sale.Find(id);
            EditSaleViewModel vm = new EditSaleViewModel();
            vm.Sale_FoodItems = new List<Sale_FoodItem>();
            var sale_FoodItem = _context.Sale_FoodItems.Where(m => m.Sale_Id == sale.Sale_Id);
           Customer customer= _context.Customers.Find(sale.Customer_Id);
            vm.Customer_Id = customer.Customer_Id;
            vm.CustomerName = customer.Name;
            vm.Sale_Id = sale.Sale_Id;
            vm.Amount = sale.Amount;
            vm.Amount_Pay = sale.Amount_Pay;
            vm.Grand_Total = sale.Grand_Total;
            vm.Left_Amount = sale.Left_Amount;
            vm.Remaining_Amount = sale.Remaining_Amount;       
            vm.Created_Date = sale.Created_Date;
            vm.Is_Paid = sale.Is_Paid == true ? "Yes" : "No";
         
            if (sale_FoodItem.Count() > 0)
            {
                foreach (var item in sale_FoodItem) {
                    Sale_FoodItem foodItem = new Sale_FoodItem();

                    foodItem.Amount = item.Amount;
                    foodItem.Quantity = item.Quantity;
                    foodItem.Rate = item.Rate;
                    foodItem.Id = item.Sale_FoodItem_Id;
                    foodItem.Name = item.Food_Item.Name;
                    vm.Sale_FoodItems.Add(foodItem);
                } }
           
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditSale(EditSaleViewModel EditSaleViewModel)
        {
            try{ 
            Sale sale = _context.Sale.Find(EditSaleViewModel.Sale_Id);
            sale.Grand_Total = EditSaleViewModel.Grand_Total;
            sale.Amount = EditSaleViewModel.Amount;
            sale.Amount_Pay = EditSaleViewModel.Amount_Pay;
            sale.Remaining_Amount = EditSaleViewModel.Remaining_Amount;
            sale.Left_Amount = EditSaleViewModel.Left_Amount;
            sale.Modified_Date = DateTime.UtcNow;
            _context.Entry(sale).State = EntityState.Modified;
            _context.SaveChanges();
            Customer customer = _context.Customers.Where(m => m.Customer_Id == sale.Customer_Id).FirstOrDefault();
            customer.Remaining_Amount = (customer.Remaining_Amount + sale.Amount) - EditSaleViewModel.Amount_Pay;
                _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
            var items = _context.Sale_FoodItems.Where(m => m.Sale_Id == sale.Sale_Id).ToList();
            if (EditSaleViewModel.Sale_FoodItems == null)
            {
               
                    _context.Sale_FoodItems.RemoveRange(items);
                _context.SaveChanges();

            }
            else {
                    var foods = EditSaleViewModel.Sale_FoodItems.Where(i => i.Id != 0).Select(m=>m.Id).ToList();
                    var editsSales = _context.Sale_FoodItems.Where(m => m.Sale_Id == EditSaleViewModel.Sale_Id).Select(m=>m.Sale_FoodItem_Id).ToList();
                    var left_values= (from e in editsSales
                    from f in foods                   
                    where f != e
                    select e).ToList();
                   var ToRemove= _context.Sale_FoodItems.Where(m => left_values.Any(c => c == m.Sale_FoodItem_Id)).ToList();
                    _context.Entry(customer).State = EntityState.Modified;
                    _context.Sale_FoodItems.RemoveRange(ToRemove);
                    _context.SaveChanges();
                    foreach (var item in EditSaleViewModel.Sale_FoodItems) {
                    if (item.Id != 0)
                    {
                        var vm = _context.Sale_FoodItems.Where(m => m.Sale_FoodItem_Id == item.Id).FirstOrDefault();
                        vm.Amount = item.Amount;
                        vm.Modified_Date = DateTime.UtcNow;
                        vm.Quantity = item.Quantity;
                        vm.Rate = item.Rate;
                        vm.Food_Item_Id = _context.FoodItems.Where(m => m.Name == item.Name).FirstOrDefault().FoodItem_Id;
                            _context.Entry(vm).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else {
                        Sale_FoodItemRelation vm = new Sale_FoodItemRelation();
                        vm.Amount = item.Amount;
                        vm.Modified_Date = DateTime.UtcNow;
                        vm.Quantity = item.Quantity;
                        vm.Rate = item.Rate;
                        vm.Created_Date = DateTime.UtcNow;
                        vm.Sale_Id = EditSaleViewModel.Sale_Id;
                        vm.Food_Item_Id = _context.FoodItems.Where(m => m.Name == item.Name).FirstOrDefault().FoodItem_Id;
                        _context.Sale_FoodItems.Add(vm);
                        _context.SaveChanges();
                    }

                }
            } 
            return Json(EditSaleViewModel.Sale_Id);
        }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));

    }

}
        public ActionResult ViewSale(int? id = 0)
        {
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.FoodItems = _context.FoodItems.ToList();
            Sale sale = _context.Sale.Find(id);
            CreateSaleViewModel vm = new CreateSaleViewModel();
            vm.Sale_FoodItems = new List<Sale_FoodItem>();
            vm.Customer_Id = sale.Customer_Id;
            vm.Amount = sale.Amount;
            vm.Sale_Id = sale.Sale_Id;

            vm.Amount_Pay = sale.Amount_Pay;
            vm.Grand_Total = sale.Grand_Total;
            vm.Left_Amount = sale.Left_Amount;
            vm.Remaining_Amount = sale.Remaining_Amount;

            vm.Created_Date = sale.Created_Date;
            vm.Is_Paid = sale.Is_Paid == true ? "Yes" : "No";
            foreach (var item in _context.Sale_FoodItems.Where(m => m.Sale_Id == sale.Sale_Id))
            {
                Sale_FoodItem foodItem = new Sale_FoodItem();
                foodItem.Amount = item.Amount;
                foodItem.Quantity = item.Quantity;
                foodItem.Rate = item.Rate;
                foodItem.Id = item.Sale_FoodItem_Id;
                foodItem.Name = item.Food_Item.Name;
                vm.Sale_FoodItems.Add(foodItem);
            }

            return View(vm);
        }


        public JsonResult GetCustomers()
        {
            var customers = _context.Customers.Select(
                m => new
                {
                    customerId = m.Customer_Id,
                    name = m.Name,
                    phoneNumber = m.Phone_Number,
                    address = m.Address,
                    shopNumber = m.Shop_Number,
                    //remaining_Amount = _context.Sale.Where(c => c.Customer_Id == m.Customer_Id).Sum(c => c.Remaining_Amount)
                    remaining_Amount = m.Remaining_Amount
                }).ToList();
            return Json(customers,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSales()
        {
            if (_context.Sale.Count() > 0)
            {
                var sales = _context.Sale.Select(
               m => new
               {
                   saleID = m.Sale_Id,
                   name = m.Customer.Name,
                   phoneNumber = m.Customer.Phone_Number,
                   isPaid = m.Is_Paid == true ? "Yes" : "No",
                   createdDate = m.Created_Date.ToShortDateString(),
                   remaining_Amount = m.Customer.Remaining_Amount,
                   canEdit = ((from c in _context.Sale
                               where c.Customer_Id == m.Customer.Customer_Id
                               orderby c.Sale_Id descending
                               select new { SaleId = c.Sale_Id.ToString(), date = m.Created_Date }
                             ).Take(1).FirstOrDefault().SaleId == m.Sale_Id.ToString() && m.Created_Date > DateTime.UtcNow.AddDays(-1)) ? true : false
                   //.Where(m => m.Customer_Id == m.Customer_Id).OrderByDescending(c => c.Sale_Id).FirstOrDefault().Sale_Id == m.Sale_Id ); ? true : false
               }).OrderByDescending(m => m.saleID).ToList();
                return Json(sales, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public ActionResult Create(CreateSaleViewModel CreateSaleViewModel)
        {
            try
            {
               
                var Customer = _context.Customers.Where(m => m.Customer_Id == CreateSaleViewModel.Customer_Id).FirstOrDefault();
             
                    Customer.Remaining_Amount = (Customer.Remaining_Amount + CreateSaleViewModel.Amount)-CreateSaleViewModel.Amount_Pay;
                _context.Entry(Customer).State = EntityState.Modified;

                _context.SaveChanges();

                Sale sale = new Sale();
                sale.Created_Date = DateTime.UtcNow;
                sale.Modified_Date = DateTime.UtcNow;
                sale.Customer_Id = CreateSaleViewModel.Customer_Id;
                sale.Amount = CreateSaleViewModel.Amount;
                sale.Remaining_Amount = Customer.Remaining_Amount;
                sale.Left_Amount = CreateSaleViewModel.Left_Amount;
                sale.Grand_Total = CreateSaleViewModel.Grand_Total;
                sale.Is_Active = true;
                sale.Is_Deleted = false;
                sale.Is_Paid = CreateSaleViewModel.Amount == 0 ? true : false;
                sale.Amount_Pay = CreateSaleViewModel.Amount_Pay;
                _context.Sale.Add(sale);
                _context.SaveChanges();
                var Sale_ID = _context.Sale.OrderByDescending(m => m.Sale_Id).FirstOrDefault().Sale_Id;
                if (CreateSaleViewModel.Sale_FoodItems!=null)
                {
                    foreach (var item in CreateSaleViewModel.Sale_FoodItems)
                    {
                        Sale_FoodItemRelation sale_FoodItem = new Sale_FoodItemRelation();
                        sale_FoodItem.Quantity = item.Quantity;
                        sale_FoodItem.Rate = item.Rate;
                        sale_FoodItem.Amount = item.Amount;
                        sale_FoodItem.Sale_Id = Sale_ID;
                        sale_FoodItem.Created_Date = DateTime.UtcNow;
                        sale_FoodItem.Modified_Date = DateTime.UtcNow;
                        sale_FoodItem.Food_Item_Id = _context.FoodItems.Where(m => m.Name == item.Name).FirstOrDefault().FoodItem_Id;
                        _context.Sale_FoodItems.Add(sale_FoodItem);
                        _context.SaveChanges();
                    }
                }
                var Amount_Pay = CreateSaleViewModel.Amount_Pay;

            //    var sales = _context.Sale.Where(m => m.Customer_Id == CreateSaleViewModel.Customer_Id && m.Is_Paid == false).ToList();
            //    if (sales.Count() > 0 ) { 
            //    foreach (var item in sales)
            //    {
            //        Sale vm = _context.Sale.Where(m => m.Sale_Id == item.Sale_Id).FirstOrDefault();
            //        if (Amount_Pay > 0)
            //        {
            //            if (Amount_Pay >= vm.Remaining_Amount)
            //            {
            //                vm.Is_Paid = true;
            //                Amount_Pay = Amount_Pay - vm.Remaining_Amount;
            //                vm.Remaining_Amount = 0;
            //                vm.Modified_Date = DateTime.UtcNow;
            //                _context.Update(vm);
            //                _context.SaveChanges();
            //            }
            //            else
            //            {
            //                vm.Is_Paid = false;
            //                vm.Remaining_Amount = vm.Remaining_Amount - Amount_Pay;
            //                Amount_Pay = 0;
            //                vm.Modified_Date = DateTime.UtcNow;
            //                _context.Update(vm);
            //                _context.SaveChanges();
            //            }
            //        }

            //        //vm.Remaining_Amount = CreateSaleViewModel.Remaining_Amount;
            //    }
            //}
               
                return Json(Sale_ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));

            }

        }

        public async Task<ActionResult> ViewReceipt(int Sale_ID)
        {
            BillInfoViewModel vm = new BillInfoViewModel();
            var sale=_context.Sale.Where(m => m.Sale_Id == Sale_ID).FirstOrDefault();
            var customer= _context.Customers.Where(m => m.Customer_Id == sale.Customer_Id).FirstOrDefault();
            var Sale_FoodItem = await _context.Sale_FoodItems.Where(m => m.Sale_Id == Sale_ID).ToListAsync();
            vm.Sale_Products = new List<Sale_Product>();
            vm.Customer = customer;
            vm.Created_Date = sale.Created_Date;
            vm.Is_Paid = sale.Is_Paid == true ? "Yes" : "No";
            vm.Amount = sale.Amount;
            vm.Amount_Pay = sale.Amount_Pay;
            vm.Grand_Total = sale.Grand_Total;
            vm.Left_Amount = sale.Left_Amount;
            vm.Remaining_Amount = sale.Remaining_Amount;
            foreach (var item in Sale_FoodItem)
            {
                var item1 = new Sale_Product();
                item1.Rate = item.Rate;
                item1.Quantity = item.Quantity;
                item1.Amount = item.Amount;
                item1.Created_Date = item.Created_Date;
                item1.Name = _context.FoodItems.Where(m => m.FoodItem_Id == item.Food_Item_Id.Value).FirstOrDefault().Name;
                item1.Category = item.Food_Item.FoodCategory.ToString();

                vm.Sale_Products.Add(item1);

            }
            
                

            return View(vm);
        }
        // GET: Sales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var sale = await _context.Sale.FindAsync(id);
            if (sale == null)
            {
                return View();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Edit(int id, Sale sale)
        {
            if (id != sale.Sale_Id)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(sale).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.Sale_Id))
                    {
                        return View();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var sale = await _context.Sale
                .FirstOrDefaultAsync(m => m.Sale_Id == id);
            if (sale == null)
            {
                return View();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sale.FindAsync(id);
            _context.Sale.Remove(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sale.Any(e => e.Sale_Id == id);
        }
        [HttpPost]
        public JsonResult InsertCustomer(Sale_FoodItem customer)
        {
            //Sale_FoodItemRelation viewModel = new Sale_FoodItemRelation();
            //viewModel.Quantity = customer.Sale_FoodItems.FirstOrDefault().Quantity;

            //viewModel.Rate= customer.Sale_FoodItems.FirstOrDefault().Rate;
            //viewModel.Amount = customer.Sale_FoodItems.FirstOrDefault().Amount;
            //viewModel.Created_Date = DateTime.UtcNow;
            //viewModel.Modified_Date = DateTime.UtcNow;


            //_context.Sale_FoodItems .Add(viewModel);
            _context.SaveChanges();


            return Json(customer);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customer customer)
        {
            //using (CustomersEntities entities = new CustomersEntities())
            //{
            //    Customer updatedCustomer = (from c in entities.Customers
            //                                where c.CustomerId == customer.CustomerId
            //                                select c).FirstOrDefault();
            //    updatedCustomer.Name = customer.Name;
            //    updatedCustomer.Country = customer.Country;
            //    entities.SaveChanges();
            //}

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int customerId)
        {
            //using (CustomersEntities entities = new CustomersEntities())
            //{
            //    Customer customer = (from c in entities.Customers
            //                         where c.CustomerId == customerId
            //                         select c).FirstOrDefault();
            //    entities.Customers.Remove(customer);
            //    entities.SaveChanges();
            //}

            return new EmptyResult();
        }
    }
}
