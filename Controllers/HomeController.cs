using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stock1.Data;
using Stock1.Models;
using Stock1.View_Model;
using System.Diagnostics;

namespace Stock1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string? busqNombre,  int? categoriaId, int pagina = 1)
        {

            Paginador paginas = new Paginador();
            paginas.PaginaActual = pagina;
            paginas.RegistrosPorPagina = 6;

            var applicationDbContext = _context.productos.Include(p => p.categoria).Select(e => e).Include(s => s.stocks).Select(se => se);
            if (!string.IsNullOrEmpty(busqNombre))
            {
                applicationDbContext = applicationDbContext.Where(e => e.nombre.Contains(busqNombre));
                paginas.ValoresQueryString.Add("busquedaNombre", busqNombre);
            }
            
            if (categoriaId != null && categoriaId > 0)
            {
                applicationDbContext = applicationDbContext.Where(e => e.categoriaId.Equals(categoriaId));
                paginas.ValoresQueryString.Add("categoriaId", categoriaId.ToString());
            }
            paginas.TotalRegistros = applicationDbContext.Count();
            var mostrarRegistros = applicationDbContext
                .Skip((pagina - 1) * paginas.RegistrosPorPagina)
                .Take(paginas.RegistrosPorPagina);
            ProductoVM datos = new ProductoVM()
            {
                productos = mostrarRegistros.ToList(),
                busquedaNombre = busqNombre,
                categoriaId = categoriaId,
                listaCategorias = new SelectList(_context.categorias, "Id", "descripcion"),
                paginador = paginas
            };
            return View(datos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
