using IV.AccesoDatos.Repositorio.IRepositorio;
using IV.Modelos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;//Permitira cargar imagenes

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Producto producto = new Producto();
            if (id == null)
            {//Para crear nuevo registro
                return View(producto);
            }
            //para actualizar
            producto = _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Producto producto)
        {
            if (ModelState.IsValid)
            {
                if (producto.Id == 0)
                {
                    _unidadTrabajo.Producto.Agregar(producto);
                }
                else
                {
                    _unidadTrabajo.Producto.Actualizar(producto);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Producto.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var marcaDb = _unidadTrabajo.Producto.Obtener(id);
            if (marcaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _unidadTrabajo.Producto.Remover(marcaDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrada exitosamente" });
        }
        #endregion
    }
}
