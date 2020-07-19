using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS_Inventory.Models;

using System.Web.Mvc;
using System.Data.Entity;

namespace POS_Inventory.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context=new ApplicationDbContext();

        //private  CustomerController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        // GET: Customers
        public async Task<ActionResult> Index()
        {
           //var Sales= await _context.Customers.Where(m => m.Is_Active == true && m.Is_Deleted == false).ToListAsync();
           // List<CustomerViewModel> vm = new List<CustomerViewModel>();

           // foreach (var item in Sales) {
           //     CustomerViewModel customer = new CustomerViewModel();
           //     customer.Customer_Id = item.Customer_Id;
           //     customer.Address = item.Address;
           //     customer.Shop_Number = item.Shop_Number;
           //     customer.Phone_Number = item.Phone_Number;
           //     customer.Name = item.Name;
           //     customer.Remaining_Amount= _context.Sale.Where(m => m.Customer_Id == item.Customer_Id).Sum(c => c.Remaining_Amount);
           //     vm.Add(customer);
           // }
            return View();
        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Customer_Id == id);
            if (customer == null)
            {
                return View();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(Customer customer)
        {
            //if (ModelState.IsValid)
            {
                customer.Created_Date = DateTime.UtcNow;
                customer.Modified_Date = DateTime.UtcNow;
                customer.Is_Active = true;
                customer.Is_Deleted = false;              
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return View();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit(int id,Customer customer)
        {
            //if (id != customer.Customer_Id)
            //{
            //    return View();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    
                    customer.Modified_Date = DateTime.UtcNow;
                    _context.Entry(customer).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (!CustomerExists(customer.Customer_Id))
                    {
                        return View();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Customer_Id == id);
            if (customer == null)
            {
                return View();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);         
            customer.Modified_Date = DateTime.UtcNow;
            customer.Is_Active = false;
            customer.Is_Deleted = true;
            _context.Entry(customer).State = EntityState.Modified;
            // _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Customer_Id == id);
        }
    }
}
