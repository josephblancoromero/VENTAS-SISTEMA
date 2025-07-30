using Fecomvr1._2.Data;
using Fecomvr1._2.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fecomvr1._2.Controllers
{
    public class FacturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Facturas
        public async Task<IActionResult> Index()
        {
            var facturas = await _context.Facturas.Include(f => f.Productos).ToListAsync();
            return View(facturas);
        }

        // GET: Facturas/Create
        public IActionResult Create()
        {
            var productos = _context.Productos.ToList();
            ViewBag.Productos = productos;

            var factura = new Factura
            {
                Fecha = DateTime.Now,
                Productos = new List<Producto>()
            };

            return View(factura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Factura factura, List<int> Productos_ProductoId, List<decimal> Productos_Precio, List<int> Productos_Stock)
        {
            if (ModelState.IsValid)
            {
                factura.Productos = new List<Producto>();

                for (int i = 0; i < Productos_ProductoId.Count; i++)
                {
                    var producto = await _context.Productos.FindAsync(Productos_ProductoId[i]);
                    if (producto != null)
                    {
                        factura.Productos.Add(new Producto
                        {
                            Nombre = producto.Nombre,
                            Precio = Productos_Precio[i],
                            Stock = Productos_Stock[i]
                        });
                    }
                }

                CalcularIGVYTotal(factura);

                _context.Facturas.Add(factura);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Productos = _context.Productos.ToList();
            return View(factura);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Productos)
                .FirstOrDefaultAsync(f => f.FacturaId == id);

            if (factura != null)
            {
                _context.Facturas.Remove(factura);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }





        private void CalcularIGVYTotal(Factura factura)
        {
            decimal totalProductos = factura.Productos.Sum(p => p.Precio * p.Stock);
            factura.IGV18 = totalProductos / 1.18m;
            factura.TotalIGV18 = totalProductos;
        }


    }
}
