using IV.AccesoDatos.Repositorio.IRepositorio;
using IV.Modelos;
using IV.Modelos.ViewModels;
using IV.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;

namespace InventarioVenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class CompaniaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;//Permitira cargar imagenes

        public CompaniaController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var compania = _unidadTrabajo.Compania.ObtenerTodos();
            return View(compania);
        }

        public IActionResult Upsert(int? id)
        {
            CompaniaVM companiaVM = new CompaniaVM()
            {
                Compania = new Compania(),
                BodegaLista = _unidadTrabajo.Bodega.ObtenerTodos().Select(m => new SelectListItem
                {
                    Text = m.Nombre,
                    Value = m.Id.ToString()
                }),
            };

            if (id == null)
            {//Para crear nuevo registro
                return View(companiaVM);
            }
            //para actualizar
            companiaVM.Compania = _unidadTrabajo.Compania.Obtener(id.GetValueOrDefault());
            if (companiaVM.Compania == null)
            {
                return NotFound();
            }

            return View(companiaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CompaniaVM companiaVM)
        {
            if (ModelState.IsValid)
            {
                //Cargar imagenes
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"img\compania");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (companiaVM.Compania.LogoUrl != null)
                    {
                        //Esto es para editar, necesitamos borrar la imagen anterior
                        var imagenPath = Path.Combine(webRootPath, companiaVM.Compania.LogoUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagenPath))
                        {
                            System.IO.File.Delete(imagenPath);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    companiaVM.Compania.LogoUrl = @"\img\compania\" + fileName + extension;
                }
                else
                {
                    //Si en el update el usuario no cambia la imagen
                    if (companiaVM.Compania.Id != 0)
                    {
                        Compania companiaDB = _unidadTrabajo.Compania.Obtener(companiaVM.Compania.Id);
                        companiaVM.Compania.LogoUrl = companiaDB.LogoUrl;
                    }
                }

                if (companiaVM.Compania.Id == 0)
                {
                    _unidadTrabajo.Compania.Agregar(companiaVM.Compania);
                }
                else
                {
                    _unidadTrabajo.Compania.Actualizar(companiaVM.Compania);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                companiaVM.BodegaLista = _unidadTrabajo.Bodega.ObtenerTodos().Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });

                if (companiaVM.Compania.Id != 0)
                {
                    companiaVM.Compania = _unidadTrabajo.Compania.Obtener(companiaVM.Compania.Id);
                }
            }
            return View(companiaVM.Compania);
        }

        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Compania.ObtenerTodos(incluirPropiedades: "Bodega");
            return Json(new { data = todos });
        }
        #endregion
    }
}
