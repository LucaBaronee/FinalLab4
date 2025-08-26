using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Stock1.Data;
using Stock1.Models;
using Stock1.View_Model;

namespace Stock1.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoriasController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //public async Task<IActionResult> ImportarExcel(IFormFile archivo)
        //{
        //    if (archivo == null || archivo.Length == 0)
        //    {
        //        ViewBag.Mensaje = "Error: No se ha podido proporcionado un archivo de Excel";
        //        return View();
        //    }
        //    try
        //    {
        //        using (var package = new ExcelPackage(archivo.OpenReadStream()))
        //        {
        //            var worksheet = package.Workbook.Worksheets[0];

        //            var generos = new List<Categoria>();

        //            for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
        //            {

        //                var categoria = new Categoria
        //                {

        //                    descripcion = worksheet.Cells[row, 1].Value.ToString(),


        //                };
        //                generos.Add(categoria);

        //            }
        //            _context.categorias.AddRange(generos);
        //            _context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Mensaje = "Error: La importación ha fallado. Verifica el formato del archivo o consulta los registros del servidor para obtener más detalles.";
        //        Console.WriteLine("Error en la importación: " + ex.Message);

        //        if (ex.InnerException != null)
        //        {
        //            Console.WriteLine("Excepción interna: " + ex.InnerException.Message);
        //        }
        //    }

        //    var applicationDbContext = _context.categorias;
        //    return RedirectToAction("Index", await applicationDbContext.ToListAsync());
        //}



        public async Task<IActionResult> Importar()
        {
            var archivos = HttpContext.Request.Form.Files;
            if (archivos != null && archivos.Count > 0)
            {
                var archivoCSV = archivos[0];
                if (archivoCSV.Length > 0)
                {
                    var pathDestino = Path.Combine(_env.WebRootPath, "importacionesCategoria");
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoCSV.FileName);
                    var rutaDestino = Path.Combine(pathDestino, archivoDestino);
                    using (var fileStream = new FileStream(rutaDestino, FileMode.Create))
                    {
                        archivoCSV.CopyTo(fileStream);
                    };

                    var fileReader = new FileStream(rutaDestino, FileMode.Open);
                    List<string> files = new List<string>();
                    List<Categoria> LibroFiles = new List<Categoria>();

                    StreamReader fileContent = new StreamReader(fileReader, System.Text.Encoding.Default);
                    do
                    {
                        files.Add(fileContent.ReadLine());
                    }
                    while (!fileContent.EndOfStream);

                    if (files.Count > 0)
                    {
                        foreach (var row in files)
                        {
                            string[] data = row.Split(';');
                            if (data.Length != 7)
                            {
                                Categoria categoria = new Categoria();
                                categoria.descripcion = data[0].Trim();
                                LibroFiles.Add(categoria);
                            }
                        }
                        if (LibroFiles.Count() > 0)
                        {
                            _context.AddRange(LibroFiles);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            var appDBContext = _context.categorias.Select(s => s);
            return View("Index", await appDBContext.ToListAsync());
        }





        // GET: Categorias
        public IActionResult Index( int pagina = 1)
        {
            Paginador paginas = new Paginador();
            paginas.PaginaActual = pagina;
            paginas.RegistrosPorPagina = 5;
            var applicationDbContext = _context.categorias.Select(c => c);

            paginas.TotalRegistros = applicationDbContext.Count();

            var mostrarRegistros = applicationDbContext
                .Skip((pagina - 1) * paginas.RegistrosPorPagina)
                .Take(paginas.RegistrosPorPagina);

            CategoriaVM datos = new CategoriaVM()
            {
                categorias = mostrarRegistros.ToList(),
                paginador = paginas
            };
            return View(datos);

            
        }

        // GET: Categorias/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,descripcion")] Categoria categoria)
        {
            if (id != categoria.Id)
            {



                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {



                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.categorias.Any(e => e.Id == id);
        }
    }
}
