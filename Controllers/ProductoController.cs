using Fecomvr1._2.Data;
using Fecomvr1._2.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fecomvr1._2.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            // Lógica para mostrar la lista de productos
            return View(await _context.Productos.ToListAsync());
        }

        public IActionResult Create()
        {
            // Cargar las categorías y proveedores desde la base de datos
            var categorias = _context.Categorias.Select(c => new SelectListItem
            {
                Value = c.CategoriaId.ToString(),
                Text = c.Nombre
            }).ToList();

            var proveedores = _context.Proveedores.Select(p => new SelectListItem
            {
                Value = p.ProveedorId.ToString(),
                Text = p.Nombre
            }).ToList();

            ViewBag.Categorias = categorias;
            ViewBag.Proveedores = proveedores;

            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Stock,CategoriaId,ProveedorId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Cargar las categorías y proveedores desde la base de datos
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewBag.Proveedores = new SelectList(_context.Proveedores, "Id", "Nombre");

            return View(producto);
        }

        // Acción para mostrar la vista de editar
        public IActionResult Editar(int id)
        {
            var producto = _context.Productos.Find(id);

            if (producto == null)
            {
                return NotFound();
            }

            // Cargar las categorías y proveedores desde la base de datos
            var categorias = _context.Categorias.Select(c => new SelectListItem
            {
                Value = c.CategoriaId.ToString(),
                Text = c.Nombre,
                Selected = c.CategoriaId == producto.CategoriaId
            }).ToList();

            var proveedores = _context.Proveedores.Select(p => new SelectListItem
            {
                Value = p.ProveedorId.ToString(),
                Text = p.Nombre,
                Selected = p.ProveedorId == producto.ProveedorId
            }).ToList();

            ViewBag.Categorias = categorias;
            ViewBag.Proveedores = proveedores;

            return View(producto);
        }

        // Acción para procesar la edición
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("ProductoId,Nombre,Precio,Stock,CategoriaId,ProveedorId")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
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

            // Lógica para cargar categorías y proveedores si es necesario

            return View(producto);
        }

        // Acción para mostrar la vista de eliminar
        public IActionResult Eliminar(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // Acción para procesar la eliminación
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para verificar si un producto existe
        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }

        [HttpGet]
        public JsonResult ObtenerProductosExistente()
        {
            var productosExistente = _context.Productos.ToList();
            return Json(productosExistente);
        }
    }
}
