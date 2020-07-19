using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using POS_Inventory.Models;

namespace POS_Inventory.Controllers
{
    [Authorize]
    public class FoodItemController : Controller
    {
        private readonly ApplicationDbContext _context=new ApplicationDbContext();

      

        // GET: FoodItems
        public async Task<ActionResult> Index()
        {
            return View(await _context.FoodItems.Where(m=>m.Is_Deleted==false && m.Is_Active==true).ToListAsync());
        }

        // GET: FoodItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var foodItem = await _context.FoodItems
                .FirstOrDefaultAsync(m => m.FoodItem_Id == id);
            if (foodItem == null)
            {
                return View();
            }

            return View(foodItem);
        }

        // GET: FoodItems/Create
        public ActionResult Create()
        {
            FoodItem fooditem = new FoodItem();
            return View();
        }

        // POST: FoodItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public async Task<ActionResult> Create(FoodItem foodItem)
        {
            if (ModelState.IsValid)
            {
                foodItem.Created_Date = DateTime.UtcNow;
                foodItem.Modified_Date = DateTime.UtcNow;
                foodItem.Is_Active = true;
                foodItem.Is_Deleted = false;
                _context.FoodItems.Add(foodItem);
                //_context.Add(foodItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
                //return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        // GET: FoodItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return View();
            }
            return View(foodItem);
        }

        // POST: FoodItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public async Task<ActionResult> Edit(int id, FoodItem foodItem)
        {
            if (id != foodItem.FoodItem_Id)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    foodItem.Modified_Date = DateTime.UtcNow;
                    foodItem.Is_Active = true;
                    foodItem.Is_Deleted = false;
                    _context.Entry(foodItem).State = EntityState.Modified;
                    //_context.SaveChanges();
                    //_context.Update(foodItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodItemExists(foodItem.FoodItem_Id))
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
            return View(foodItem);
        }

        // GET: FoodItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var foodItem = await _context.FoodItems
                .FirstOrDefaultAsync(m => m.FoodItem_Id == id);
            if (foodItem == null)
            {
                return View();
            }

            return View(foodItem);
        }

        // POST: FoodItems/Delete/5
        [HttpPost, ActionName("Delete")]
       
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            
            foodItem.Modified_Date = DateTime.UtcNow;
            foodItem.Is_Active = false;
            foodItem.Is_Deleted = true;
            // _context.FoodItems.Remove(foodItem);
            _context.Entry(foodItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(e => e.FoodItem_Id == id);
        }
    }
}
