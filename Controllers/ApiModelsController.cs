using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test_Framework;
using Test_Framework.Models;

namespace Test_Framework.Controllers
{
    public class ApiModelsController : Controller
    {
        private readonly TestContext _context;

        public ApiModelsController(TestContext context)
        {
            _context = context;
        }

        // GET: ApiModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Apis.ToListAsync());
        }

        // GET: ApiModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiModel = await _context.Apis
                .FirstOrDefaultAsync(m => m.ID == id);
            if (apiModel == null)
            {
                return NotFound();
            }

            return View(apiModel);
        }

        // GET: ApiModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApiModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Uri")] ApiModel apiModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apiModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apiModel);
        }

        // GET: ApiModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiModel = await _context.Apis.FindAsync(id);
            if (apiModel == null)
            {
                return NotFound();
            }
            return View(apiModel);
        }

        // POST: ApiModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Uri")] ApiModel apiModel)
        {
            if (id != apiModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apiModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiModelExists(apiModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(apiModel);
        }

        // GET: ApiModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiModel = await _context.Apis
                .FirstOrDefaultAsync(m => m.ID == id);
            if (apiModel == null)
            {
                return NotFound();
            }

            return View(apiModel);
        }

        // POST: ApiModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiModel = await _context.Apis.FindAsync(id);
            _context.Apis.Remove(apiModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApiModelExists(int id)
        {
            return _context.Apis.Any(e => e.ID == id);
        }
    }
}
