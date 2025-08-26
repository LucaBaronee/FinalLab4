using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stock1.Data;
using Stock1.Models;
using Stock1.View_Model;

namespace Stock1.Controllers
{
    public class StocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public IActionResult Index(int pagina = 1)
        {
            Paginador paginas = new Paginador();
            paginas.PaginaActual = pagina;
            paginas.RegistrosPorPagina = 5;
            var applicationDbContext = _context.stocks.Include(s => s.comprador).Include(s => s.producto);
            paginas.TotalRegistros = applicationDbContext.Count();

           


            var mostrarRegistros = applicationDbContext
                .Skip((pagina - 1) * paginas.RegistrosPorPagina)
                .Take(paginas.RegistrosPorPagina);
            StockVM datos = new StockVM()
            {

                stocks = mostrarRegistros.ToList(),
                paginador = paginas
            };


            return View(datos);
        }
        [Authorize]
        public IActionResult Venta()
        {
            ViewData["compradorId"] = new SelectList(_context.compradores, "Id", "dni");
            ViewData["productoId"] = new SelectList(_context.productos, "Id", "nombre");

            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Venta([Bind("Id,cantidad,fecha,productoId,compradorId")] Stock stock)
        {
     
                if (ModelState.IsValid)
                {

                    stock.cantidad *= -1;
                    _context.Add(stock);
                      await _context.SaveChangesAsync();
                         return RedirectToAction(nameof(Index));
                }
            ViewData["compradorId"] = new SelectList(_context.compradores, "Id", "dni", stock.compradorId);
            ViewData["productoId"] = new SelectList(_context.productos, "Id", "Nombre", stock.productoId);
            return View(stock);
        }
        [Authorize]
        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.stocks
                .Include(s => s.comprador)
                .Include(s => s.producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }
        [Authorize]
        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewData["compradorId"] = new SelectList(_context.compradores, "Id", "dni");
            ViewData["productoId"] = new SelectList(_context.productos, "Id", "nombre");
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,cantidad,fecha,productoId,compradorId")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["compradorId"] = new SelectList(_context.compradores, "Id", "dni", stock.compradorId);
            ViewData["productoId"] = new SelectList(_context.productos, "Id", "nombre", stock.productoId);
            return View(stock);
        }
        [Authorize]
        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["compradorId"] = new SelectList(_context.compradores, "Id", "dni", stock.compradorId);
            ViewData["productoId"] = new SelectList(_context.productos, "Id", "nombre", stock.productoId);
            return View(stock);
        }
        [Authorize]
        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,cantidad,fecha,productoId,compradorId")] Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
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
            ViewData["productoId"] = new SelectList(_context.productos, "Id", "nombre", stock.productoId);
            ViewData["compradorId"] = new SelectList(_context.compradores, "Id", "dni", stock.compradorId);
            return View(stock);
        }
        [Authorize]
        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.stocks
                .Include(s => s.comprador)
                .Include(s => s.producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.stocks.FindAsync(id);
            if (stock != null)
            {
                _context.stocks.Remove(stock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
            return _context.stocks.Any(e => e.Id == id);
        }
    }
}
